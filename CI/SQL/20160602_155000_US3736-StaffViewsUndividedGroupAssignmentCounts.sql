USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Page_Views] ON
GO

INSERT INTO [dbo].[dp_Page_Views]		
      ([Page_View_ID]
      ,[View_Title]
      ,[Page_ID]
      ,[Description]
      ,[Field_List]
      ,[View_Clause])
    VALUES
      (2301,
      'Undivided - Groups',
      '322',   
      'Current Undivided groups with the number of assigned facilitators and participants per ethnicity for all sites',
      'Groups.Group_Name
,(SELECT SUM(CASE ca.Ethnicity_Attributes WHEN ca.Caucasian_Attributes THEN 1 ELSE 0 END) AS Caucasians
    FROM Group_Participants gp 					
		INNER JOIN Participants p ON p.Participant_Id = gp.Participant_Id
		INNER JOIN Contacts c ON c.Contact_ID = p.Contact_ID
		LEFT OUTER JOIN (SELECT COUNT(*) AS Ethnicity_Attributes, 
				   COUNT(CASE WHEN a.Attribute_ID = 6836 THEN 1 ELSE NULL END) as Caucasian_Attributes, ca.Contact_ID
				FROM Contact_Attributes ca 
				LEFT OUTER JOIN Attributes a ON a.Attribute_id = ca.Attribute_id 
				WHERE a.Attribute_Type_Id = 20 AND (ca.End_Date > GETDATE() OR ca.End_Date IS NULL)
				GROUP BY ca.Contact_ID) ca ON ca.Contact_Id = p.Contact_Id			
   WHERE gp.Group_Role_Id = 22 AND gp.Group_Id = Groups.Group_id 
   GROUP BY gp.Group_Id) AS No_of_Caucasian_Facilitators
,(SELECT SUM(CASE ca.Ethnicity_Attributes WHEN ca.Caucasian_Attributes THEN 0 ELSE 1 END) AS Non_Caucasians
	FROM Group_Participants gp 					
		INNER JOIN Participants p ON p.Participant_Id = gp.Participant_Id
		INNER JOIN Contacts c ON c.Contact_ID = p.Contact_ID
		LEFT OUTER JOIN (SELECT COUNT(*) AS Ethnicity_Attributes, 
				   COUNT(CASE WHEN a.Attribute_ID = 6836 THEN 1 ELSE NULL END) as Caucasian_Attributes, ca.Contact_ID
				FROM Contact_Attributes ca 
				LEFT OUTER JOIN Attributes a ON a.Attribute_Id = ca.Attribute_Id 
				WHERE a.Attribute_Type_Id = 20 AND (ca.End_Date > GETDATE() OR ca.End_Date IS NULL)
			    GROUP BY ca.Contact_ID) ca ON ca.Contact_Id = p.Contact_Id			
	WHERE gp.Group_Role_Id = 22 AND gp.Group_Id = Groups.Group_Id 
	GROUP BY gp.Group_Id) AS No_of_People_of_Color_Facilitators
,(SELECT SUM(CASE ca.Ethnicity_Attributes WHEN ca.Caucasian_Attributes THEN 1 ELSE 0 END) AS Caucasians
	FROM Group_Participants gp 					
		INNER JOIN Participants p ON p.Participant_Id = gp.Participant_Id
		INNER JOIN Contacts c ON c.Contact_ID = p.Contact_ID
		LEFT OUTER JOIN (SELECT COUNT(*) AS Ethnicity_Attributes, 
				   COUNT(CASE WHEN a.attribute_ID = 6836 THEN 1 ELSE NULL END) as Caucasian_Attributes, ca.Contact_ID
				FROM Contact_Attributes ca 
				LEFT OUTER JOIN Attributes a ON a.Attribute_Id = ca.Attribute_Id 
				WHERE a.Attribute_Type_Id = 20 AND (ca.End_Date > GETDATE() OR ca.End_Date IS NULL)
			    GROUP BY ca.Contact_ID) ca ON ca.Contact_Id = p.Contact_Id			
	WHERE gp.Group_Role_Id = 16 AND gp.Group_Id = Groups.Group_id
	GROUP BY gp.Group_Id) AS No_of_Caucasian_Participants
,(SELECT SUM(CASE ca.Ethnicity_Attributes WHEN ca.Caucasian_Attributes THEN 0 ELSE 1 END) AS Non_Caucasians
	FROM Group_Participants gp 					
		INNER JOIN Participants p ON p.Participant_Id = gp.Participant_Id
		INNER JOIN Contacts c ON c.Contact_ID = p.Contact_ID
		LEFT OUTER JOIN (SELECT COUNT(*) AS Ethnicity_Attributes, 
					COUNT(CASE WHEN a.Attribute_ID = 6836 THEN 1 ELSE NULL END) as Caucasian_Attributes, ca.Contact_ID
				FROM Contact_Attributes ca 
				LEFT OUTER JOIN Attributes a ON a.Attribute_Id = ca.Attribute_Id 
				WHERE a.Attribute_Type_Id = 20 AND (ca.End_Date > GETDATE() OR ca.End_Date IS NULL)
			GROUP BY ca.Contact_ID) ca ON ca.Contact_Id = p.Contact_Id			
	WHERE gp.Group_Role_Id = 16 AND gp.Group_Id = Groups.Group_Id
	GROUP BY gp.Group_Id) AS No_of_People_of_Color_Participants',
      'Group_Type_ID_Table.[Group_Type_ID] = 26 AND (Groups.[End_Date] > getDate() OR Groups.[End_Date] IS NULL)' )

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
END