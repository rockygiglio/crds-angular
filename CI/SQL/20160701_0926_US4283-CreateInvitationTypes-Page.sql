Use [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM dp_Pages WHERE display_name = 'Invitatation Types')
BEGIN

SET IDENTITY_INSERT [dbo].[dp_pages] ON;

INSERT INTO [dbo].[dp_pages] 
(Page_ID,Display_Name      ,Singular_Name     ,Description           ,View_Order,Table_Name           ,Primary_Key         ,Display_Search,Default_Field_List                                                    ,Selected_Record_Expression,Direct_Delete_Only,Display_Copy) VALUES
(559    ,'Invitation Types','Invitation Type' ,'Types of invitations',195       ,'cr_Invitation_Types','Invitation_Type_ID',1             ,'cr_Invitation_Types.Invitation_Type, cr_Invitation_Types.Description','Invitation_type'         ,1                 ,1           );

SET IDENTITY_INSERT [dbo].[dp_pages] OFF;

END

IF NOT EXISTS(SELECT * FROM dp_Page_Section_Pages WHERE page_id = 559 AND page_section_id = 4)
BEGIN
INSERT INTO [dbo].[dp_Page_Section_Pages]
(Page_ID, Page_Section_ID) VALUES
(559    , 4)
END
GO