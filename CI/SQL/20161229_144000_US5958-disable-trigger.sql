use [MinistryPlatform]
GO

IF EXISTS  (select * from sys.triggers where name = 'tr_turn_off_event_registration')
BEGIN
  DISABLE TRIGGER [dbo].[tr_turn_off_event_registration] ON Event_Participants
END 