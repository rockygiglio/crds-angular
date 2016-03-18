USE [MinistryPlatform];
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;
GO

DECLARE @PageViewId INT = 2219;
DECLARE @PageId INT = 13;
DECLARE @PageViewTitle VARCHAR(50) = 'GroupConnectorsByOrganization';
DECLARE @FieldList VARCHAR(1000) = 'Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Primary_Registration]
, Project_ID_Table.[Project_Name]
, Primary_Registration_Table_Preferred_Launch_Site_ID_Table.[Location_Name] AS [Preferred_Launch_Site]
, Primary_Registration_Table_Organization_ID_Table.[Organization_ID]
, Primary_Registration_Table_Initiative_ID_Table.[Initiative_ID]
, cr_GroupConnectors.[GroupConnector_ID]';
DECLARE @ViewClause VARCHAR(1000) = 'cr_GroupConnectors.[GroupConnector_ID] IS NOT NULL';
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