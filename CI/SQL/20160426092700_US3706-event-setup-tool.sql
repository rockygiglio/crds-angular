USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 2222;
DECLARE @PageId INT = 308;
DECLARE @PageViewTitle VARCHAR(50) = 'EventsBySite';
DECLARE @FieldList VARCHAR(1000) = 'Events.[Event_ID] AS [Event ID], 
	Events.[Event_Title] AS [Event Title], 
	Congregation_ID_Table.[Congregation_ID] AS [Congregation ID], 
	Congregation_ID_Table.[Congregation_Name] AS [Congregation Name], 
	Events.[Template] AS [Template]';
DECLARE @ViewClause VARCHAR(1000) = '1=1';
DECLARE @Description VARCHAR(1000) = 'API View';

DELETE FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = @PageViewId;

INSERT INTO [dbo].[dp_Page_Views]
       ( [Page_View_ID],
         [View_Title],
         [Page_ID],
         [Field_List],
         [View_Clause],
         [Description]
       )
VALUES( @PageViewId, @PageViewTitle, @PageId, @FieldList, @ViewClause, @Description );
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF;
GO