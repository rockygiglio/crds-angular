USE MinistryPlatform
GO

DECLARE @SubPageID INT = 602

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
	          N'Events' , -- Display_Name - nvarchar(50)
	          N'Event' , -- Singular_Name - nvarchar(50)
	          605 , -- Page_ID - int
	          1 , -- View_Order - smallint
	          610 , -- Link_To_Page_ID - int
	          N'Event_Waiver_ID' , -- Link_From_Field_Name - nvarchar(50)
	          308 , -- Select_To_Page_ID - int
	          N'Event_Waivers.Event_ID' , -- Select_From_Field_Name - nvarchar(50)
	          N'cr_Event_Waivers' , -- Primary_Table - nvarchar(50)
	          N'Event_Waiver_ID' , -- Primary_Key - nvarchar(50)
	          N'Event_ID_Table.Event_Title,  Event_ID_Table.Event_Start_Date, Event_ID_Table.Event_End_Date' , -- Default_Field_List - nvarchar(2000)
	          N'Event_ID_Table.Event_Title' , -- Selected_Record_Expression - nvarchar(500)
	          N'Waiver_ID' , -- Filter_Key - nvarchar(50)
	          2 , -- Relation_Type_ID - int
	          1  -- Display_Copy - bit
	        )
	SET IDENTITY_INSERT dbo.dp_Sub_Pages OFF
END

GO
