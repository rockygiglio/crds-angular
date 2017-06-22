USE [MinistryPlatform]
GO

SET IDENTITY_INSERT [dp_Page_Views] ON

INSERT INTO [dbo].[dp_Page_Views]
           ([Page_View_ID]
		   ,[View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By])
     VALUES
           (2300
		   ,'Email Page View'
           ,36
           ,'Columns for Email Notification'
           ,'cr_Childcare_Requests.[Childcare_Request_ID]
				, Requester_ID_Table.[Contact_ID]
				, Requester_ID_Table.[Email_Address]
				, Requester_ID_Table.[Display_Name]
                , Requester_ID_Table.[Nickname]
                , Requester_ID_Table.[Last_Name]
				, Ministry_ID_Table.[Ministry_Name]
				, Congregation_ID_Table.[Congregation_Name]
				, cr_Childcare_Requests.[Start_Date]
				, cr_Childcare_Requests.[End_Date]
				, cr_Childcare_Requests.[Childcare_Session],
Group_ID_Table.[Group_Name],Congregation_ID_Table_Childcare_Contact_Table.[Contact_ID] AS [Childcare_Contact_ID]
, Congregation_ID_Table_Childcare_Contact_Table.[Email_Address] AS [Childcare_Contact_Email_Address]'
           ,'cr_Childcare_Requests.[Childcare_Request_ID] > 0'
           ,null)
GO


