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
           ,[Contact_ID_Field]
           ,[Display_Copy])
     VALUES
           (20
		   ,'Volunteer Checklist'
           ,'Volunteer Checklist'
           ,'Printable Checklist with Volunteer and Project details' 
           ,20
           ,'cr_Group_connector_Registrations'
           ,'Group_Connector_Registration_ID'
		   ,NULL
           ,'Group_Connector_ID_Table_Project_ID_Table.[Project_Name] AS [Project Name]
		        , Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[First_Name] AS [Registrant First Name]
				, Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[Last_Name] AS [Registrant Last Name]
				, Registration_ID_Table.[_Family_Count] AS [Family Count]
				, Group_Connector_ID_Table_Project_ID_Table_Location_ID_Table.[Location_Name] AS [Launch Site]
				, Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Group Connector]
				, [dbo].crds_GoVolunteerSkills(Group_Connector_Registration_ID) as [Skills]
				,[dbo].crds_GoVolunteerEquipment(Group_Connector_Registration_ID) as [Equipment]
				, Registration_ID_Table_Initiative_ID_Table.[Initiative_Name] AS [Initiative]'
           ,'Group_Connector_Registration_ID'
           ,'Registration_ID_Table_Participant_ID_Table.Contact_ID'
           ,1);


		   SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO


