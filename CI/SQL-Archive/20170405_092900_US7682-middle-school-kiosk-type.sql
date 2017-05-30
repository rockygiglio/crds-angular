USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- JPC 4/5/17: Add MSM Kiosk Type for Checkin

BEGIN

SET IDENTITY_INSERT [dbo].[cr_Kiosk_Types] ON

IF NOT EXISTS (SELECT * FROM cr_Kiosk_Types WHERE Kiosk_Type='MSSignIn')
BEGIN
INSERT INTO [cr_Kiosk_Types]([Kiosk_Type_ID],[Kiosk_Type],[Description],[Domain_ID])
VALUES(4,'MSSignIn','Kiosk Config for Middle School Sign In',1)
END

SET IDENTITY_INSERT [dbo].[cr_Kiosk_Types] OFF

END
GO