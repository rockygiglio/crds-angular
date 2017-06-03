use MinistryPlatform
GO

Update dbo.dp_Page_Views 
	SET View_Clause = N'Events.Event_End_Date >= Getdate() AND
         Congregation_ID_Table.[Congregation_Name] = ''Oakley'' AND ISNULL(Events.[Cancelled], 0) = 0'
	WHERE Page_View_ID = 2209;

Update dbo.dp_Page_Views 
	SET View_Clause = N'Events.Event_End_Date >= Getdate() AND
         Congregation_ID_Table.[Congregation_Name] = ''West Side'' AND ISNULL(Events.[Cancelled], 0) = 0'
	WHERE Page_View_ID = 2211;

Update dbo.dp_Page_Views 
	SET View_Clause = N'Events.Event_End_Date >= Getdate() AND
         Congregation_ID_Table.[Congregation_Name] = ''Uptown'' AND ISNULL(Events.[Cancelled], 0) = 0'
	WHERE Page_View_ID = 2210;

Update dbo.dp_Page_Views 
	SET View_Clause = N'Events.Event_End_Date >= Getdate() AND
         Congregation_ID_Table.[Congregation_Name] = ''Mason'' AND ISNULL(Events.[Cancelled], 0) = 0'
	WHERE Page_View_ID = 2208;

Update dbo.dp_Page_Views 
	SET View_Clause = N'Events.Event_End_Date >= Getdate() AND
         Congregation_ID_Table.[Congregation_Name] = ''Florence'' AND ISNULL(Events.[Cancelled], 0) = 0'
	WHERE Page_View_ID = 2207;
