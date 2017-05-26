USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

BEGIN

SET IDENTITY_INSERT [dbo].[cr_Kiosk_Types] ON

INSERT INTO [cr_Kiosk_Types]([Kiosk_Type_ID],[Kiosk_Type],[Description],[Domain_ID])
VALUES(1,'KCSignIn','Kiosk Config for Kids Club Sign In',1)
INSERT INTO [cr_Kiosk_Types]([Kiosk_Type_ID],[Kiosk_Type],[Description],[Domain_ID])
VALUES(2,'KCCheckIn','Kiosk Config for Kids Club Check In',1)

SET IDENTITY_INSERT [dbo].[cr_Kiosk_Types] OFF

END
GO