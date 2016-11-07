USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

BEGIN

SET IDENTITY_INSERT [dbo].[cr_Bumping_Rule_Types] ON

INSERT INTO [cr_Bumping_Rule_Types]([Bumping_Rule_Type_ID],[Bumping_Rule_Type],[Description],[Domain_ID])
VALUES(1,'Priority','Priority Rule Type',1)
INSERT INTO [cr_Bumping_Rule_Types]([Bumping_Rule_Type_ID],[Bumping_Rule_Type],[Description],[Domain_ID])
VALUES(2,'Vacancy','Vacancy Rule Type',1)

SET IDENTITY_INSERT [dbo].[cr_Bumping_Rule_Types] OFF

END
GO