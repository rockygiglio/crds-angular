USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Sub_Pages]
SET  [Display_Name] = 'Attributes'
    ,[Singular_Name] = 'Attribute'
    ,[Page_ID] = 322
    ,[View_Order] = 6
    ,[Link_To_Page_ID] = 277
    ,[Link_From_Field_Name] = 'Attribute_ID'
    ,[Select_To_Page_ID] = 277
    ,[Select_From_Field_Name] = 'Group_Attributes.Attribute_ID'
    ,[Primary_Table] = 'Group_Attributes'
    ,[Primary_Key] = 'Group_Attribute_ID'
    ,[Default_Field_List] = 'Attribute_ID_Table_Attribute_Type_ID_Table.Attribute_Type,Attribute_ID_Table.Attribute_Name,Group_Attributes.Start_Date,Group_Attributes.End_Date, Group_Attributes.[Notes] AS [Notes], Group_Attributes.[Order]'
    ,[Selected_Record_Expression] = 'Attribute_ID_Table.Attribute_Name'
    ,[Filter_Key] = 'Group_ID'
    ,[Relation_Type_ID] = 2
    ,[On_Quick_Add] = NULL
    ,[Contact_ID_Field] = NULL
    ,[Default_View] = NULL
    ,[System_Name] = NULL
    ,[Date_Pivot_Field] = NULL
    ,[Start_Date_Field] = NULL
    ,[End_Date_Field] = NULL
    ,[Custom_Form_Name] = NULL
    ,[Display_Copy] = 1
  WHERE Sub_Page_ID = 303;
GO

