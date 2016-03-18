USE [MinistryPlatform]	
GO

UPDATE [dbo].[dp_Page_Views]
SET [Field_List] = 'Groups.[Group_Name] AS [Group_Name]
,Congregation_ID_Table.Congregation_Name
, Groups.[Start_Date]
, Primary_Contact_Table.[Display_Name] AS [Primary_Contact]
, Groups.[Remaining_Capacity]
,(SELECT COUNT(*) FROM Group_Participants GP WHERE GP.Group_ID = Groups.Group_ID) AS No_Of_Participants
, Meeting_Day_ID_Table.[Meeting_Day]
, Groups.[Meeting_Time]
, Offsite_Meeting_Address_Table.[Address_Line_1]
, Offsite_Meeting_Address_Table.[Address_Line_2]
, Offsite_Meeting_Address_Table.[City]
, Offsite_Meeting_Address_Table.[State/Region] AS [State]
, Offsite_Meeting_Address_Table.[Postal_Code]
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 77 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) AS Marital_Status
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 71 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) AS Group_Goal
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 76 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) AS Gender
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 73 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) AS Group_Type
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 75 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as Kids
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Group_Attributes.Attribute_ID in (7012) and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as Has_Cat
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Group_Attributes.Attribute_ID in (7011) and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as Has_Dog
, Groups.[Description] AS [Group_Message]
,CASE  
 WHEN ( Meeting_Day_ID_Table.Meeting_Day_ID IS NULL AND Groups.[Meeting_Time] IS NULL AND  Offsite_Meeting_Address_Table.[Address_ID] IS NULL)  
 THEN  ''Private''
 WHEN ( Meeting_Day_ID_Table.Meeting_Day_ID IS NOT NULL AND Groups.[Meeting_Time] IS NOT NULL AND  Offsite_Meeting_Address_Table.[Address_ID] IS NOT NULL)  
 THEN  ''Public''
 END AS [Private_Public]'
WHERE Page_View_ID = 2218

GO

UPDATE [dbo].[dp_Page_Views]
SET [Field_List] = 'Groups.[Group_Name] AS [Group_Name]
,Congregation_ID_Table.Congregation_Name
, Groups.[Start_Date]
, Primary_Contact_Table.[Display_Name] AS [Primary_Contact]
, Groups.[Remaining_Capacity]
,(SELECT COUNT(*) FROM Group_Participants GP WHERE GP.Group_ID = Groups.Group_ID) AS No_Of_Participants
, Meeting_Day_ID_Table.[Meeting_Day]
, Groups.[Meeting_Time]
, Offsite_Meeting_Address_Table.[Address_Line_1]
, Offsite_Meeting_Address_Table.[Address_Line_2]
, Offsite_Meeting_Address_Table.[City]
, Offsite_Meeting_Address_Table.[State/Region] AS [State]
, Offsite_Meeting_Address_Table.[Postal_Code]
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 77 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) AS Marital_Status
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 71 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) AS Group_Goal
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 76 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) AS Gender
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 73 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) AS Group_Type
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 75 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as Kids
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Group_Attributes.Attribute_ID in (7012) and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as Has_Cat
, (SELECT TOP 1 Attributes.Attribute_Name FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Group_Attributes.Attribute_ID in (7011) and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as Has_Dog
, Groups.[Description] AS [Group_Message]
,CASE  
 WHEN ( Meeting_Day_ID_Table.Meeting_Day_ID IS NULL AND Groups.[Meeting_Time] IS NULL AND  Offsite_Meeting_Address_Table.[Address_ID] IS NULL)  
 THEN  ''Private''
 WHEN ( Meeting_Day_ID_Table.Meeting_Day_ID IS NOT NULL AND Groups.[Meeting_Time] IS NOT NULL AND  Offsite_Meeting_Address_Table.[Address_ID] IS NOT NULL)  
 THEN  ''Public''
END AS [Private_Public]'
WHERE Page_View_ID = 2216

GO
	


