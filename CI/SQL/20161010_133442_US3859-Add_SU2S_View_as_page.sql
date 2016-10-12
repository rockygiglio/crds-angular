USE MinistryPlatform
GO

DECLARE @PageID INT = 609;
IF NOT EXISTS (SELECT * FROM dp_Pages WHERE Page_ID = @PageID)
BEGIN
	SET IDENTITY_INSERT dp_Pages ON
	INSERT INTO [dbo].[dp_pages] 
	  ( Page_ID,
		Display_Name,
		Singular_Name,
		Description,
		View_Order,
		Table_Name,
		Default_Field_List,
		Selected_Record_Expression,
		Display_Copy
	  ) 
	VALUES 
	  ( @PageID,
		'Serving Participants',
		'Serving Participant',
		'SU2S View',
		-10,
		'vw_crds_Serving_Participants',
		'vw_crds_Serving_Participants.Group_ID,
		vw_crds_Serving_Participants.Participant_ID,
		vw_crds_Serving_Participants.Email_Address,
		vw_crds_Serving_Participants.Display_Name,
		vw_crds_Serving_Participants.Nickname,
		vw_crds_Serving_Participants.Last_Name,
		vw_crds_Serving_Participants.Domain_ID,
		vw_crds_Serving_Participants.Group_Name,
		vw_crds_Serving_Participants.Group_Type_ID,
		vw_crds_Serving_Participants.Primary_Contact_Email,
		vw_crds_Serving_Participants.Opportunity_ID,
		vw_crds_Serving_Participants.Opportunity_Title,
		vw_crds_Serving_Participants.Minimum_Needed,
		vw_crds_Serving_Participants.Maximum_Needed,
		vw_crds_Serving_Participants.Shift_Start,
		vw_crds_Serving_Participants.Shift_End,
		vw_crds_Serving_Participants.Room,
		vw_crds_Serving_Participants.Sign_Up_Deadline,
		vw_crds_Serving_Participants.Deadline_Passed_Message_ID,
		vw_crds_Serving_Participants.Role_Title,
		vw_crds_Serving_Participants.Event_ID,
		vw_crds_Serving_Participants.Event_Title,
		vw_crds_Serving_Participants.Event_Start_Date,
		vw_crds_Serving_Participants.RSVP,
		vw_crds_Serving_Participants.Contact_ID,
		vw_crds_Serving_Participants.Group_Role_ID,
		vw_crds_Serving_Participants.Event_Type_ID,
		vw_crds_Serving_Participants.Event_Type,
		vw_crds_Serving_Participants.Participant_Start_Date,
		vw_crds_Serving_Participants.Participant_End_Date',
		'vw_crds_Serving_Participants.[Event_ID]',
		0
		);
		
		SET IDENTITY_INSERT dp_Pages OFF;
END
GO

DECLARE @Role INT = 62;
DECLARE @PageID INT = 609;
IF NOT EXISTS (SELECT Role_Page_ID FROM [dbo].[dp_Role_Pages] WHERE Page_ID = @PageId AND ROLE_ID = @Role)
BEGIN

	INSERT INTO [dbo].[Dp_Role_Pages] 
	(RoleID,Page_ID,Access_Level,Scope_All,Approver,File_Attacher,Data_Importer,Data_Exporter,Secure_Records,Allow_Comments,Quick_Add) VALUES
	(@Role ,@PageId,3           ,0        ,0       ,0            ,0            ,0            ,0             ,0             ,0        );
END
GO