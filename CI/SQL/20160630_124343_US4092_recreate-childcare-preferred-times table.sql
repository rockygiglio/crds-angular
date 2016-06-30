USE MinistryPlatform
GO

DROP TABLE [dbo].[cr_Childcare_Preferred_Times];

CREATE TABLE [dbo].[cr_Childcare_Preferred_Times](
	[Childcare_Preferred_Time_ID] [int] IDENTITY(1,1) NOT NULL,
	[Congregation_ID] [int] NOT NULL,
	[Childcare_Day_ID] [int] NOT NULL,
	[Childcare_Start_Time] [time](7) NOT NULL,
	[Childcare_End_Time] [time](7) NOT NULL,
	[End_Date] [datetime] NULL,
	[Domain_ID] [int] NOT NULL,
 CONSTRAINT [PK_Childcare_Preferred_Times] PRIMARY KEY CLUSTERED 
(
	[Childcare_Preferred_Time_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[cr_Childcare_Preferred_Times]  WITH CHECK ADD  CONSTRAINT [FK_ChildcarPreferredTimes_Congregation] FOREIGN KEY([Congregation_ID])
REFERENCES [dbo].[Congregations] ([Congregation_ID])
GO

ALTER TABLE [dbo].[cr_Childcare_Preferred_Times] CHECK CONSTRAINT [FK_ChildcarPreferredTimes_Congregation]
GO

ALTER TABLE [dbo].[cr_Childcare_Preferred_Times]  WITH CHECK ADD  CONSTRAINT [FK_ChildcarPreferredTimes_Domain] FOREIGN KEY([Domain_ID])
REFERENCES [dbo].[dp_Domains] ([Domain_ID])
GO

ALTER TABLE [dbo].[cr_Childcare_Preferred_Times] CHECK CONSTRAINT [FK_ChildcarPreferredTimes_Domain]
GO

ALTER TABLE [dbo].[cr_Childcare_Preferred_Times]  WITH CHECK ADD  CONSTRAINT [FK_ChildcarPreferredTimes_MeetingDay] FOREIGN KEY([Childcare_Day_ID])
REFERENCES [dbo].[Meeting_Days] ([Meeting_Day_ID])
GO

ALTER TABLE [dbo].[cr_Childcare_Preferred_Times] CHECK CONSTRAINT [FK_ChildcarPreferredTimes_MeetingDay]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sessions will not be available after the date listed in this field.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'cr_Childcare_Preferred_Times', @level2type=N'COLUMN',@level2name=N'End_Date'
GO


