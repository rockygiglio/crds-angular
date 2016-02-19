USE [MinistryPlatform];

-- Read only access for 'Group Leader w/o Edit - CRDS' (97)
INSERT INTO [dbo].[dp_Role_Sub_Pages] (
     Role_ID
    ,Sub_Page_ID
    ,Access_Level
) VALUES (
     97
    ,557
    ,0
);

-- Full access for 'Staff - CRDS' (100)
INSERT INTO [dbo].[dp_Role_Sub_Pages] (
     Role_ID
    ,Sub_Page_ID
    ,Access_Level
) VALUES (
     100
    ,557
    ,3
);

-- Full access for 'Group Leader with Edit - CRDS' (96)
INSERT INTO [dbo].[dp_Role_Sub_Pages] (
     Role_ID
    ,Sub_Page_ID
    ,Access_Level
) VALUES (
     96
    ,557
    ,3
);

-- Full access for 'Group Manager - CRDS' (94)
INSERT INTO [dbo].[dp_Role_Sub_Pages] (
     Role_ID
    ,Sub_Page_ID
    ,Access_Level
) VALUES (
     94
    ,557
    ,3
);

-- Full access for 'System Administrator - CRDS' (107)
INSERT INTO [dbo].[dp_Role_Sub_Pages] (
     Role_ID
    ,Sub_Page_ID
    ,Access_Level
) VALUES (
     107
    ,557
    ,3
);

-- Full access for 'unauthenticatedCreate' (62)
INSERT INTO [dbo].[dp_Role_Sub_Pages] (
     Role_ID
    ,Sub_Page_ID
    ,Access_Level
) VALUES (
     62
    ,557
    ,3
);