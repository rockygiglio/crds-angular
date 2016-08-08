USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

BEGIN
UPDATE [dbo].[dp_Role_Pages]
SET [Access_Level] = 0
WHERE [Role_ID] = 62 and [Page_ID] = 515;

END
