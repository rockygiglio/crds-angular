USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dbo].[dp_Pages] ON

INSERT INTO [dbo].[dp_Pages]
           ([Page_ID]
		   ,[Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Display_Search]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
		   ,[Filter_Clause]
           ,[Contact_ID_Field]
           ,[Display_Copy])
     VALUES
           (556
		   , N'Undivided Participants'
           , N'Undivided Participant'
           , N'Undivided Current Participants - provides fields required for email template merge fields.' 
           , 250 
           , 'Group_Participants'
           , 'Group_Participants_ID'
		   , NULL
           , 'Participant_ID_Table_Contact_ID_Table.[First_Name] AS [First_Name]
				, Participant_ID_Table_Contact_ID_Table.[Last_Name] AS [Last_Name]
                , Group_ID_Table.[Group_Name] AS [Group_Name]
                , Group_ID_Table.[Description] AS [Description]
                , Group_ID_Table_Congregation_ID_Table.[Congregation_Name] AS [Congregation_Name]
				, (SELECT Attribute_Name 
	            FROM Group_Participant_Attributes GPA, Attributes A 
	            WHERE GPA.Attribute_ID = A.Attribute_ID
	            AND GPA.Group_Participant_ID = Group_Participants.Group_Participant_ID
	            AND GETDATE() BETWEEN GPA.Start_Date AND ISNULL(GPA.End_Date,GETDATE()) 
	            AND Attribute_Type_ID = 85) AS [Facilitator_Training] 
				, (STUFF((SELECT ' ' + c.First_Name + ' ' + c.Last_Name + ' (' +  c.email_address + ') '
                    FROM Group_Participants gp
                    INNER JOIN Contacts c ON c.Participant_record = gp.Participant_Id
                    WHERE group_id =  Group_ID_Table.[Group_ID] 
                    AND Group_Role_ID = 22
                    FOR XML PATH('')), 1, 1, '')) AS [Facilitators]'
           , 'Participant_ID_Table_Contact_ID_Table.Display_Name'
		   , 'Group_ID_Table_Group_Type_ID_Table.Group_Type_ID = 26 AND (Group_Participants.End_Date > GetDate() OR Group_Participants.End_Date IS NULL) AND (Group_ID_Table.End_Date > GetDate() OR Group_ID_Table.End_Date IS NULL) AND Group_ID_Table_Parent_Group_Table.Group_ID IS NOT NULL'
           , 'Participant_ID_Table.Contact_ID'
           , 1);

SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO