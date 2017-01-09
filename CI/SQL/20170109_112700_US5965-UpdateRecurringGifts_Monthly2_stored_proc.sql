USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_RecurringGifts_MonthlyII]    Script Date: 1/9/2017 11:26:37 AM ******/
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

As Begin


select 'MONTHLY II' as recurrance,count(d.Donor_ID) num_of_gifts, sum(rg.amount) gifts_for_II, sum(rg.amount * 1) total_monthly_II_expected, sum(rg.amount * 12) total_yearly_II_expected
		from dbo.Recurring_Gifts rg
		inner join dbo.Recurring_Gift_Frequencies rgf on rg.Frequency_ID = rgf.Frequency_ID 
		inner join dbo.Programs p on rg.Program_ID = p.Program_ID 
		inner join dbo.Donors d on rg.Donor_ID = d.Donor_ID
		INNER JOIN dbo.Congregations c ON p.Congregation_Id = c.Congregation_Id
		where 	
		  rgf.Frequency_ID = 2 --monthly
		  and p.Program_ID = 146 --General Giving
		  and rg.end_date is null
		  AND c.Accounting_Company_Id = 1 --crossroads
		union all 
		select 'WEEKLY II' as recurrance,count(d.Donor_ID) num_of_gifts, sum(rg.amount) gifts_for_II,sum(rg.amount *52/12) total_monthly_II_expected, sum(rg.amount * 52) total_yearly_II_expected
		from dbo.Recurring_Gifts rg
		inner join dbo.Recurring_Gift_Frequencies rgf on rg.Frequency_ID = rgf.Frequency_ID
		inner join dbo.Programs p on rg.Program_ID = p.Program_ID
		inner join dbo.Donors d on  rg.Donor_ID = d.Donor_ID
		INNER JOIN dbo.Congregations c ON p.Congregation_Id = c.Congregation_Id
		where 
		 rgf.Frequency_ID = 1 --weekly
		 and p.Program_ID = 146 --General Giving
	  	 and rg.end_date is null
		 AND c.Accounting_Company_Id = 1 --crossroads
End
GO


