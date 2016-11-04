USE MinistryPlatform
GO

DECLARE @SubPageID INT = 603

IF NOT EXISTS (SELECT * FROM dbo.dp_Sub_Pages WHERE Sub_Page_ID = @SubPageID)
BEGIN
	SET IDENTITY_INSERT dbo.dp_Sub_Pages ON
	INSERT INTO dbo.dp_Sub_Pages
	        ( Sub_Page_ID,
			  Display_Name ,
	          Singular_Name ,
	          Page_ID ,
	          View_Order ,
	          Link_To_Page_ID ,
	          Link_From_Field_Name ,
	          Select_To_Page_ID ,
	          Select_From_Field_Name ,
	          Primary_Table ,
	          Primary_Key ,
	          Default_Field_List ,
	          Selected_Record_Expression ,
	          Filter_Key ,
	          Relation_Type_ID ,
	          Display_Copy
	        )
	VALUES  ( @SubPageID,
	          N'Event Participants' , -- Display_Name - nvarchar(50)
	          N'Event Participant' , -- Singular_Name - nvarchar(50)
	          605 , -- Page_ID - int
	          2 , -- View_Order - smallint
	          611 , -- Link_To_Page_ID - int
	          N'Event_Participant_Waiver_ID' , -- Link_From_Field_Name - nvarchar(50)
	          305 , -- Select_To_Page_ID - int
	          N'Event_Participant_Waiver.Event_Participant_ID' , -- Select_From_Field_Name - nvarchar(50)
	          N'cr_Event_Participant_Waivers' , -- Primary_Table - nvarchar(50)
	          N'Event_Participant_Waiver_ID' , -- Primary_Key - nvarchar(50)
	          N'Event_Participant_ID_Table_Participant_ID_Table_Contact_ID_Table.Display_Name' , -- Default_Field_List - nvarchar(2000)
	          N'Event_Participant_ID_Table_Participant_ID_Table_Contact_ID_Table.Display_Name' , -- Selected_Record_Expression - nvarchar(500)
	          N'Waiver_ID' , -- Filter_Key - nvarchar(50)
	          2 , -- Relation_Type_ID - int
	          1  -- Display_Copy - bit
	        )
	SET IDENTITY_INSERT dbo.dp_Sub_Pages OFF
END

GO
