USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM dp_Pages WHERE display_name = 'Private Invitations')
BEGIN
SET IDENTITY_INSERT [dbo].[dp_pages] ON;

INSERT INTO [dbo].[dp_pages] 
(Page_ID,Display_Name         ,Singular_Name       ,Description              ,View_Order,Table_Name      ,Primary_Key    ,Display_Search,Default_Field_List                                                                                                                                                                                                                                           ,Selected_Record_Expression,Direct_Delete_Only,Display_Copy) 
VALUES
(560    ,'Private Invitations','Private Invitation','List of private invites',2         ,'cr_Invitations','Invitation_ID',1             ,'cr_Invitations.Recipient_Name, cr_Invitations.Email_address, cr_Invitations_Invitation_Date, cr_Invitations.Invitation_Type_ID, cr_Invitations.Source_ID, cr_invitations.Invitation_GUID, cr_invitations.Invitation_Used, Group_Role_ID_Table.Group_Role_ID','Invitation_ID'           ,1                 ,1           );

SET IDENTITY_INSERT [dbo].[dp_pages] OFF;
END

GO

IF NOT EXISTS(SELECT * FROM dp_Page_Section_Pages WHERE page_id = 560 AND page_section_id = 1)
BEGIN
INSERT INTO [dbo].[dp_Page_Section_Pages]
(Page_ID, Page_Section_ID) 
VALUES
(560    , 1)
END
GO