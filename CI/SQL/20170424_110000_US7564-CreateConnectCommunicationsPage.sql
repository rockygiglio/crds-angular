USE MinistryPlatform

DECLARE @PageID INTEGER = 629;
DECLARE @PeopleListPageSectionId INTEGER = 6;

/* Define Page */
IF NOT EXISTS(SELECT * FROM dp_Pages WHERE Page_ID = @PageID)
BEGIN
	SET IDENTITY_INSERT dbo.dp_Pages ON; 
	INSERT INTO dp_Pages( Page_ID
	                    , Display_Name
						, Singular_Name
						, Description
						, View_Order
						, Table_Name
						, Primary_Key,Default_Field_List
						, Selected_Record_Expression
						, Display_Copy)
				VALUES (@PageID
				        , 'Connect Connections'
						, 'Connect Connection'
						, 'Connect Connections'
						, 300
						, 'cr_Connect_Communications'
						, 'Connect_Communications_ID'
						, 'FromUser_Contact_ID_Table.[Display_Name] AS From_User_Name,FromUser_Contact_ID_Table.[Email_Address] AS From_User_Email, ToUser_Contact_ID_Table.[Display_Name] AS To_User_Name, ToUser_Contact_ID_Table.[Email_Address] AS To_User_Email, Communication_ID_Table.[Start_Date] AS Communication_Date, Communication_Type, Communication_Status'
						, 'Connect_Communications_ID'
						, 1)
	SET IDENTITY_INSERT dbo.dp_Pages OFF; 
END

/* Assign Page to Section */
IF NOT EXISTS(SELECT * FROM dp_Page_Section_Pages WHERE Page_ID = @PageID AND  Page_Section_ID = @PeopleListPageSectionId)
BEGIN
	INSERT INTO dp_Page_Section_Pages(Page_ID, Page_Section_ID) VALUES(@PageID,@PeopleListPageSectionId);
END

/*system admin crds */
IF NOT EXISTS(SELECT * FROM dp_Role_Pages where role_id = 107 and page_id = 629)
BEGIN
	insert into dp_Role_Pages(role_id, page_id,access_level,scope_all,Approver,File_Attacher,Data_Importer,Data_Exporter,Secure_Records,Allow_Comments,Quick_Add)
             values(107,	629,	3,	0,	0,	1,	0,	1,	1,	0,	1);
END

/*sapi user */
IF NOT EXISTS(SELECT * FROM dp_Role_Pages where role_id = 62 and page_id = 629)
BEGIN
	insert into dp_Role_Pages(role_id, page_id,access_level,scope_all,Approver,File_Attacher,Data_Importer,Data_Exporter,Secure_Records,Allow_Comments,Quick_Add)
             values(107,	629,	3,	0,	0,	0,	0,	0,	0,	0,	0);
END
GO

