USE [MinistryPlatform]
GO

update dp_page_views
set field_list = 'Contacts.Contact_ID, Email_Address, Donor_Record, Donor_Record_Table.Processor_ID, Contacts.First_Name, Contacts.Last_Name'
where page_view_id = 1098