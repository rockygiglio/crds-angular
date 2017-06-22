USE [MinistryPlatform]
GO

BEGIN TRAN T1

DELETE FROM [MinistryPlatform].[dbo].[dp_Role_Pages]
where Role_ID = 116  --Manage Event Tools - CRDS
GO

DELETE FROM [MinistryPlatform].[dbo].[dp_Role_Sub_Pages]
where Role_ID = 116  --Manage Event Tools - CRDS
GO

UPDATE [dbo].[dp_Role_Pages]
   SET [Access_Level] = 0
 WHERE Page_ID in (302, 308, 384)
	and Role_ID = 100 -- Staff - CRDS

UPDATE [dbo].[dp_Role_Sub_Pages]
   SET [Access_Level] = 0
 WHERE Sub_Page_ID in (284, 285, 286)
	and Role_ID = 100 -- Staff - CRDS
GO

COMMIT TRAN T1