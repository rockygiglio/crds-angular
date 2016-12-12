use MinistryPlatform;
GO

DECLARE @PROGRAM_ID int;

SELECT @PROGRAM_ID = Program_ID FROM [dbo].[Programs] WHERE [Program_Name] = 'Summer Camps';


IF NOT EXISTS ( SELECT * FROM sys.columns WHERE Name = N'Program_ID' AND Object_ID = Object_ID(N'dbo.Products'))
BEGIN
	ALTER TABLE [dbo].[Products] ADD [Program_ID] int null;

	-- initialize a current products to summer camp program so we can make it a not null field
	--UPDATE [dbo].[Products] SET Program_ID = @PROGRAM_ID;

	--ALTER TABLE [dbo].[Products] ALTER COLUMN [Program_ID] INTEGER NOT NULL
END

IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_Products_Programs]') AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[Products] WITH CHECK ADD CONSTRAINT [FK_Products_Programs] FOREIGN KEY([Program_ID]) REFERENCES [dbo].[Programs] ([Program_ID])
END