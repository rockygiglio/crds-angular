USE [MinistryPlatform]
GO

/****** Object:  Table [dbo].[cr_Echeck_Registrations]    Script Date: 9/12/2016 4:09:26 PM ******/
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
	[Service_Day] [varchar](50) NULL,
	[Service_Time] [varchar](20) NULL,
	[Building_Name] [varchar](50) NULL 
 CONSTRAINT [PK_crds_Echeck_Registrations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_cr_Echeck_Registrations_Unique_Sarah_Id] UNIQUE NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

GRANT INSERT ON [dbo].[cr_Echeck_Registrations] TO [EcheckAgent];
GRANT UPDATE ON [dbo].[cr_Echeck_Registrations] TO [EcheckAgent];
GRANT SELECT ON [dbo].[cr_Echeck_Registrations] TO [EcheckAgent];

GO