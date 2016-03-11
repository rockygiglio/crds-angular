USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 2212;
DECLARE @PageId INT = 400;
DECLARE @PageViewTitle VARCHAR(50) = 'Tasks Needing Auto Started';
DECLARE @FieldList VARCHAR(1000) = 'dp_Tasks.[Task_ID] AS [Task ID]
, dp_Tasks.[Title] AS [Title]
, Author_User_ID_Table.[User_ID] AS [Author User ID]
, Assigned_User_ID_Table.[User_ID] AS [Assigned User ID]
, dp_Tasks.[Start_Date] AS [Start Date]
, dp_Tasks.[End_Date] AS [End Date]
, dp_Tasks.[Completed] AS [Completed]
, dp_Tasks.[Description] AS [Description]
, dp_Tasks.[_Record_ID] AS [Record ID]
, _Page_ID_Table.[Page_ID] AS [Page ID]
, _Process_Submission_ID_Table.[Process_Submission_ID] AS [Process Submission ID]
, dp_Tasks.[_Process_Step_ID] AS [Process Step ID]
, dp_Tasks.[_Rejected] AS [Rejected]
, dp_Tasks.[_Escalated] AS [Escalated]
, dp_Tasks.[_Record_Description] AS [Record Description]';
DECLARE @ViewClause VARCHAR(1000) = 'dp_Tasks.[Title] LIKE ''SUBMIT:%''
 AND dp_Tasks.[Completed] = 0
 AND _Page_ID_Table.[Page_ID] IN (302,384)
';
DECLARE @Description VARCHAR(1000) = 'Used by .NET scheduled task TaskService.AutoCompleteTasks function';

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
