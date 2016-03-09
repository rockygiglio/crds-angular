USE MinistryPlatform;
GO

SET IDENTITY_INSERT [dbo].[dp_Page_VIews] ON

INSERT INTO [dp_Page_Views]([Page_View_ID],[View_Title],[Page_ID],[Description],[Field_List],[View_Clause],[Order_By],[User_ID],[User_Group_ID])
VALUES(2214,'Journey Search Results',322,NULL,'Groups.[Group_ID]
, Groups.[Group_Name]
, Groups.[Description]
, Groups.[Start_Date]
, Groups.[End_Date]
, Groups.[Meeting_Time]
, Meeting_Day_ID_Table.[Meeting_Day_ID]
, Meeting_Day_ID_Table.[Meeting_Day]
, Offsite_Meeting_Address_Table.[Address_ID]
, Offsite_Meeting_Address_Table.[Address_Line_1]
, Offsite_Meeting_Address_Table.[Address_Line_2]
, Offsite_Meeting_Address_Table.[City] AS [City]
, Offsite_Meeting_Address_Table.[State/Region]
, Offsite_Meeting_Address_Table.[Postal_Code]
, Groups.[Remaining_Capacity]
, Primary_Contact_Table.[Nickname]
, Primary_Contact_Table.[Last_Name]
, (SELECT TOP 1 Attributes.Attribute_ID FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 73 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as ''Group_Type''
, (SELECT TOP 1 Attributes.Attribute_ID FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 71 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as ''Group_Goal''
, (SELECT TOP 1 Attributes.Attribute_ID FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Attributes.Attribute_Type_ID = 75 and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as ''Kids''
, (SELECT TOP 1 Attributes.Attribute_ID FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Group_Attributes.Attribute_ID in (7012) and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as ''Has_Cat''
, (SELECT TOP 1 Attributes.Attribute_ID FROM Group_Attributes, Attributes where Group_Attributes.Attribute_ID=Attributes.Attribute_ID and Group_Attributes.Attribute_ID in (7011) and Group_ID = Groups.[Group_ID] and ( Group_Attributes.End_Date > GetDate() OR Group_Attributes.End_Date IS NULL)) as ''Has_Dog''
,CASE  
 WHEN ((Meeting_Day_ID_Table.Meeting_Day_ID = 1 OR Meeting_Day_ID_Table.Meeting_Day_ID = 7)AND Groups.[Meeting_Time] BETWEEN ''00:00:00'' AND ''08:59:59'' ) 
 THEN  ''7029''
 WHEN ((Meeting_Day_ID_Table.Meeting_Day_ID = 1 OR Meeting_Day_ID_Table.Meeting_Day_ID = 7)AND Groups.[Meeting_Time] BETWEEN ''09:00:00'' AND ''12:00:00'' ) 
 THEN  ''7030''
 WHEN ((Meeting_Day_ID_Table.Meeting_Day_ID = 1 OR Meeting_Day_ID_Table.Meeting_Day_ID = 7)AND Groups.[Meeting_Time] BETWEEN ''12:00:00'' AND ''17:00:00'' ) 
 THEN  ''7031''
 WHEN ((Meeting_Day_ID_Table.Meeting_Day_ID = 1 OR Meeting_Day_ID_Table.Meeting_Day_ID = 7)AND Groups.[Meeting_Time] BETWEEN ''17:00:00'' AND ''20:00:00'') 
 THEN  ''7032''
 WHEN ((Meeting_Day_ID_Table.Meeting_Day_ID = 1 OR Meeting_Day_ID_Table.Meeting_Day_ID = 7)AND Groups.[Meeting_Time] BETWEEN ''20:00:00'' AND ''23:59:59'') 
 THEN  ''7033'' 
 WHEN (Meeting_Day_ID_Table.Meeting_Day_ID BETWEEN 2 AND 6 AND Groups.[Meeting_Time] BETWEEN ''00:00:00'' AND ''08:59:59'')  
 THEN ''7023''  
 WHEN (Meeting_Day_ID_Table.Meeting_Day_ID BETWEEN 2 AND 6 AND Groups.[Meeting_Time] BETWEEN ''09:00:00'' AND ''12:00:00'')  
 THEN ''7024''  
 WHEN (Meeting_Day_ID_Table.Meeting_Day_ID BETWEEN 2 AND 6 AND Groups.[Meeting_Time] BETWEEN ''12:00:00'' AND ''17:00:00'')  
 THEN ''7025''  
 WHEN (Meeting_Day_ID_Table.Meeting_Day_ID BETWEEN 2 AND 6 AND Groups.[Meeting_Time] BETWEEN ''17:00:00'' AND ''20:00:00'')  
 THEN ''7026'' 
 WHEN (Meeting_Day_ID_Table.Meeting_Day_ID BETWEEN 2 AND 6 AND Groups.[Meeting_Time] BETWEEN ''20:00:00'' AND ''23:59:59'')  
 THEN ''7027''  
 END AS [Meeting_Range]','Group_Type_ID_Table.[Group_Type_ID] = 19
 AND (Groups.[End_Date] > getDate() OR Groups.[End_Date] IS NULL)
 AND Groups.[Remaining_Capacity] > 0
',NULL,NULL,NULL)

SET IDENTITY_INSERT [dbo].[dp_Page_VIews] OFF