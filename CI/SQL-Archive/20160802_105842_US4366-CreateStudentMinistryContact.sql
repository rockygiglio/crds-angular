USE [MinistryPlatform]
GO

--This was created directly in prod. For Demo mostly. 
IF NOT EXISTS(SELECT * FROM Contacts WHERE Email_Address = 'studentministry@crossroads.net')
BEGIN

SET IDENTITY_INSERT [dbo].[Contacts] ON;

INSERT INTO [dbo].Contacts 
(Contact_ID,Company,Display_Name            ,First_Name        ,Last_Name,Contact_Status_ID,Email_Address                   ,Bulk_Email_Opt_Out,Bulk_SMS_Opt_Out,Contact_GUID,Domain_ID) VALUES
(7676101   ,0      ,'Team, Student Ministry','Student Ministry','Team'   ,1                ,'studentministry@crossroads.net',1                 ,1               ,NEWID()     ,1        );

SET IDENTITY_INSERT [dbo].[Contacts] OFF;
END