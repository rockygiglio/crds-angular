USE [MinistryPlatform]
GO

CREATE TABLE [dbo].[cr_Childcare_Request_Statuses](
	[Childcare_Request_Status_ID] [int] IDENTITY(1,1) NOT NULL,
	[Request_Status] [nvarchar](50) NOT NULL,
	[Domain_ID] [int] NOT NULL,
	CONSTRAINT [PK_Childcare_Request_Statuses] PRIMARY KEY CLUSTERED 
(
	[Childcare_Request_Status_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO