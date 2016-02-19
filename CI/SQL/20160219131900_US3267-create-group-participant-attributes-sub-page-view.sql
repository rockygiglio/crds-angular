USE [MinistryPlatform];

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON;

INSERT INTO [dbo].[dp_Sub_Page_Views](
     [Sub_Page_View_ID]
    ,[View_Title]
    ,[Sub_Page_ID]
    ,[Description]
    ,[Field_List]
    ,[View_Clause]
    ,[Order_By]
    ,[User_ID]
) VALUES (
     136
    ,'Current Attributes'
    ,557
    ,'Show current group participant attributes'
    ,NULL
    ,'GetDate() BETWEEN Group_Participant_Attributes.Start_Date AND ISNULL(Group_Participant_Attributes.End_Date
    , GetDate())'
    ,NULL
    ,NULL
);

UPDATE [dbo].[dp_Sub_Pages] SET [Default_View] = 136 WHERE [Sub_Page_ID] = 557;

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF;