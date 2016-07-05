Use [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_pages] ON;

IF NOT EXISTS(Select * from dp_Pages where display_name = 'Invitatation Types')
BEGIN
INSERT INTO [dbo].[dp_pages] 
(Page_ID,Display_Name      ,Singular_Name     ,Description           ,View_Order,Table_Name           ,Primary_Key         ,Display_Search,Default_Field_List                                                    ,Selected_Record_Expression,Filter_Clause,Start_Date_Field,End_Date_Field,Contact_ID_Field,Default_View,Pick_List_View,Image_Name,Direct_Delete_Only,System_Name,Date_Pivot_Field,Custom_Form_Name,Display_Copy) VALUES
(559    ,'Invitation Types','Invitation Type' ,'Types of invitations',195       ,'cr_Invitation_Types','Invitation_Type_ID',1             ,'cr_Invitation_Types.Invitation_Type, cr_Invitation_Types.Description','Invitation_type'         ,null         ,null            ,null          ,null            ,null        ,null          ,null      ,1                 ,null       ,null            ,null            ,1           )
END
GO

SET IDENTITY_INSERT [dbo].[dp_pages] OFF;

IF NOT EXISTS(select * from dp_Page_Section_Pages where page_id = 559 and page_section_id = 4)
BEGIN
INSERT INTO [dbo].[dp_Page_Section_Pages]
(Page_ID, Page_Section_ID) VALUES
(559    , 4)
END
GO