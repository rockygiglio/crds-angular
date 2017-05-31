USE MinistryPlatform
GO

DECLARE @PageID INT = 384
DECLARE @SubPageID INT = 605

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
	          N'Bumping Rules' , -- Display_Name - nvarchar(50)
	          N'Bumping Rule' , -- Singular_Name - nvarchar(50)
	          384 , -- Page_ID - int
	          100 , -- View_Order - smallint
	          @PageID , -- Link_To_Page_ID - int
	          N'Bumping_Rule_ID' , -- Link_From_Field_Name - nvarchar(50)
	          384 , -- Select_To_Page_ID - int
	          N'Bumping_Rules.Bumping_Rule_ID' , -- Select_From_Field_Name - nvarchar(50)
	          N'cr_Bumping_Rules' , -- Primary_Table - nvarchar(50)
	          N'Bumping_Rule_ID' , -- Primary_Key - nvarchar(50)
	          N'From_Room_ID_Table.Room_Name' , -- Default_Field_List - nvarchar(2000)
	          N'Bumping_Rule_ID_Table.Bumping_Rule_ID, Bumping_Rule_ID_Table.From_Event_Room_ID, Bumping_Rule_ID_Table.To_Event_Room_ID' , -- Selected_Record_Expression - nvarchar(500)
	          N'From_Event_Room_ID' , -- Filter_Key - nvarchar(50)
	          2 , -- Relation_Type_ID - int
	          0 , -- On_Quick_Add - bit
	          1  -- Display_Copy - bit
	        )
	SET IDENTITY_INSERT dbo.dp_Sub_Pages OFF
END

GO
