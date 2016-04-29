USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 2221;
DECLARE @PageId INT = 408;
DECLARE @PageViewTitle VARCHAR(50) = 'Groups By Event Id';
DECLARE @FieldList VARCHAR(1000) = 'Event_Groups.[Event_Group_ID]
, Event_ID_Table.[Event_ID]
, Group_ID_Table.[Group_ID]
, Room_ID_Table.[Room_ID]
, Event_Groups.[Closed]
, Event_Room_ID_Table.[Event_Room_ID]';
DECLARE @ViewClause VARCHAR(1000) = 'Event_ID_Table.[Event_ID] IS NOT NULL';
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