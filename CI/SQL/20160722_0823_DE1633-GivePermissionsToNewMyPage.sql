USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Role_Pages] WHERE Role_ID = 39 AND Page_ID = 563)
BEGIN
INSERT INTO [DBO].[Role_Pages]
(Role_ID,
Page_ID,
Access_Level,
Scope_All,
Approver,
File_Attacher,
Data_Importer,
Secure_Records,
Allow_Comments,
Quick_Add) VALUES
(39,
563,
1,
0,
0,
0,
0,
0,
0,
0,
0)
END