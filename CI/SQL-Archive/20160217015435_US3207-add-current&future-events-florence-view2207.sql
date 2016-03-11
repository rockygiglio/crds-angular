USE [MinistryPlatform]
GO

SET IDENTITY_INSERT dbo.dp_Page_Views ON 

INSERT INTO dbo.dp_Page_Views 
    (
        Page_View_ID, 
        View_Title, 
        Page_ID, 
        [Description], 
        Field_List, 
        View_Clause, 
        Order_By, 
        [User_ID], 
        User_Group_ID
    )
    VALUES
    (
        2207, 
        'Current & Future Events - Florence', 
        308, 
        'Current and future events at Florence', 
		NULL,
		'Events.Event_End_Date >= Getdate() AND
		 Congregation_ID_Table.[Congregation_Name] = ''Florence''', 
        'Events.Event_Start_Date', 
        NULL, 
        NULL
    )

	SET IDENTITY_INSERT dbo.dp_Page_Views OFF