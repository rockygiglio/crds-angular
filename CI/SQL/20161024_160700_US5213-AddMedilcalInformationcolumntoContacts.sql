USE [MinistryPlatform]
GO


IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'MedicalInformation_ID'  AND Object_ID = Object_ID(N'Contacts'))
BEGIN
	ALTER TABLE dbo.Contacts ADD MedicalInformation_ID INT NULL; 

	ALTER TABLE [dbo].[Contacts]  WITH CHECK ADD  CONSTRAINT [FK_Contacts_cr_Medical_Information] FOREIGN KEY([MedicalInformation_ID])
	REFERENCES [dbo].[cr_Medical_Information] ([MedicalInformation_ID])


	ALTER TABLE [dbo].[Contacts] CHECK CONSTRAINT [FK_Contacts_cr_Medical_Information]

END
GO

