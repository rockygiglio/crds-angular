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
           (30
		   ,'Volunteer Project Info'
           ,'Volunteer Project Info'
           ,'Volunteer Project Info' 
           ,130
           ,'cr_Group_connector_Registrations'
           ,'Group_Connector_Registration_ID'
		   ,NULL
           ,'Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[First_Name] AS [Volunteer First Name]
				 , Group_Connector_ID_Table_Project_ID_Table_Location_ID_Table.[Location_Name] AS [Launch Site] 
				,[dbo].crds_GoLocAddress(Group_Connector_Registration_ID) as [Launch Site Address]  
				, Group_Connector_ID_Table_Project_ID_Table.[Check_In_Floor] AS [Check-In Floor #]
				, Group_Connector_ID_Table_Project_ID_Table.[Check_In_Area] AS [Check-In Area]
				, Group_Connector_ID_Table_Project_ID_Table.[Check_In_Room_Number] AS [Check-In Room Number]
				, [dbo].[crds_GoTCInfo] (Group_Connector_Registration_ID, 1) as [TC Name]
				, [dbo].[crds_GoTCInfo] (Group_Connector_Registration_ID, 2) as [TC Contact]
				, Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Group Connector Name]
				, Group_Connector_ID_Table_Project_ID_Table.[Project_Name] AS [Project Name]
				, Group_Connector_ID_Table_Project_ID_Table_Project_Type_ID_Table.[Description] AS [Project Type]
				, Group_Connector_ID_Table_Project_ID_Table.[Note_To_Volunteers_1] AS [Note To Volunteers 1]
				, Group_Connector_ID_Table_Project_ID_Table.[Note_To_Volunteers_2] AS [Note To Volunteers 2]
				, Group_Connector_ID_Table_Project_ID_Table_Address_ID_Table.[Address_Line_1] AS [Project Street Address]
				, Group_Connector_ID_Table_Project_ID_Table_Address_ID_Table.[City] AS [Project City] 
				, Group_Connector_ID_Table_Project_ID_Table_Address_ID_Table.[State/Region] AS [Project State]
				, Group_Connector_ID_Table_Project_ID_Table_Address_ID_Table.[Postal_Code] AS [Project Zip] 
				, Group_Connector_ID_Table_Project_ID_Table.[Project_Parking_Location] AS [Parking Location]'
           ,'Group_Connector_Registration_ID'
           ,'Registration_ID_Table_Participant_ID_Table.Contact_ID'
           ,1);


		   SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO


