USE [MinistryPlatform]

SET IDENTITY_INSERT Contacts ON 

INSERT INTO dbo.Contacts (Contact_ID, Company, Company_Name, Display_Name, First_Name, Nickname, Contact_Status_ID, Household_Position_ID, Email_Address, Bulk_Email_Opt_Out, Bulk_SMS_Opt_Out, Contact_GUID, Domain_ID)
VALUES (53, 1, 'safetyclientuser','safetyclientuser','S', 'S',1, 6,'safety@thinkministry.org',0,0,'bc9aa7c1-9847-4eb5-8d0a-8b32a2be4568', 1)


SET IDENTITY_INSERT Contacts OFF

GO

SET IDENTITY_INSERT dp_Users ON

INSERT INTO dbo.dp_Users  ([User_ID], [User_Name], [User_Email],Display_Name, [Password], Domain_ID, Contact_ID)
VALUES (1002,'Cro55ro@d$_Safety_Client', 'safety@thinkministry.org', 'safetyclientuser', 0x406C6C40, 1, 53);

SET IDENTITY_INSERT dp_Users OFF

GO
