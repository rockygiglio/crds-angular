USE MinistryPlatform
GO

DECLARE @PageID INT = 611
DECLARE @SubPageID INT = 601

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
	          On_Quick_Add ,
	          Display_Copy
	        )
	VALUES  ( @SubPageID,
	          N'Waivers' , -- Display_Name - nvarchar(50)
	          N'Waiver' , -- Singular_Name - nvarchar(50)
	          305 , -- Page_ID - int
	          110 , -- View_Order - smallint
	          @PageID , -- Link_To_Page_ID - int
	          N'Event_Participant_Waiver_ID' , -- Link_From_Field_Name - nvarchar(50)
	          605 , -- Select_To_Page_ID - int
	          N'Event_Participant_Waivers.Waiver_Id' , -- Select_From_Field_Name - nvarchar(50)
	          N'cr_Event_Participant_Waivers' , -- Primary_Table - nvarchar(50)
	          N'Event_Participant_Waiver_ID' , -- Primary_Key - nvarchar(50)
	          N'Waiver_ID_Table.Waiver_Name' , -- Default_Field_List - nvarchar(2000)
	          N'Waiver_ID_Table.Waiver_Name' , -- Selected_Record_Expression - nvarchar(500)
	          N'Event_Participant_ID' , -- Filter_Key - nvarchar(50)
	          2 , -- Relation_Type_ID - int
	          0 , -- On_Quick_Add - bit
	          1  -- Display_Copy - bit
	        )
	SET IDENTITY_INSERT dbo.dp_Sub_Pages OFF
END

GO
