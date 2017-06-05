use MinistryPlatform;
GO

IF NOT EXISTS ( SELECT * FROM sys.columns WHERE Name = N'Program_ID' AND Object_ID = Object_ID(N'dbo.Products'))
BEGIN
	ALTER TABLE [dbo].[Products] ADD [Program_ID] int null;
END

IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_Products_Programs]') AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[Products] WITH CHECK ADD CONSTRAINT [FK_Products_Programs] FOREIGN KEY([Program_ID]) REFERENCES [dbo].[Programs] ([Program_ID])
END