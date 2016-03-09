USE MinistryPlatform;
GO

SET IDENTITY_INSERT [dbo].[dp_Page_VIews] ON

INSERT INTO [dp_Page_Views]([Page_View_ID],[View_Title],[Page_ID],[Description],[Field_List],[View_Clause],[Order_By],[User_ID],[User_Group_ID])
VALUES(2216,'Current Journey Groups',322,'Staff members can view the BRAVE journey groups that have been created.','Groups.[Group_Name] AS [Group_Name]
, Groups.[Start_Date]
, Primary_Contact_Table.[Display_Name] AS [Primary_Contact]
, Groups.[Remaining_Capacity]
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
END AS [Private_Public]','Group_Type_ID_Table.[Group_Type_ID] = 19
 AND (Groups.[End_Date] > getDate() OR Groups.[End_Date] IS NULL)',NULL,NULL,NULL)

 SET IDENTITY_INSERT [dbo].[dp_Page_VIews] OFF