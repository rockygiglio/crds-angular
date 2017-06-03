use MinistryPlatform


IF NOT EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Role_ID' AND Object_ID = Object_ID(N'cr_GroupConnectorRegistrations'))
BEGIN
    -- Add the column
	ALTER TABLE [dbo].[cr_GroupConnectorRegistrations] 
		ADD Role_ID int NOT NULL default 16

	ALTER TABLE [dbo].[cr_GroupConnectorRegistrations]  WITH CHECK ADD  CONSTRAINT [FK_GroupConnectorRegistrations_Role] FOREIGN KEY([Role_ID])
		REFERENCES [dbo].[Group_Roles] ([Group_Role_ID])

END

IF EXISTS(SELECT * FROM sys.columns 
            WHERE Name = N'Role_ID' AND Object_ID = Object_ID(N'cr_Registrations'))
BEGIN
	-- Drop the constraint
	ALTER TABLE [dbo].[cr_Registrations] 
		DROP CONSTRAINT FK_Registrations_Role

	-- Drop the column
	ALTER TABLE [dbo].[cr_Registrations] 
		DROP COLUMN Role_ID ;
END

GO
