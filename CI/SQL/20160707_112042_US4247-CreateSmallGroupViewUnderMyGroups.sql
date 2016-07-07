USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].[dp_Page_Views] WHERE Page_View_ID = 2308)
BEGIN

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;

INSERT INTO [dbo].[dp_Page_Views]
(Page_View_ID
,View_Title               
,Page_ID
,Description
,Field_List
,View_Clause) 
VALUES
(2308        
,'My Current Small Groups'
,461    
,'Group Tool Small Group View'         
,'Group_Type_ID_Table.[Group_Type_ID] AS [Group_Type_ID]
, Groups.[Group_ID] AS [Group_ID]
, Groups.[Group_Name] AS [Group_Name]
, Meeting_Day_ID_Table.[Meeting_Day] AS [Meeting_Day]
, Groups.[Meeting_Time] AS [Meeting_Time]
, Meeting_Frequency_ID_Table.[Meeting_Frequency] AS [Meeting_Frequency]
, Offsite_Meeting_Address_Table.[Address_Line_1] AS [Address_Line_1]
, Offsite_Meeting_Address_Table.[Address_Line_2] AS [Address_Line_2]
, Offsite_Meeting_Address_Table.[City] AS [City]
, Offsite_Meeting_Address_Table.[State/Region] AS [State_Region]
, Offsite_Meeting_Address_Table.[Postal_Code] AS [Postal_Code]'
, 'Group_Type_ID_Table.[Group_Type_ID] = 1'
)

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON;

END