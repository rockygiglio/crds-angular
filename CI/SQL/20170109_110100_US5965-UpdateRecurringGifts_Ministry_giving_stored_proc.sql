USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_RecurringGifts_Ministry_giving]') AND TYPE IN (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_RecurringGifts_Ministry_giving] AS' 
END
GO

ALTER PROCEDURE [dbo].[report_CRDS_RecurringGifts_Ministry_giving]

AS BEGIN

SELECT 'MONTHLY GENERAL' AS recurrance,COUNT(d.Donor_ID) num_of_gifts, SUM(rg.amount) gifts_for_ministry, SUM(rg.amount * 1) total_monthly_ministry_expected, SUM(rg.amount * 12) total_yearly_ministry_expected
		FROM dbo.Recurring_Gifts rg
		INNER JOIN dbo.Recurring_Gift_Frequencies rgf ON rg.Frequency_ID = rgf.Frequency_ID
		INNER JOIN dbo.Programs p ON rg.Program_ID = p.Program_ID
		INNER JOIN dbo.Donors d ON rg.Donor_ID = d.Donor_ID
		INNER JOIN dbo.Congregations c ON p.Congregation_Id = c.Congregation_Id
		WHERE rgf.Frequency_ID = 2 --monthly
		AND p.Program_ID = 3 --General Giving
		AND rg.end_date IS NULL
		AND c.Accounting_Company_Id = 1 --crossroads
		UNION ALL
		SELECT 'WEEKLY GENERAL' AS recurrance,COUNT(d.Donor_ID) num_of_gifts, SUM(rg.amount) gifts_for_ministry, SUM(rg.amount *52/12) total_monthly_ministry_expected, SUM(rg.amount * 52) total_yearly_ministry_expected
		FROM dbo.Recurring_Gifts rg
		 INNER JOIN dbo.Recurring_Gift_Frequencies rgf ON  rg.Frequency_ID = rgf.Frequency_ID
		 INNER JOIN dbo.Programs p ON rg.Program_ID = p.Program_ID
		 INNER JOIN dbo.Donors d ON rg.Donor_ID = d.Donor_ID
		 INNER JOIN dbo.Congregations c ON p.Congregation_Id = c.Congregation_Id
		WHERE rgf.Frequency_ID = 1 --weekly
		AND p.Program_ID = 3 --General Giving
		AND rg.end_date IS NULL
		AND c.Accounting_Company_Id = 1 --crossroads
END
GO


