USE [MinistryPlatform]
GO

IF OBJECT_ID('dbo.api_crds_Get_Pledge_Campaign_Summary') IS NULL -- Check if SP Exists
	EXEC('CREATE PROCEDURE dbo.api_crds_Get_Pledge_Campaign_Summary AS SET NOCOUNT ON;') -- Create dummy/empty SP
GO

-- 04/13/2017
-- This procedure provides data for an overview of total giving vs. commitments for
-- a pledge campaign.  This supports the /leaveyourmark page on CR.net.
ALTER PROCEDURE dbo.api_crds_Get_Pledge_Campaign_Summary (
	@Pledge_Campaign_Id INT 
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Current_Date DATETIME = CONVERT(DATE, GETDATE())

	DECLARE @Start_Date DATE;
	DECLARE @End_Date DATE;

	SELECT
		@Start_Date = pc.Start_Date,
		@End_Date = pc.End_Date
	FROM
		Pledge_Campaigns pc
	WHERE
		pc.Pledge_Campaign_ID = @Pledge_Campaign_Id
	;

	; WITH PledgeData (Pledge_Id, Total_Committed, Total_Given, Percent_Money, Percent_Time)
	AS
	(
		SELECT
			p.Pledge_Id,
			p.Total_Pledge,
			agg.Total_Given,
			PercentMoney =		-- percent of commitment met so far
				CASE
					WHEN p.Total_Pledge > 0 AND agg.Total_Given IS NOT NULL THEN 100.0 * agg.Total_Given / p.Total_Pledge
					ELSE 0
				END,
			PercentTime =		-- percent of days elapsed so far (unique per pledge)
				CASE
					WHEN @Current_Date > @End_Date THEN 100					-- past the campaign end date
					WHEN @Current_Date < p.First_Installment_Date THEN 0	-- before the pledge start date
					ELSE -- DaysUsed / DaysAvailable
						100.0 * DATEDIFF(DAY, p.First_Installment_Date, DATEADD(DAY, 1, @Current_Date)) / 
						DATEDIFF(DAY, p.First_Installment_Date, DATEADD(DAY, 1, @End_Date))
				END
		FROM
			Pledges p
			INNER JOIN (
				SELECT
					p.Pledge_Id,
					SUM(dd.Amount) as Total_Given
				FROM
					Pledges p
					LEFT JOIN Donation_Distributions dd ON dd.Pledge_Id = p.Pledge_Id
				WHERE
					Pledge_Campaign_Id = @Pledge_Campaign_Id AND Pledge_Status_Id IN (1, 2)	-- Active, Completed
				GROUP BY
					p.Pledge_Id
			) AS agg ON agg.Pledge_Id = p.Pledge_Id
	)

	SELECT
		Start_Date = @Start_Date,
		End_Date = @End_Date,
		Total_Given = (SELECT SUM(Total_Given) FROM PledgeData WHERE Total_Given IS NOT NULL),
		Total_Committed = (SELECT SUM(Total_Committed) FROM PledgeData),
		Not_Started_Count = (SELECT COUNT(*) FROM PledgeData WHERE COALESCE(Total_Given, 0) <= 0),
		On_Pace_Count = (SELECT COUNT(*) FROM PledgeData WHERE Percent_Money >= Percent_Time),
		Completed_Count = (SELECT COUNT(*) FROM PledgeData WHERE COALESCE(Total_Given, 0) >= Total_Committed),
		Total_Count = (SELECT COUNT(*) FROM PledgeData)
	WHERE
		@Start_Date IS NOT NULL
	;
END
GO


-- setup permissions for API User in MP

DECLARE @procName nvarchar(100) = N'api_crds_Get_Pledge_Campaign_Summary'
DECLARE @procDescription nvarchar(100) = N'Retrieves summary information for a pledge campaign'

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_API_Procedures] WHERE [Procedure_Name] = @procName)
BEGIN
        INSERT INTO [dbo].[dp_API_Procedures] (
                 Procedure_Name
                ,Description
        ) VALUES (
                 @procName
                ,@procDescription
        )
END


DECLARE @API_ROLE_ID int = 62;
DECLARE @API_ID int;

SELECT @API_ID = API_Procedure_ID FROM [dbo].[dp_API_Procedures] WHERE [Procedure_Name] = @procName;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Role_API_Procedures] WHERE [Role_ID] = @API_ROLE_ID AND [API_Procedure_ID] = @API_ID)
BEGIN
        INSERT INTO [dbo].[dp_Role_API_Procedures] (
                 [Role_ID]
                ,[API_Procedure_ID]
                ,[Domain_ID]
        ) VALUES (
                 @API_ROLE_ID
                ,@API_ID
                ,1
        )
END
GO
