USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM dp_Pages WHERE display_name = 'Categories')
BEGIN
SET IDENTITY_INSERT [dbo].[dp_pages] ON;

INSERT INTO [dbo].[dp_pages] 
(Page_ID,Display_Name ,Singular_Name,Description                    ,View_Order,Table_Name    ,Primary_Key   ,Display_Search,Default_Field_List                                 ,SELECTed_Record_Expression,Filter_Clause,Start_Date_Field,End_Date_Field,Contact_ID_Field,Default_View,Pick_List_View,Image_Name,Direct_Delete_Only,System_Name,Date_Pivot_Field,Custom_Form_Name,Display_Copy) VALUES
(561    ,'Categories' ,'Category'   ,'Categories defined for groups',75       ,'cr_Categories','Category_ID' ,1             ,'cr_Categories.Category, cr_Categories.Description','Category'                ,NULL         ,NULL            ,NULL          ,NULL            ,NULL        ,NULL          ,NULL      ,1                 ,NULL       ,NULL            ,NULL            ,1           )
SET IDENTITY_INSERT [dbo].[dp_pages] OFF;
END

IF NOT EXISTS(SELECT * FROM dp_Page_Section_Pages WHERE page_id = 561 AND page_section_id = 4)
BEGIN
INSERT INTO [dbo].[dp_Page_Section_Pages]
(Page_ID, Page_Section_ID) VALUES
(561    , 4)
END
