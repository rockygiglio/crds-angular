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
           ,[Display_Copy])
     VALUES
           (36,
		   'Childcare Requests'
           ,'Childcare Request'
           ,'Display childcare request for childcare site owners'
           ,10
           ,'cr_Childcare_Requests'
           ,'Childcare_Request_ID'
           , null
           ,('Requester_ID_Table.[Display_Name] AS [Requester] 
			 , Group_ID_Table.[Group_Name] AS [Group] 
			 , Ministry_ID_Table.[Ministry_Name] AS [Ministry] 
			 , Congregation_ID_Table.[Congregation_Name] AS [Congregation] 
			 , cr_Childcare_Requests.[Childcare_Start_Date] AS [Childcare Start Date] 
			 , cr_Childcare_Requests.[Childcare_End_Date] AS [Childcare End Date] 
			 , cr_Childcare_Requests.[Frequency] AS [Frequency] 
			 , cr_Childcare_Requests.[Time_Frame] AS [Preferred Time] 
			 , cr_Childcare_Requests.[No_of_Children_Attending] AS [No of Children Attending] 
			 , cr_Childcare_Requests.[Notes] AS [Notes] 
			 , Request_Status_ID_Table.[Request_Status] AS [Request Status]')
		   ,'Childcare_Request_ID'
           ,1);

	 SET IDENTITY_INSERT [dbo].[dp_Pages] OFF
GO



