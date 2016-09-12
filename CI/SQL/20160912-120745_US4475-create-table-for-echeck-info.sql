USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Echeck_Registrations]    Script Date: 9/12/2016 1:06:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[cr_Echeck_Registrations]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[cr_Echeck_Registrations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Sarah_ID] [int] NOT NULL,
	[Child_ID] [int] NOT NULL,
	[Checkin_Date] [datetime] NULL,
	[Checkin_Time] [time](7) NULL,
 CONSTRAINT [PK_crds_Echeck_Registrations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


