USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM dp_sub_pages WHERE display_name = 'Category Details')
BEGIN

SET IDENTITY_INSERT [dbo].[dp_sub_pages] ON;

INSERT INTO [dbo].[dp_sub_pages] 
(Sub_Page_ID,Display_Name       ,Singular_Name        ,Page_ID,View_Order,Link_To_Page_ID,Link_From_Field_Name,SELECT_To_Page_ID,SELECT_From_Field_Name                        ,Primary_Table              ,Primary_Key               ,Default_Field_List                                                                                                                                                                                                ,SELECTed_Record_Expression                ,Filter_Key,Relation_Type_ID,On_Quick_Add,Contact_ID_Field,Default_View,System_Name,Date_Pivot_Field,Start_Date_Field,End_Date_Field,Custom_Form_Name,Display_Copy) VALUES
(568        ,'Category Details' ,'Category Detail'    ,322    ,100       ,562            ,'Category_Detail_ID',562              ,'cr_Group_Category_Details.Category_Detail_ID','cr_Group_Category_Details','Group_Category_Detail_ID','Category_Detail_ID_Table.Category_Detail as Category_Detail, Category_Detail_ID_Table_Category_ID_Table.Category as Category,Category_Detail_ID_Table.Description as Description,cr_Group_Category_Details.Notes','Category_Detail_ID_Table.Category_Detail','Group_ID',2               ,NULL        ,NULL            ,NULL        ,NULL       ,NULL            ,NULL            ,NULL          ,NULL            ,true        )

SET IDENTITY_INSERT [dbo].[dp_sub_pages] OFF;

END