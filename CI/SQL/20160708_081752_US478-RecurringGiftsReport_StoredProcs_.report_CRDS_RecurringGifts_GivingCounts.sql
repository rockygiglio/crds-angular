
USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_RecurringGifts_GivingCounts]    Script Date: 7/08/2016 8:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[report_CRDS_RecurringGifts_GivingCounts]

As Begin

select 'Unique Givers' as type,count(distinct d.Donor_ID) num_of_gifts
		from dbo.Recurring_Gifts rg
		join dbo.Recurring_Gift_Frequencies rgf on rg.Frequency_ID = rgf.Frequency_ID
		join dbo.Programs p on rg.Program_ID = p.Program_ID
		join dbo.Donors d on rg.Donor_ID = d.Donor_ID
		where 
		  rg.end_date is null
		union all
--gives total OVERALL givers 
		select 'Total Givers Setup' as type,count(d.Donor_ID) num_of_gifts 
		from dbo.Recurring_Gifts rg
		join dbo.Recurring_Gift_Frequencies rgf on rg.Frequency_ID = rgf.Frequency_ID
		join dbo.Programs p on rg.Program_ID = p.Program_ID
		join dbo.Donors d on rg.Donor_ID = d.Donor_ID
		where 
		 rg.end_date is null

End

Go
