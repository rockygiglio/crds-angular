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
,(SELECT  COUNT(CASE WHEN a.non_caucasian <> 0 THEN 1 END) as non_caucasian FROM (SELECT gp.Group_Participant_Id, gp.Group_Id, gp.Participant_ID, COUNT(ca.Attribute_id) AS non_caucasian     
                    FROM Group_Participants gp 
                        INNER JOIN Participants p ON p.Participant_id = gp.Participant_Id
                        LEFT OUTER JOIN (Contact_Attributes ca  INNER JOIN Attributes a ON a.Attribute_Id = ca.Attribute_Id AND a.Attribute_Type_Id = 20)
                            ON ca.Contact_Id = p.Contact_Id AND ca.Attribute_Id NOT IN (6836,6843)  AND (ca.End_Date > GETDATE() OR ca.End_Date IS NULL)                        
                    WHERE gp.Group_Role_Id = 22 AND (gp.End_Date > getdate() OR gp.End_Date IS NULL) AND gp.Group_Id = Groups.Group_Id                  
                    GROUP BY gp.Group_Participant_Id ,gp.Group_Id, gp.Participant_ID
        ) a  GROUP BY Group_Id) AS No_of_People_of_Color_Facilitators
,(SELECT  COUNT(*) - COUNT(CASE WHEN a.non_caucasian <> 0 THEN 1 END) AS caucasian FROM (SELECT gp.Group_Participant_Id, gp.Group_Id, gp.Participant_ID, COUNT(ca.Attribute_id) AS non_caucasian              
                    FROM Group_Participants gp 
                        INNER JOIN Participants p ON p.Participant_id = gp.Participant_Id
                        LEFT OUTER JOIN (Contact_Attributes ca
                            INNER JOIN Attributes a ON a.Attribute_Id = ca.Attribute_Id AND a.Attribute_Type_Id = 20)
                            ON ca.Contact_Id = p.Contact_Id AND ca.Attribute_Id NOT IN (6836,6843)  AND (ca.End_Date > GETDATE() OR ca.End_Date IS NULL)                        
                    WHERE gp.Group_Role_Id = 22 AND (gp.End_Date > getdate() OR gp.End_Date IS NULL) AND gp.Group_Id = Groups.Group_Id                  
                    GROUP BY gp.Group_Participant_Id ,gp.Group_Id, gp.Participant_ID
        ) a  GROUP BY Group_Id) AS No_of_Caucasian_Facilitators
,(SELECT  COUNT(CASE WHEN a.non_caucasian <> 0 THEN 1 END) as non_caucasian FROM (SELECT gp.Group_Participant_Id, gp.Group_Id, gp.Participant_ID, COUNT(ca.Attribute_id) AS non_caucasian              
                    FROM Group_Participants gp   INNER JOIN Participants p ON p.Participant_id = gp.Participant_Id
                        LEFT OUTER JOIN (Contact_Attributes ca   INNER JOIN Attributes a ON a.Attribute_Id = ca.Attribute_Id AND a.Attribute_Type_Id = 20)
                            ON ca.Contact_Id = p.Contact_Id AND ca.Attribute_Id NOT IN (6836,6843) AND (ca.End_Date > GETDATE() OR ca.End_Date IS NULL)                        
                    WHERE gp.Group_Role_Id = 16 AND (gp.End_Date > getdate() OR gp.End_Date IS NULL) AND gp.Group_Id = Groups.Group_Id                  
                    GROUP BY gp.Group_Participant_Id ,gp.Group_Id, gp.Participant_ID
         ) a  GROUP BY Group_Id) AS No_of_People_of_Color_Particpants
,(SELECT  COUNT(*) - COUNT(CASE WHEN a.non_caucasian <> 0 THEN 1 END) AS caucasian  FROM (SELECT gp.Group_Participant_Id, gp.Group_Id, gp.Participant_ID, COUNT(ca.Attribute_id) AS non_caucasian              
                    FROM Group_Participants gp 
                        INNER JOIN Participants p ON p.Participant_id = gp.Participant_Id
                        LEFT OUTER JOIN (Contact_Attributes ca
                            INNER JOIN Attributes a ON a.Attribute_Id = ca.Attribute_Id AND a.Attribute_Type_Id = 20)
                            ON ca.Contact_Id = p.Contact_Id AND ca.Attribute_Id NOT IN (6836,6843) AND (ca.End_Date > GETDATE() OR ca.End_Date IS NULL)                        
                    WHERE gp.Group_Role_Id = 16 AND (gp.End_Date > getdate() OR gp.End_Date IS NULL) AND gp.Group_Id = Groups.Group_Id                  
                    GROUP BY gp.Group_Participant_Id ,gp.Group_Id, gp.Participant_ID
        ) a  GROUP BY Group_Id) AS No_of_Caucasian_Participants',
      'Group_Type_ID_Table.[Group_Type_ID] = 26 AND (Groups.[End_Date] > getDate() OR Groups.[End_Date] IS NULL)' )

SET IDENTITY_INSERT [dbo].[dp_Page_Views] OFF
GO