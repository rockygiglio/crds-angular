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
     137
    ,'Current Selected'
    ,303
    ,'Currently selected group participant attributes with AttributeType'
    ,'Group_Attributes.[Group_Attribute_ID]
          , Group_Attributes.[Start_Date]
          , Group_Attributes.[End_Date]
          , Group_Attributes.[Notes]
          , Attribute_ID_Table.[Attribute_ID]
          , Attribute_ID_Table_Attribute_Type_ID_Table.[Attribute_Type_ID]
          , Attribute_ID_Table_Attribute_Type_ID_Table.[Attribute_Type]'
    ,'GetDate() BETWEEN Group_Attributes.Start_Date AND ISNULL(Group_Attributes.End_Date, GetDate())'
    ,NULL
    ,NULL
);

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] OFF;
