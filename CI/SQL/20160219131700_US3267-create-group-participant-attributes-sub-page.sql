USE [MinistryPlatform];

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON;

INSERT INTO [dbo].[dp_Sub_Pages] (
     [Sub_Page_ID]
    ,[Display_Name]
    ,[Singular_Name]
    ,[Page_ID]
    ,[View_Order]
    ,[Link_To_Page_ID]
    ,[Link_From_Field_Name]
    ,[Select_To_Page_ID]
    ,[Select_From_Field_Name]
    ,[Primary_Table]
    ,[Primary_Key]
    ,[Default_Field_List]
    ,[Selected_Record_Expression]
    ,[Filter_Key]
    ,[Relation_Type_ID]
    ,[On_Quick_Add]
    ,[Contact_ID_Field]
    ,[Default_View]
    ,[System_Name]
    ,[Date_Pivot_Field]
    ,[Start_Date_Field]
    ,[End_Date_Field]
    ,[Custom_Form_Name]
    ,[Display_Copy]
) VALUES (
     557
    ,'Attributes'
    ,'Attribute'
    ,316
    ,9
    ,277
    ,'Attribute_ID'
    ,277
    ,'Group_Participant_Attributes.Attribute_ID'
    ,'Group_Participant_Attributes'
    ,'Group_Participant_Attribute_ID'
    ,'Attribute_ID_Table_Attribute_Type_ID_Table.Attribute_Type,Attribute_ID_Table.Attribute_Name,Group_Participant_Attributes.Start_Date,Group_Participant_Attributes.End_Date,Group_Participant_Attributes.Notes'
    ,'Attribute_ID_Table.Attribute_Name','Group_Participant_ID'
    ,2
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,1
);

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF;