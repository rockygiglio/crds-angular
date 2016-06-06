USE [MinistryPlatform]
GO

PRINT 'Creating subpage Request_Dates to My Childcare Requests page'
INSERT INTO [dbo].[dp_Sub_Pages]
           ([Display_Name]
           ,[Singular_Name]
           ,[Page_ID]
           ,[View_Order]
           ,[Primary_Table]
           ,[Primary_Key]
           ,[Default_Field_List]
           ,[Selected_Record_Expression]
		   ,[Filter_Key]
           ,[Relation_Type_ID]
           ,[Display_Copy])
     VALUES
           ('Request Dates'
           ,'Request Date'
           ,(SELECT Page_ID FROM dp_Pages WHERE Display_Name = 'My Childcare Requests')
           ,1
           ,'cr_Childcare_Request_Dates'
           ,'Childcare_Request_Date_ID'
           ,'Childcare_Request_Date, Approved'
           ,'Childcare_Request_Date'
		   ,'Childcare_Request_ID'
           ,1
           ,1)
GO