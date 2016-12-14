USE [MinistryPlatform]
GO

IF NOT EXISTS ( SELECT *
				FROM sys.columns 
				WHERE Name = N'Source_Url'
				AND Object_ID = Object_ID(N'Donations') )
	BEGIN
		ALTER TABLE [MinistryPlatform].[dbo].[Donations]
		ADD Source_Url nvarchar(512)
	END
GO

IF NOT EXISTS ( SELECT *
				FROM sys.columns 
				WHERE Name = N'Predefined_Amount'
				AND Object_ID = Object_ID(N'Donations') )
	BEGIN
		ALTER TABLE [MinistryPlatform].[dbo].[Donations]
		ADD Predefined_Amount decimal(6,2)
	END
GO