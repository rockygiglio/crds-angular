Use [MinistryPlatform]
GO

IF NOT EXISTS (select * from cr_Invitation_Types where Invitation_Type = 'Groups')
BEGIN
INSERT INTO [dbo].cr_Invitation_Types 
(Invitation_Type, Description)  VALUES
('Groups'       , 'Invitations for Small Group Finder Tool');
END
GO

IF NOT EXISTS (select * from cr_Invitation_Types where Invitation_Type = 'Trips')
BEGIN
INSERT INTO [dbo].cr_Invitation_Types 
(Invitation_Type, Description)  VALUES
('Trips'       , 'Invitations for GO Trips (Needs Refactored to use this)');
END
GO