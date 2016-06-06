USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'cr_Childcare_Request_Dates')
BEGIN
	PRINT 'Dropping cr_Childcare_Request_Dates table'
	DROP TABLE [dbo].[cr_Childcare_Request_Dates]
END

PRINT 'Creating cr_Childcare_Request_Dates table'
CREATE TABLE [dbo].[cr_Childcare_Request_Dates](
	[Childcare_Request_Date_ID] [int] IDENTITY(1,1) NOT NULL,
	[Childcare_Request_ID] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL,
	[Childcare_Request_Date] [DateTime] NOT NULL,
	[Approved] [Bit] Not Null Default(0) 	
 CONSTRAINT [PK_Childcare_Request_Date_ID] PRIMARY KEY CLUSTERED 
(
	[Childcare_Request_Date_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

PRINT 'Adding foreign key for Childcare Request'
ALTER TABLE [dbo].[cr_Childcare_Request_Dates]  WITH CHECK ADD  CONSTRAINT [FK_Childcare_Requests_Dates_Childcare_Request] FOREIGN KEY([Childcare_Request_ID])
	REFERENCES [dbo].[cr_Childcare_Requests] ([Childcare_Request_ID])
GO

PRINT 'Adding foreign key for Domain'
ALTER TABLE [dbo].[cr_Childcare_Request_Dates]  WITH CHECK ADD  CONSTRAINT [FK_Childcare_Requests_Dates_Domain] FOREIGN KEY([Domain_ID])
	REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

PRINT 'Creating subpage Request_Dates to Childcare Requests page'
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
           ,36
           ,1
           ,'cr_Childcare_Request_Dates'
           ,'Childcare_Request_Date_ID'
           ,'Childcare_Request_Date, Approved'
           ,'Childcare_Request_Date'
		   ,'Childcare_Request_ID'
           ,1
           ,1)
GO


