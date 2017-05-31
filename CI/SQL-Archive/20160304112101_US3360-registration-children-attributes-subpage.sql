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
     2
    ,'Children'
    ,'Children'
    ,16
    ,1
    ,null
    ,null
    ,null
    ,null
    ,'cr_Registration_Children_Attributes'
    ,'Registration_Children_Attribute_ID'
    ,'Count'
    ,'Count'
    ,'Registration_ID'
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

--Migrate down
--DELETE FROM [dbo].[dp_Sub_Pages] WHERE [Sub_Page_ID] = 2;