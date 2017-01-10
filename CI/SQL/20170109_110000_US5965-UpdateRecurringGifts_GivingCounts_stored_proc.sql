USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_RecurringGifts_GivingCounts]    Script Date: 1/9/2017 11:16:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_RecurringGifts_GivingCounts]') AND TYPE IN (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_RecurringGifts_GivingCounts] AS' 
END
GO

ALTER PROCEDURE [dbo].[report_CRDS_RecurringGifts_GivingCounts]

AS BEGIN

SELECT 'Unique Givers' AS TYPE,COUNT(DISTINCT d.Donor_ID) num_of_gifts
		FROM dbo.Recurring_Gifts rg
		INNER JOIN dbo.Recurring_Gift_Frequencies rgf ON rg.Frequency_ID = rgf.Frequency_ID
		INNER JOIN dbo.Programs p ON rg.Program_ID = p.Program_ID
		INNER JOIN dbo.Donors d ON rg.Donor_ID = d.Donor_ID
		INNER JOIN dbo.Congregations c ON p.Congregation_Id = c.Congregation_Id
		WHERE 
		  rg.end_date IS NULL
		  AND c.Accounting_Company_Id = 1 --crossroads
		UNION ALL
        --gives total OVERALL givers 
		SELECT 'Total Givers Setup' AS TYPE,COUNT(d.Donor_ID) num_of_gifts 
		FROM dbo.Recurring_Gifts rg
		INNER JOIN dbo.Recurring_Gift_Frequencies rgf ON rg.Frequency_ID = rgf.Frequency_ID
		INNER JOIN dbo.Programs p ON rg.Program_ID = p.Program_ID
		INNER JOIN dbo.Donors d ON rg.Donor_ID = d.Donor_ID
		INNER JOIN dbo.Congregations c ON p.Congregation_Id = c.Congregation_Id
		WHERE 
		 rg.end_date IS NULL
		 AND c.Accounting_Company_Id = 1 --crossroads
END

GO
