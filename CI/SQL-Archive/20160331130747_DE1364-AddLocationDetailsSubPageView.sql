USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
INSERT INTO [dbo].[dp_Sub_Page_Views]
           ([Sub_Page_View_ID]
		   ,[View_Title]
           ,[Sub_Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause])
     VALUES
           (180
		   ,'Locations With Details'
           ,5
           ,''
           ,'Location_ID_Table.[Location_ID] AS [Location ID]
, Location_ID_Table.[Location_Name] AS [Location Name]
, Location_ID_Table_Location_Type_ID_Table.[Location_Type_ID] AS [Location Type ID]
, Organization_ID_Table.[Organization_ID] AS [Organization ID]
, Organization_ID_Table.[Name] AS [Name]
, Location_ID_Table_Address_ID_Table.[Address_Line_1] AS [Address Line 1]
, Location_ID_Table_Address_ID_Table.[Address_Line_2] AS [Address Line 2]
, Location_ID_Table_Address_ID_Table.[City] AS [City]
, Location_ID_Table_Address_ID_Table.[State/Region] AS [State/Region]
, Location_ID_Table_Address_ID_Table.[Postal_Code] AS [Postal Code]
, Location_ID_Table.[Image_URL] AS [Image URL]'
           ,'1=1')

SET IDENTITY_INSERT [dbo].[dp_Sub_Page_Views] ON
GO