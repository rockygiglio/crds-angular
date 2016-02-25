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
     138
    ,'Current Selected'
    ,557
    ,'Currently selected group participant attributes with AttributeType'
    ,'Group_Participant_Attributes.[Group_Participant_Attribute_ID] AS [Group Participant Attribute ID]
          , Group_Participant_Attributes.[Start_Date] AS [Start Date]
          , Group_Participant_Attributes.[End_Date] AS [End Date]
          , Group_Participant_Attributes.[Notes] AS [Notes]
          , Attribute_ID_Table.[Attribute_ID] AS [Attribute ID]
          , Attribute_ID_Table_Attribute_Type_ID_Table.[Attribute_Type_ID] AS [Attribute Type ID]
          , Attribute_ID_Table_Attribute_Type_ID_Table.[Attribute_Type] AS [Attribute Type]'
    ,'GetDate() BETWEEN Group_Participant_Attributes.Start_Date AND ISNULL(Group_Participant_Attributes.End_Date, GetDate())'
    ,NULL
    ,NULL
);

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF;
