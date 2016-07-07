USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM dp_Pages WHERE display_name = 'Invitatation Types')
BEGIN

SET IDENTITY_INSERT [dbo].[dp_pages] ON;

INSERT INTO [dbo].[dp_pages] 
(Page_ID,Display_Name       ,Singular_Name    ,Description                                ,View_Order,Table_Name           ,Primary_Key          ,Display_Search,Default_Field_List                                                                                ,SELECTed_Record_Expression,Filter_Clause,Start_Date_Field,End_Date_Field,Contact_ID_Field,Default_View,Pick_List_View,Image_Name,Direct_Delete_Only,System_Name,Date_Pivot_Field,Custom_Form_Name,Display_Copy) VALUES
(562    ,'Category Details' ,'Category Detail','Category Details associated to a Category',76        ,'cr_Category_Details','Category_Detail_ID' ,1             ,'cr_Category_Details.Category_Detail, cr_Category_Details.Description, Category_ID_Table.Category','Category_Detail'         ,NULL         ,NULL            ,NULL          ,NULL            ,NULL        ,NULL          ,NULL      ,1                 ,NULL       ,NULL            ,NULL            ,1           )

SET IDENTITY_INSERT [dbo].[dp_pages] OFF;

END

IF NOT EXISTS(SELECT * FROM dp_Page_Section_Pages WHERE page_id = 562 AND page_section_id = 4)
BEGIN
INSERT INTO [dbo].[dp_Page_Section_Pages]
(Page_ID, Page_Section_ID) VALUES
(562    , 4)
END
