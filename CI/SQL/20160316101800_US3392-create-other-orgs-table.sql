USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[cr_Other_Organizations](
	[Other_Organization_ID] [int] IDENTITY(1,1) NOT NULL,
	[Other_Organization] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_cr_Other_Organizations] PRIMARY KEY CLUSTERED 
(
	[Other_Organization_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


