use [MinistryPlatform]
GO

update dp_Processes
set Trigger_Fields = 'Participant_ID'
where Process_ID = 36 --Process JoinGroupNotification

GO