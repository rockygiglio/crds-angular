USE MinistryPlatform
GO

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = N'Payments' AND COLUMN_NAME = 'Payment_Status_ID')
BEGIN
	ALTER TABLE dbo.Payments ADD Payment_Status_ID INT NOT NULL DEFAULT 1;

	ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Payment_Status] FOREIGN KEY([Payment_Status_ID])
	REFERENCES [dbo].[Donation_Statuses] ([Donation_Status_ID]);

	ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Payment_Status]
END
GO