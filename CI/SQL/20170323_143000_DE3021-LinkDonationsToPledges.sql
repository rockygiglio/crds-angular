IF IndexProperty(Object_Id('Donors'), 'IX_Donors__ContactID_DomainID', 'IndexId') IS NULL
	CREATE INDEX IX_Donors__ContactID_DomainID ON Donors(Contact_ID, Domain_ID);

IF IndexProperty(Object_Id('Pledges'), 'IX_Pledges__DonorID_DomainID', 'IndexId') IS NULL
	CREATE INDEX IX_Pledges__DonorID_DomainID ON Pledges(Donor_ID, Domain_ID);

IF OBJECT_ID('dbo.crds_service_assign_pledges_nightly') IS NULL -- Check if SP Exists
	EXEC('CREATE PROCEDURE dbo.crds_service_assign_pledges_nightly AS SET NOCOUNT ON;') -- Create dummy/empty SP
GO

-- This procedure is intended to be run daily to find orphaned donations that should be
-- linked with an existing pledge.  Orphaned donations can occur in two scenarios:
--
-- 1. A donation is not linked with a pledge until the Donation_Status is Succeeded or
-- Deposited (i.e., wait for Stripe).  Prior to that, the Pledge_ID should remain null.
--
-- 2. A person can donate to a campaign prior to making a pledge to that campaign.  For
-- example, I can donate to a campaign for 12 months, then decide to make a pledge.  Once
-- my pledge has been created, we want to go back and link all previous donations to my
-- new pledge.
--
-- The implementation below will look back 4 years based on conversations with Nick.
-- Campaigns can remain open up to 3 years, so we'll go back 4 years just to be safe.  

