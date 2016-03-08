USE [MinistryPlatform];

-- Full access for 'unauthenticatedCreate' (62)
INSERT INTO [dbo].[dp_Role_Sub_Pages] (
     Role_ID
    ,Sub_Page_ID
    ,Access_Level
) VALUES (
     62
    ,303
    ,3
);

-- Migrate down
--DELETE FROM [dbo].[dp_Role_Sub_Pages] WHERE Role_ID = 62 AND Sub_Page_ID = 303;