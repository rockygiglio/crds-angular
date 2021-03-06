USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* Sets the 'AssignPledgesNightly' in 'Administration>Configuration Settings' to 'NO' to prevent the built-in MP data_quality script from assigning donations to pledges */
BEGIN TRANSACTION
	UPDATE [dbo].[dp_Configuration_Settings]
	SET
	[Value] = 'No'
	WHERE Key_Name = 'AssignPledgesNightly'

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO [dbo].[dp_Configuration_Settings]
				   ([Application_Code]
				   ,[Key_Name]
				   ,[Value]
				   ,[Description]
				   ,[Domain_ID]
				   ,[_Warning])
			 VALUES
				   ('SERVICES'
				   ,'AssignPledgesNightly'
				   ,'No'
				   ,'If set to YES, Donation Distributions assigned to programs participating in pledge campaigns will be assign to active pledges by the donor or spouse of the donor to the campaign.'
				   ,1
				   ,'Incorrectly editing configuration settings can result in application errors. Changes may not take effect immediately. Contact support@thinkministry.com if you would like assistance.')
	END
COMMIT TRANSACTION


/****** Object:  StoredProcedure [dbo].[crds_service_assign_pledges_nightly]    Script Date: 10/27/2015 ******/
/* This is the modified procedure taken from [dbo].[service_data_quality] that assigns donations to pledges. */
/* It checks the Donation Status for 'Succeeded' or 'Deposited' before it assigns the pledge.                */

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name= 'crds_service_assign_pledges_nightly')
	DROP PROCEDURE [dbo].[crds_service_assign_pledges_nightly]
GO

CREATE PROCEDURE [dbo].[crds_service_assign_pledges_nightly]

	@DomainID INT

AS
BEGIN

DECLARE @FromDate DATETIME = GETDATE() - 90 --Config setting?
DECLARE @UnassignedDonorID INT = 2

DECLARE @DefaultDonorID INT = (select top 1 value 
							from    dp_Configuration_Settings
							where   lower(Key_Name) = 'DefaultDonorID'
									and ISNUMERIC(Value)=1
									and domain_ID = @DomainID)

SELECT C.Display_Name
,PC.Campaign_name
,Prog.Program_Name
,DD.Amount
,D.Donation_Date
,DD.Donation_Distribution_ID, D.Donor_ID, DD.Program_ID, Prog.Pledge_Campaign_ID, ISNULL(Spouse_Donor_ID,0) AS Spouse_Donor_ID
INTO #DD
FROM Donation_Distributions DD
 INNER JOIN Programs Prog ON Prog.Program_ID = DD.Program_ID
 INNER JOIN Pledge_Campaigns PC ON PC.Pledge_Campaign_ID = Prog.Pledge_Campaign_ID
 INNER JOIN Donations D ON D.Donation_ID = DD.Donation_ID
 INNER JOIN Donors Do ON Do.Donor_ID = D.Donor_ID
 INNER JOIN Contacts C ON C.Contact_ID = Do.Contact_ID
 OUTER APPLY (SELECT TOP 1 Donor_ID AS Spouse_Donor_ID FROM Contacts SP INNER JOIN Donors SPDo ON SPDo.Contact_ID = SP.Contact_ID AND SPDo.Statement_Type_ID = 2 AND Do.Statement_Type_ID = 2 WHERE SP.Household_Id = C.Household_ID AND SP.Contact_ID <> C.Contact_ID AND SP.Household_Position_ID = 1 AND C.Household_Position_ID = 1) SP
WHERE DD.Pledge_ID IS NULL
 AND D.Donation_Date BETWEEN @FromDate AND GETDATE()
 AND D.Donor_ID NOT IN (@DefaultDonorID,@UnassignedDonorID)
 AND (D.Donation_Status_ID = 2 OR D.Donation_Status_ID = 4)

CREATE INDEX IX_TempDD_DistributionID ON #DD(Donation_Distribution_ID)
CREATE INDEX IX_TempDD_DonorID ON #DD(Donor_ID)
CREATE INDEX IX_TempDD_SpouseDonorID ON #DD(Spouse_Donor_ID)
 
DECLARE @DonationDistributionID INT, @DonorID INT, @ProgramID INT, @PledgeCampaignID INT, @SpouseDonorID INT, @DonationDate DATETIME
DECLARE @NewPledgeID INT, @DDAuditITemID INT

DECLARE @Date_Time DATETIME = GETDATE()

DECLARE CursorDD CURSOR FAST_FORWARD FOR
	SELECT Donation_Distribution_ID, Donor_ID, Program_ID, Pledge_Campaign_ID, Spouse_Donor_ID,Donation_Date
	FROM #DD
	WHERE EXISTS (SELECT 1 FROM Pledges PL 
	WHERE PL.Donor_ID 
	IN (#DD.Donor_ID,#DD.Spouse_Donor_ID) AND PL.Pledge_Status_ID = 1 AND Pledge_Campaign_ID = #DD.Pledge_Campaign_ID AND PL.First_Installment_Date <= #DD.Donation_Date)

	OPEN CursorDD
	FETCH NEXT FROM CursorDD INTO @DonationDistributionID, @DonorID, @ProgramID, @PledgeCampaignID, @SpouseDonorID, @DonationDate
		WHILE @@FETCH_STATUS = 0
			BEGIN

			SELECT @NewPledgeID = COALESCE(
			(SELECT TOP 1 Pledge_ID FROM Pledges
			 WHERE Pledge_Campaign_ID = @PledgeCampaignID AND Pledge_Status_ID = 1 AND Donor_ID = @DonorID AND First_Installment_Date <= @DonationDate),
			 (SELECT TOP 1 Pledge_ID FROM Pledges 
			 WHERE Pledge_Campaign_ID = @PledgeCampaignID AND Pledge_Status_ID = 1 AND Donor_ID = @SpouseDonorID AND First_Installment_Date <= @DonationDate))
			FROM Donation_Distributions
			WHERE Pledge_ID IS NULL 
			 AND Donation_Distribution_ID = @DonationDistributionID

			UPDATE Donation_Distributions SET Pledge_ID = @NewPledgeID WHERE Donation_Distribution_ID = @DonationDistributionID

			IF @NewPledgeID > 0
			BEGIN
			INSERT INTO [dp_Audit_Log] ([Table_Name],[Record_ID],[Audit_Description],[User_Name],[User_ID],[Date_Time])
			VALUES ('Donation_Distributions',@DonationDistributionID,'Updated','Svc Mngr',0,@Date_Time)
			SET @DDAuditITemID = SCOPE_IDENTITY()
			INSERT INTO dp_Audit_Detail (Audit_Item_ID, Field_Name, Field_Label, Previous_Value, New_Value, Previous_ID, New_ID)
			VALUES(@DDAuditItemID, 'Pledge_ID', 'Pledge',NULL,'Pledge Assigned',NULL,@NewPledgeID)
			END		 

			SET @NewPledgeID = NULL

			FETCH NEXT FROM CursorDD INTO @DonationDistributionID, @DonorID, @ProgramID, @PledgeCampaignID, @SpouseDonorID,@DonationDate

			END
	
	CLOSE CursorDD
	DEALLOCATE CursorDD

END
GO

/****** Object:  StoredProcedure [dbo].[service_church_specific]    Script Date: 10/27/2015 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name= 'service_church_specific')
	DROP PROCEDURE [dbo].[service_church_specific]
GO


CREATE PROCEDURE [dbo].[service_church_specific]

	@DomainID INT	

AS
BEGIN

EXEC crds_service_assign_pledges_nightly @DomainID

END
GO