ALTER PROCEDURE dbo.crds_service_assign_pledges_nightly (
	@DomainID INT
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @FromDate DATETIME = DATEADD(yy, -4, GETDATE());	-- look back 4 years
	DECLARE @UnassignedDonorID INT = 2;
	DECLARE @DefaultDonorID INT = (SELECT TOP 1 Value 
								FROM dp_Configuration_Settings
								WHERE LOWER(Key_Name) = 'DefaultDonorID'
										AND ISNUMERIC(Value) = 1
										AND Domain_ID = @DomainID);

	DECLARE @DonationsMissingPledge TABLE (
		Donation_Distribution_ID INT NOT NULL,
		Donor_ID INT NOT NULL,
		Spouse_Donor_ID INT NOT NULL,
		Program_ID INT NOT NULL,
		Pledge_Campaign_ID INT NOT NULL,
		Pledge_ID INT,

		PRIMARY KEY(Donation_Distribution_ID)
	);

	-- Find all donation distributions within our date window where Pledge_ID is null
	-- and collect some additional info about the donation distribution.
	INSERT INTO @DonationsMissingPledge
	SELECT
		DD.Donation_Distribution_ID
		,D.Donor_ID
		,ISNULL(Spouse_Donor_ID, 0) AS Spouse_Donor_ID
		,DD.Program_ID
		,Prog.Pledge_Campaign_ID
		,DD.Pledge_ID			-- always null in our result set
	FROM
		Donations D
		INNER JOIN Donation_Distributions DD ON DD.Donation_ID = D.Donation_ID
		INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
		INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
		INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
		OUTER APPLY (
			SELECT TOP 1 Donor_ID AS Spouse_Donor_ID
			FROM
				Contacts SP
				INNER JOIN Donors SPDo ON SPDo.Contact_ID = SP.Contact_ID AND SPDo.Statement_Type_ID = 2 AND Do.Statement_Type_ID = 2
			WHERE
				SP.Household_Id = C.Household_ID AND
				SP.Contact_ID <> C.Contact_ID AND
				SP.Household_Position_ID = 1 AND
				C.Household_Position_ID = 1
		) SP
	WHERE
		DD.Pledge_ID IS NULL
		AND D.Donation_Date BETWEEN @FromDate AND GETDATE()
		AND D.Donor_ID NOT IN (@DefaultDonorID, @UnassignedDonorID)
		AND (D.Donation_Status_ID = 2 OR D.Donation_Status_ID = 4)
		AND Prog.Pledge_Campaign_ID IS NOT NULL
	;

	-- For each donation distribution in our list, see if we can find an applicable Pledge
	-- made by the donor or the donor's spouse that should be linked to the distribution.
	-- Note that the COALESCE clause below purposely filters out duplicates (shouldn't happen
	-- in theory, but the database doesn't prevent it) and also favors the donor's Pledge
	-- over the spouse's Pledge if both have a pledge.  The ORDER BY for Pledge_ID is
	-- simply to prevent randomness if there are duplicates.
	UPDATE DMP
	SET
		DMP.Pledge_ID = COALESCE(
			(SELECT TOP 1 Pledge_ID FROM Pledges
			WHERE Pledge_Status_ID = 1 AND Pledge_Campaign_ID = DMP.Pledge_Campaign_ID AND Donor_ID = DMP.Donor_ID
			ORDER BY Pledge_ID),

			(SELECT TOP 1 Pledge_ID FROM Pledges
			WHERE Pledge_Status_ID = 1 AND Pledge_Campaign_ID = DMP.Pledge_Campaign_ID AND Donor_ID = DMP.Spouse_Donor_ID
			ORDER BY Pledge_ID)
		)
	FROM
		@DonationsMissingPledge DMP
	WHERE
		DMP.Pledge_ID IS NULL
	;

	-- Now it's time to actually change data.  We'll update the Donation Distributions that
	-- we were able link with a new Pledge_ID, and also add entries to the audit log for each
	-- donation distribution that we change.  This whole process will be wrapped in a 
	-- transaction so changes are All Or Nothing.
	BEGIN TRY
		BEGIN TRAN

		-- update donation distributions with new Pledge_ID
		UPDATE DD
		SET
			DD.Pledge_ID = DMP.Pledge_ID
		FROM
			Donation_Distributions DD
			INNER JOIN @DonationsMissingPledge DMP ON DMP.Donation_Distribution_ID = DD.Donation_Distribution_ID
		WHERE
			DMP.Pledge_ID IS NOT NULL
		;

		DECLARE @NumRowsUpdated INT = @@ROWCOUNT;

		-- add top-level audit log items
		DECLARE @AuditLogItems TABLE (
			Audit_Item_ID INT NOT NULL,
			Pledge_ID INT NOT NULL,

			PRIMARY KEY(Audit_Item_ID)
		);

		-- Note this is basically an INSERT, but we're using MERGE in order to gain
		-- access to Pledge_ID in the OUTPUT clause because it's not part of the
		-- SELECT list
		MERGE INTO dp_Audit_Log USING @DonationsMissingPledge AS DMP ON 1 = 0
		WHEN NOT MATCHED AND DMP.Pledge_ID IS NOT NULL THEN
			INSERT
				(Table_Name, Record_ID, Audit_Description, User_Name, User_ID, Date_Time)
			VALUES (
				'Donation_Distributions'
				,DMP.Donation_Distribution_ID
				,'Updated'
				,'Svc Mngr'
				,0
				,GETDATE()
			)
			OUTPUT
				INSERTED.Audit_Item_ID, DMP.Pledge_ID INTO @AuditLogItems(Audit_Item_ID, Pledge_ID)
		;

		-- add audit log detail items
		INSERT INTO dp_Audit_Detail
			(Audit_Item_ID, Field_Name, Field_Label, Previous_Value, New_Value, Previous_ID, New_ID)
		SELECT
			A.Audit_Item_ID, 'Pledge_ID', 'Pledge', NULL, 'Pledge Assigned', NULL, A.Pledge_ID
		FROM
			@AuditLogItems A
		ORDER BY
			Audit_Item_ID
		;

		COMMIT
		PRINT 'crds_service_assign_pledges_nightly success: Updated ' + CONVERT(varchar(12), @NumRowsUpdated) + ' records'
	END TRY

	BEGIN CATCH
		PRINT 'crds_service_assign_pledges_nightly failed: ' + ERROR_MESSAGE()
		IF @@TRANCOUNT > 0
			ROLLBACK
	END CATCH
END
