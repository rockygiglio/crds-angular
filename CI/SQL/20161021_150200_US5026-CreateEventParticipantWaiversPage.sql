USE MinistryPlatform
GO

DECLARE @PageID INT = 611

IF NOT EXISTS(SELECT * FROM dbo.dp_Pages WHERE Page_ID = @PageID)
BEGIN
	SET IDENTITY_INSERT dbo.dp_Pages ON
	INSERT INTO dbo.dp_Pages
			( Page_ID,
			  Display_Name ,
			  Singular_Name ,
			  Description ,
			  View_Order ,
			  Table_Name ,
			  Primary_Key ,
			  Display_Search ,
			  Default_Field_List ,
			  Selected_Record_Expression ,
			  Display_Copy
			)
	VALUES  ( @PageID,
			  N'Event Participant Waivers' , -- Display_Name - nvarchar(50)
			  N'Event Participant Waiver' , -- Singular_Name - nvarchar(50)
			  N'Event Participant Waivers' , -- Description - nvarchar(255)
			  110 , -- View_Order - smallint
			  N'cr_Event_Participant_Waivers' , -- Table_Name - nvarchar(50)
			  N'Event_Participant_Waiver_ID' , -- Primary_Key - nvarchar(50)
			  1 , -- Display_Search - bit
			  N'Event_Participant_ID_Table_Participant_ID_Table_Contact_ID_Table.Display_Name  ,Waiver_ID_Table.Waiver_Name' , -- Default_Field_List - nvarchar(2000)
			  N'Waiver_ID_Table.Waiver_Name' , -- Selected_Record_Expression - nvarchar(255)
			  1  -- Display_Copy - bit
			)
	SET IDENTITY_INSERT dbo.dp_Pages ON
END

GO
