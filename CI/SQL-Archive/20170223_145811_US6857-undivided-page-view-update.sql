USE MinistryPlatform
GO

IF EXISTS (SELECT 1 FROM dp_Pages WHERE Page_ID = 556)
BEGIN
UPDATE dp_Pages
SET Default_Field_List = 'Participant_ID_Table_Contact_ID_Table.[First_Name] 
				, Participant_ID_Table_Contact_ID_Table.[Last_Name] 
                , Participant_ID_Table_Contact_ID_Table.[Email_Address] AS [Email]
				, Group_ID_Table.[Group_Name]
                , Group_ID_Table.[Description] 
                , Group_Role_ID_Table.Role_Title
                , Group_ID_Table_Congregation_ID_Table.[Congregation_Name] AS [Site]
				, (SELECT Attribute_Name 
	              FROM Group_Participant_Attributes GPA, Attributes A 
	              WHERE GPA.Attribute_ID = A.Attribute_ID
	              AND GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID
	              AND GETDATE() BETWEEN GPA.Start_Date AND ISNULL(GPA.End_Date,GETDATE()) 
	              AND Attribute_Type_ID = 85) AS [Facilitator_Training] 
				, (STUFF((SELECT '' ''+ c.First_Name + '' '' + c.Last_Name + '' (''  +  c.email_address + '') ''
                    FROM Group_Participants gp
                    INNER JOIN Contacts c ON c.Participant_record = gp.Participant_Id
                    WHERE group_id =  Group_ID_Table.[Group_ID] 
                    AND Group_Role_ID = 22
                    FOR XML PATH('')), 1, 1, '')) AS [Facilitators]		
			   , Group_Participants.[Notes]'
WHERE Page_ID = 556
END
