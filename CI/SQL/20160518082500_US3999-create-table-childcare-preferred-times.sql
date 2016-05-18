USE Ministryplatform
GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='cr_Childcare_Preferred_Times' AND xtype='U')
BEGIN

    CREATE TABLE cr_Childcare_Preferred_Times (
        [Childcare_Preferred_Time_ID] [int] IDENTITY(1,1) NOT NULL,
		[Congregation_ID] [int] NOT NULL,
		[Childcare_Start_Time] [time] NOT NULL,
		[Childcare_End_Time] [time] NOT NULL,
		[Childcare_Day_ID] [INT] NOT NULL,
		[Domain_ID] [int] NOT NULL,
		CONSTRAINT [PK_Childcare_Preferred_Times] PRIMARY KEY CLUSTERED 
		(
			[Childcare_Preferred_Time_ID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	
	-- FK for DomainID
	ALTER TABLE [dbo].[cr_Childcare_Preferred_Times]  WITH CHECK ADD  CONSTRAINT [FK_ChildcarPreferredTimes_Domain] FOREIGN KEY([Domain_ID])
	REFERENCES [dbo].[dp_Domains] ([Domain_ID])
	
	ALTER TABLE [dbo].[cr_Childcare_Preferred_Times] CHECK CONSTRAINT [FK_ChildcarPreferredTimes_Domain]

	-- FK for CongregationID
	ALTER TABLE [dbo].[cr_Childcare_Preferred_Times]  WITH CHECK ADD  CONSTRAINT [FK_ChildcarPreferredTimes_Congregation] FOREIGN KEY([Congregation_ID])
	REFERENCES [dbo].[Congregations] ([Congregation_ID])
	
	ALTER TABLE [dbo].[cr_Childcare_Preferred_Times] CHECK CONSTRAINT [FK_ChildcarPreferredTimes_Congregation]

	-- FK for Day of Week
	ALTER TABLE [dbo].[cr_Childcare_Preferred_Times]  WITH CHECK ADD  CONSTRAINT [FK_ChildcarPreferredTimes_MeetingDay] FOREIGN KEY([Childcare_Day_ID])
	REFERENCES [dbo].[Meeting_Days] ([Meeting_Day_ID])
	
	ALTER TABLE [dbo].[cr_Childcare_Preferred_Times] CHECK CONSTRAINT [FK_ChildcarPreferredTimes_MeetingDay]
	
END


