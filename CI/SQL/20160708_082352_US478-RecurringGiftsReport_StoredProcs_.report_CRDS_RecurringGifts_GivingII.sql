
USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_RecurringGifts_MonthlyII]]    Script Date: 7/08/2016 8:1233 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[report_CRDS_RecurringGifts_MonthlyII]

As Begin


select 'MONTHLY II' as recurrance,count(d.Donor_ID) num_of_gifts, sum(rg.amount) gifts_for_II, sum(rg.amount * 1) total_monthly_II_expected, sum(rg.amount * 12) total_yearly_II_expected
		from dbo.Recurring_Gifts rg
		join dbo.Recurring_Gift_Frequencies rgf on rg.Frequency_ID = rgf.Frequency_ID 
		join dbo.Programs p on rg.Program_ID = p.Program_ID 
		join dbo.Donors d on rg.Donor_ID = d.Donor_ID
		where 	
		  rgf.Frequency_ID = 2 --monthly
		  and p.Program_ID = 146 --General Giving
		  and rg.end_date is null
		union all 
		select 'WEEKLY II' as recurrance,count(d.Donor_ID) num_of_gifts, sum(rg.amount) gifts_for_II,sum(rg.amount *52/12) total_monthly_II_expected, sum(rg.amount * 52) total_yearly_II_expected
		from dbo.Recurring_Gifts rg
		join dbo.Recurring_Gift_Frequencies rgf on rg.Frequency_ID = rgf.Frequency_ID
		join dbo.Programs p on rg.Program_ID = p.Program_ID
		join dbo.Donors d on  rg.Donor_ID = d.Donor_ID
		where 
		 rgf.Frequency_ID = 1 --weekly
		 and p.Program_ID = 146 --General Giving
	  	and rg.end_date is null

End