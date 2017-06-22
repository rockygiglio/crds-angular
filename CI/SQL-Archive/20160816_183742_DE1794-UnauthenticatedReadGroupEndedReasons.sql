USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM [dbo].[dp_Role_Pages] where Role_ID = 62 AND Page_ID = 313)
BEGIN
INSERT INTO [dbo].[dp_Role_Pages]
(Role_ID,Page_ID,Access_Level,Scope_All,Approver,File_Attacher,Data_Importer,Data_Exporter,Secure_Records,Allow_Comments,Quick_Add) VALUES
(62     ,313    ,0           ,0        ,0       ,0            ,0            ,0            ,0             ,0             ,0        );
END