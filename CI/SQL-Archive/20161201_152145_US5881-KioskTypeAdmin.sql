USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

BEGIN

SET IDENTITY_INSERT [dbo].[cr_Kiosk_Types] ON

INSERT INTO [cr_Kiosk_Types]([Kiosk_Type_ID],[Kiosk_Type],[Description],[Domain_ID])
VALUES(3,'KCAdmin','Kiosk Config for Kids Club Admin',1)

SET IDENTITY_INSERT [dbo].[cr_Kiosk_Types] OFF

END
GO
