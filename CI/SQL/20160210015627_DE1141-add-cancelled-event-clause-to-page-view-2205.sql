USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 2205;
DECLARE @PageId INT = 308;
DECLARE @PageViewTitle VARCHAR(50) = 'Events Ready for Primary Contact Reminder';
DECLARE @FieldList VARCHAR(1000) = 'Events.[Event_Start_Date]
, Events.[Event_Title] 
, Event_Type_ID_Table.[Event_Type]
, Congregation_ID_Table.[Congregation_Name] 
, Events.[Event_End_Date]
, Events.[Event_ID]
, Primary_Contact_Table.[Contact_ID] as [Primary_Contact_ID] 
, Primary_Contact_Table.[Email_Address] as [Primary_Contact_Email_Address] ';
DECLARE @ViewClause VARCHAR(1000) = 'DATEDIFF(d, GETDATE(), Events.[Event_Start_Date]) = 3 AND
Events.[Event_Title] NOT LIKE ''%serv%'' AND
Events.[Event_Title] NOT LIKE ''%monday%'' AND
Events.[Event_Title] NOT LIKE ''%tuesday%'' AND
Events.[Event_Title] NOT LIKE ''%wednesday%'' AND
Events.[Event_Title] NOT LIKE ''%thursday%'' AND 
Events.[Event_Title] NOT LIKE ''%friday%'' AND
Events.[Event_Title] NOT LIKE ''%saturday%'' AND
Events.[Event_Title] NOT LIKE ''%sunday%'' AND
Event_Type_ID_Table.[Event_Type] NOT LIKE ''No Type'' AND
Events.[Cancelled] = 0 ';
DECLARE @Description VARCHAR(1000) = 'These are events that will generate a reminder tonight. The reminders are generated 2 days prior to the event Start Date and will be sent to the Primary Contact on the event.';

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
