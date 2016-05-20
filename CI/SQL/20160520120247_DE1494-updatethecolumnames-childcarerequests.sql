USE MinistryPlatform
GO

DROP TABLE cr_Childcare_Requests;

CREATE TABLE [dbo].[cr_Childcare_Requests](
	[Childcare_Request_ID] [int] IDENTITY(1,1) NOT NULL,
	[Requester_ID] [int] NOT NULL,
	[Ministry_ID] [int] NOT NULL,
	[Congregation_ID] [int] NOT NULL,
	[Group_ID] [int] NOT NULL,
	[Start_Date] [Date] NOT NULL,
	[End_Date] [Date] NOT NULL,
	[Frequency] [nvarchar](50) NOT NULL,
	[Time_Frame] [nvarchar](100) NOT NULL,
	[No_of_Children_Attending] [int] NOT NULL,
	[Notes] [nvarchar](250) NULL,
	[Request_Status_ID] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_Childcare_Requests] PRIMARY KEY CLUSTERED 
(
	[Childcare_Request_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Childcare_Requests]  WITH CHECK ADD  CONSTRAINT [FK_Childcare_Requests_Congregation] FOREIGN KEY([Congregation_ID])
REFERENCES [dbo].[Congregations] ([Congregation_ID])
GO

ALTER TABLE [dbo].[cr_Childcare_Requests] CHECK CONSTRAINT [FK_Childcare_Requests_Congregation]
GO

ALTER TABLE [dbo].[cr_Childcare_Requests]  WITH CHECK ADD  CONSTRAINT [FK_Childcare_Requests_Contact] FOREIGN KEY([Requester_ID])
REFERENCES [dbo].[Contacts] ([Contact_ID])
GO

ALTER TABLE [dbo].[cr_Childcare_Requests] CHECK CONSTRAINT [FK_Childcare_Requests_Contact]
GO

ALTER TABLE [dbo].[cr_Childcare_Requests]  WITH CHECK ADD  CONSTRAINT [FK_Childcare_Requests_Domain] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Childcare_Requests] CHECK CONSTRAINT [FK_Childcare_Requests_Domain]
GO

ALTER TABLE [dbo].[cr_Childcare_Requests]  WITH CHECK ADD  CONSTRAINT [FK_Childcare_Requests_Group] FOREIGN KEY([Group_ID])
REFERENCES [dbo].[Groups] ([Group_ID])
GO

ALTER TABLE [dbo].[cr_Childcare_Requests] CHECK CONSTRAINT [FK_Childcare_Requests_Group]
GO

ALTER TABLE [dbo].[cr_Childcare_Requests]  WITH CHECK ADD  CONSTRAINT [FK_Childcare_Requests_Ministry] FOREIGN KEY([Ministry_ID])
REFERENCES [dbo].[Ministries] ([Ministry_ID])
GO

ALTER TABLE [dbo].[cr_Childcare_Requests] CHECK CONSTRAINT [FK_Childcare_Requests_Ministry]
GO

ALTER TABLE [dbo].[cr_Childcare_Requests]  WITH CHECK ADD  CONSTRAINT [FK_Childcare_Requests_Request_Status] FOREIGN KEY([Request_Status_ID])
REFERENCES [dbo].[cr_Childcare_Request_Statuses] ([Childcare_Request_Status_ID])
GO

ALTER TABLE [dbo].[cr_Childcare_Requests] CHECK CONSTRAINT [FK_Childcare_Requests_Request_Status]
GO


