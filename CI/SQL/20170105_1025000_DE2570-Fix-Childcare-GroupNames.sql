USE [MinistryPlatform]
GO

update Groups
set Group_name = '__childcaregroup'
where Group_Name = '__childcare'

