USE MinistryPlatform;
GO

UPDATE [dbo].[dp_Sub_Pages] 
SET Default_Field_List = N'Project_Type_ID_Table.Description as [Project_Type], Priority'
WHERE Sub_Page_ID = 248
