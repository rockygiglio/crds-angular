USE MinistryPlatform
GO

DECLARE @PageID INT = 604
IF NOT EXISTS (SELECT * FROM dp_Pages WHERE Page_ID = @PageID)
BEGIN
	SET IDENTITY_INSERT dp_Pages ON
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
	          Filter_Clause ,
	          Start_Date_Field ,
	          End_Date_Field ,
	          Contact_ID_Field ,
	          Default_View ,
	          Pick_List_View ,
	          Image_Name ,
	          Direct_Delete_Only ,
	          System_Name ,
	          Date_Pivot_Field ,
	          Custom_Form_Name ,
	          Display_Copy
	        )
	VALUES  ( @PageID,
			  N'Medications' , -- Display_Name - nvarchar(50)
	          N'Medication' , -- Singular_Name - nvarchar(50)
	          N'Medications' , -- Description - nvarchar(255)
	          10 , -- View_Order - smallint
	          N'cr_Medications' , -- Table_Name - nvarchar(50)
	          N'Medication_ID' , -- Primary_Key - nvarchar(50)
	          NULL , -- Display_Search - bit
	          N'cr_Medications.Medication_ID,cr_Medications.MedicationName AS Medication_Name,cr_Medications.MedicationType AS Medication_Type' , -- Default_Field_List - nvarchar(2000)
	          N'cr_Medications.Medication_ID' , -- Selected_Record_Expression - nvarchar(255)
	          NULL , -- Filter_Clause - nvarchar(500)
	          NULL , -- Start_Date_Field - nvarchar(50)
	          NULL , -- End_Date_Field - nvarchar(50)
	          NULL , -- Contact_ID_Field - nvarchar(100)
	          NULL , -- Default_View - int
	          NULL , -- Pick_List_View - int
	          NULL , -- Image_Name - nvarchar(50)
	          NULL , -- Direct_Delete_Only - bit
	          NULL , -- System_Name - varchar(16)
	          NULL , -- Date_Pivot_Field - nvarchar(50)
	          NULL , -- Custom_Form_Name - nvarchar(32)
	          1  -- Display_Copy - bit
	        )


	SET IDENTITY_INSERT dp_Pages OFF

	DECLARE @PageSectionID INT = 22
	INSERT INTO [dbo].[dp_Page_Section_Pages](Page_ID,Page_Section_ID) VALUES(@PageID, @PageSectionID)
END
GO
