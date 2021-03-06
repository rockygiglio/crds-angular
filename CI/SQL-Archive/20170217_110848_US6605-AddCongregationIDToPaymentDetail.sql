USE MinistryPlatform
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Congregation_ID' AND object_id = Object_ID(N'Payment_Detail'))
BEGIN
	ALTER TABLE [dbo].[Payment_Detail]
	ADD Congregation_ID int NULL
END

IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_Product_Detail_Congregation]') AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[Payment_Detail] WITH CHECK ADD CONSTRAINT [FK_Product_Detail_Congregation] FOREIGN KEY([Congregation_ID]) REFERENCES [dbo].[Congregations] ([Congregation_ID])
END