USE MinistryPlatform
GO

Alter table [dbo].[cr_Medical_Information]
	add Contact_ID [int]

ALTER TABLE [dbo].[cr_Medical_Information]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Contacts] FOREIGN KEY([Contact_ID])
REFERENCES [dbo].[Contacts] ([Contact_ID])