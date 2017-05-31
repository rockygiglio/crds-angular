USE MinistryPlatform
GO

IF NOT EXISTS(SELECT 1 FROM sys.Tables WHERE  Name = N'cr_Group_Leader_Statuses' AND Type = N'U')
BEGIN
    CREATE TABLE cr_Group_Leader_Statuses (
		Group_Leader_Status_ID  [int] IDENTITY(1,1) NOT NULL,
		Group_Leader_Status [nvarchar](50) NOT NULL,
		Sort_Order [int] NOT NULL
		CONSTRAINT [PK_Group_Leader_Statuses] PRIMARY KEY CLUSTERED 
		(
			[Group_Leader_Status_ID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	INSERT INTO cr_Group_Leader_Statuses (Group_Leader_Status, Sort_Order)
	VALUES 
		('Not Applied', 0),
		('Interested', 1),
		('Applied', 2),
		('Approved', 3),
		('Denied', 4)
END 

IF NOT EXISTS(SELECT 1 FROM sys.columns WHERE Name = N'Group_Leader_Status_ID' AND Object_ID = Object_ID(N'dbo.Participants'))
BEGIN
    ALTER TABLE dbo.Participants
	ADD  Group_Leader_Status_ID [int] NOT NULL Default(1)
END

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Participants_Group_Leader_Statuses]') AND parent_object_id = OBJECT_ID(N'[dbo].[Participants]'))
ALTER TABLE dbo.Participants ADD CONSTRAINT FK_Participants_Group_Leader_Statuses FOREIGN KEY (Group_Leader_Status_ID) 
REFERENCES dbo.cr_Group_Leader_Statuses (Group_Leader_Status_ID)
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Participants_Group_Leader_Statuses]') AND parent_object_id = OBJECT_ID(N'[dbo].[Participants]'))
ALTER TABLE [dbo].[Participants] CHECK CONSTRAINT [FK_Participants_Group_Leader_Statuses]