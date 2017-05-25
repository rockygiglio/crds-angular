USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Pages]
           ([Display_Name]
           ,[Singular_Name]
           ,[Description]
           ,[View_Order]
           ,[Table_Name]
           ,[Primary_Key]
           ,[Display_Search]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
           ,[Filter_Clause]
           ,[Display_Copy])
     VALUES
           ('My Childcare Requests'
           ,'My Childcare Request'
           ,'Childcare requester can view their requests or apply for childcare'
           ,2
           ,'cr_Childcare_Requests'
           ,'cr_Childcare_Request_ID'
           ,NULL
           ,('Group_ID_Table.[Group_Name] AS [Group] 
			 ,cr_Childcare_Requests.[Childcare_Start_Date] AS [Childcare Start Date] 
			 ,cr_Childcare_Requests.[Childcare_End_Date] AS [Childcare End Date] 
			 ,cr_Childcare_Requests.[No_of_Children_Attending] AS [No of Children Attending] 
			 ,Request_Status_ID_Table.[Request_Status] AS [Request Status]')
           ,'cr_Childcare_Request_ID'
           ,'Requester_ID_Table.User_Account = dp_UserID'
           ,1)
GO


