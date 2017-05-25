USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET Default_Field_List = 'Go_Volunteer_Skills_ID,Label,Attribute_ID_Table.Attribute_Name,Attribute_ID_Table.[Attribute_ID]'
 WHERE Page_ID = 17
GO


