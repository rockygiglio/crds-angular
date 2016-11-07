USE MinistryPlatform
GO

DECLARE @SubPageViewID INT = 300

IF NOT EXISTS (SELECT * FROM dbo.dp_Sub_Page_Views WHERE Sub_Page_View_ID = @SubPageViewID)
BEGIN
	SET IDENTITY_INSERT dbo.dp_Sub_Page_Views ON
	INSERT INTO [dp_Sub_Page_Views]
	([Sub_Page_View_ID],
	[View_Title],
	[Sub_Page_ID],
	[Description],
	[Field_List],
	[View_Clause],
	[Order_By],
	[User_ID])
	VALUES(@SubPageViewID,
	'Default Bumping Rule View',
	605,
	'Default Bumping Rule View',
	'cr_Bumping_Rules.[Bumping_Rules_ID] AS [Bumping Rules ID]
	, From_Event_Room_ID_Table_Room_ID_Table.[Room_Name] AS [From Room Name]
	, To_Event_Room_ID_Table_Room_ID_Table.[Room_Name] AS [To Room Name]
	, cr_Bumping_Rules.[Priority_Order] AS [Priority Order]
	, Bumping_Rule_Type_ID_Table.[Bumping_Rule_Type] AS [Bumping Rule Type]
	',
	'1=1',
	NULL,
	4412415)

	SET IDENTITY_INSERT dbo.dp_Sub_Page_Views OFF
END
GO