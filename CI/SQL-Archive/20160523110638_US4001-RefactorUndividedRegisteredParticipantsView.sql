USE [MinistryPlatform]	
GO

UPDATE [dp_page_views]
SET [Field_List] = 	'Group_Participants.[Start_Date] AS [Registration_Date] 
	,Participant_ID_Table_Contact_ID_Table.[First_Name]
	,Participant_ID_Table_Contact_ID_Table.[Last_Name]
	,Participant_ID_Table_Contact_ID_Table_Gender_ID_Table.[Gender]
	,STUFF((SELECT '', '' + Attribute_Name 
      FROM Contact_Attributes CA
      INNER JOIN Attributes A ON A.Attribute_ID = CA.Attribute_ID	
	  INNER JOIN Contacts C ON C.Contact_ID = CA.Contact_ID	
      WHERE CA.Contact_ID = Participant_ID_Table_Contact_ID_Table.[Contact_ID]
	  AND A.Attribute_Type_ID = 20
	  AND GETDATE() BETWEEN CA.Start_Date 
      AND ISNULL(CA.End_Date,GETDATE()) 
	  FOR XML PATH('''')), 1, 1, '''') AS [Ethnicity]
	,DATEDIFF(hour,Participant_ID_Table_Contact_ID_Table.[Date_of_Birth],GETDATE())/8766 AS [AGE]
	,Group_ID_Table.Group_Name AS [Preferred Undivided Session]		
	,(SELECT Notes 
	  FROM Group_Participant_Attributes GPA, Attributes A 
      WHERE GPA.Attribute_ID = A.Attribute_ID
	  AND GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID
	  AND GETDATE() BETWEEN GPA.Start_Date AND ISNULL(GPA.End_Date,GETDATE()) 
	  AND Attribute_Type_ID = 88) AS [Preferred_Co-Participant]	
	,Group_Participants.[Child_Care_Requested] AS [Requested_Child_Care]'
,[View_Clause] ='Group_ID_Table_Group_Type_ID_Table.Group_Type_ID = 26 AND Group_Role_ID_Table.Group_Role_ID = 16 AND
     (Group_Participants.End_Date > GetDate() OR Group_Participants.End_Date IS NULL) 
		 AND (Group_ID_Table.End_Date > GetDate() OR Group_ID_Table.End_Date IS NULL) AND Group_ID_Table_Parent_Group_Table.Group_ID IS NULL'
		
WHERE Page_View_ID = '92145'			
