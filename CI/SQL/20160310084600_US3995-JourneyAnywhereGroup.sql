USE [MinistryPlatform]	
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON

INSERT INTO [dp_Page_Views]([Page_View_ID],[View_Title],[Page_ID],[Description],[Field_List],[View_Clause],[Order_By],[User_ID],[User_Group_ID])
VALUES(2215,'Current Journey Anywhere Group',316,'BRAVE Journey participants that are non-local and were not able to join a group ','Participant_ID_Table_Contact_ID_Table.[First_Name]
, Participant_ID_Table_Contact_ID_Table.[Last_Name]
, Participant_ID_Table_Contact_ID_Table.[Email_Address]
, Participant_ID_Table_Contact_ID_Table.[Mobile_Phone] AS [Phone]
, Participant_ID_Table_Contact_ID_Table_Household_ID_Table_Congregation_ID_Table.Congregation_Name
,(SELECT Attribute_Name 
 FROM Group_Participant_Attributes GPA, Attributes A 
 where GPA.Attribute_ID = A.Attribute_ID and 
GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID and
Attribute_Type_ID = 76) AS ''Gender''
,(SELECT Attribute_Name 
 FROM Group_Participant_Attributes GPA, Attributes A 
 where GPA.Attribute_ID = A.Attribute_ID and 
GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID and
Attribute_Type_ID = 77) AS ''Marital Status''
,(SELECT Attribute_Name 
 FROM Group_Participant_Attributes GPA, Attributes A 
 where GPA.Attribute_ID = A.Attribute_ID and 
GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID and
Attribute_Type_ID = 70) AS ''Group Participant Past Experience''
,(SELECT Attribute_Name 
 FROM Group_Participant_Attributes GPA, Attributes A 
 where GPA.Attribute_ID = A.Attribute_ID and 
GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID and
Attribute_Type_ID = 72) AS ''Group Participant Goal''
,STUFF((SELECT '', '' + Attribute_Name 
     FROM Group_Participant_Attributes GPA
      INNER JOIN Attributes A ON A.Attribute_ID = GPA.Attribute_ID	
     WHERE GPA.Attribute_ID = A.Attribute_ID
      AND A.Attribute_Type_ID = 78
	  AND GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID
	  AND GETDATE() BETWEEN GPA.Start_Date 
	  AND ISNULL(GPA.End_Date,GETDATE())   
     FOR XML PATH('''')), 1, 1, '''') AS [Weekday Times]
,STUFF((SELECT '', '' + Attribute_Name 
     FROM Group_Participant_Attributes GPA
      INNER JOIN Attributes A ON A.Attribute_ID = GPA.Attribute_ID	
     WHERE GPA.Attribute_ID = A.Attribute_ID
      AND A.Attribute_Type_ID = 79
	  AND GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID
	  AND GETDATE() BETWEEN GPA.Start_Date 
	  AND ISNULL(GPA.End_Date,GETDATE())   
     FOR XML PATH('''')), 1, 1, '''') AS [Weekend Times]

 ','Group_ID_Table.Group_ID = 166574 AND  (Group_Participants.End_Date > GetDate() OR Group_Participants.End_Date IS NULL)
',NULL,NULL,NULL)

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF