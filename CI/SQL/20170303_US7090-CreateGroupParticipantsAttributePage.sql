USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = 626)
BEGIN
SET IDENTITY_INSERT [dbo].[dp_Pages] ON
INSERT INTO [dbo].dp_Pages 
(
	Page_ID,
	Display_Name,
	Singular_Name,
	Description,
	View_Order,
	Table_Name,
	Primary_Key,
	Display_Search,
	Default_Field_List,
	Selected_Record_Expression,
	Display_Copy
) 
VALUES
	(
	626,
	'Group Participant Attributes',
	'Group Participant Attribute',
	'Characteristics of a group participant record',
	-100,
	'Group_Participant_Attributes',
	'Group_Participant_Attribute_ID',
	1,
	'Group_Participant_Attribute_ID, Group_Participant_ID, Attribute_ID, Group_Participant_ID, Start_Date, End_Date',
	'Group_Participant_Attribute_ID',
	0       )
END