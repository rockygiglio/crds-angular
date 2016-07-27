USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

BEGIN
INSERT [dbo].[dp_Role_Pages]
    ([Role_ID],
     [Page_ID],
     [Access_Level],
     [Scope_All],
     [Approver],
     [File_Attacher],
     [Data_Importer],
     [Data_Exporter],
     [Secure_Records],
     [Allow_Comments],
     [Quick_Add])
VALUES
    (62,
     515,
     0,
     0,
     0,
     0,
     0,
     0,
     0,
     0,
     0);
END