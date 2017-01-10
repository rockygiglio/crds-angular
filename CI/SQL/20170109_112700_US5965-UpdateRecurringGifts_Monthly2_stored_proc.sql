USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_RecurringGifts_MonthlyII]') AND TYPE IN (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_RecurringGifts_MonthlyII] AS' 
END
GO

ALTER PROCEDURE [dbo].[report_CRDS_RecurringGifts_MonthlyII]

AS BEGIN


		SELECT 'MONTHLY II' AS recurrance,COUNT(d.Donor_ID) num_of_gifts, SUM(rg.amount) gifts_for_II, SUM(rg.amount * 1) total_monthly_II_expected, SUM(rg.amount * 12) total_yearly_II_expected
		FROM dbo.Recurring_Gifts rg
		INNER JOIN dbo.Recurring_Gift_Frequencies rgf ON rg.Frequency_ID = rgf.Frequency_ID 
		INNER JOIN dbo.Programs p ON rg.Program_ID = p.Program_ID 
		INNER JOIN dbo.Donors d ON rg.Donor_ID = d.Donor_ID
		INNER JOIN dbo.Congregations c ON p.Congregation_Id = c.Congregation_Id
		WHERE 	
		  rgf.Frequency_ID = 2 --monthly
		  AND p.Program_ID = 146 --General Giving
		  AND rg.end_date IS NULL
		  AND c.Accounting_Company_Id = 1 --crossroads
		UNION ALL
		SELECT 'WEEKLY II' AS recurrance,COUNT(d.Donor_ID) num_of_gifts, SUM(rg.amount) gifts_for_II,SUM(rg.amount *52/12) total_monthly_II_expected, SUM(rg.amount * 52) total_yearly_II_expected
		FROM dbo.Recurring_Gifts rg
		INNER JOIN dbo.Recurring_Gift_Frequencies rgf ON rg.Frequency_ID = rgf.Frequency_ID
		INNER JOIN dbo.Programs p ON rg.Program_ID = p.Program_ID
		INNER JOIN dbo.Donors d ON  rg.Donor_ID = d.Donor_ID
		INNER JOIN dbo.Congregations c ON p.Congregation_Id = c.Congregation_Id
		WHERE 
		 rgf.Frequency_ID = 1 --weekly
		 AND p.Program_ID = 146 --General Giving
	  	 AND rg.end_date IS NULL
		 AND c.Accounting_Company_Id = 1 --crossroads
END
GO

