USE [MinistryPlatform]
GO

UPDATE [dbo].[Group_Ended_Reasons] 
SET Description = '(We recently split off in new directions and saw solid growth within our group.)',
Group_Ended_Reason = 'We''re branching out'
WHERE Group_Ended_Reason_ID = 1;

UPDATE [dbo].[Group_Ended_Reasons] SET Description = '(We didn''t intentionally multiply, or have a fight and break up. It was just time to move on.)'
WHERE Group_Ended_Reason_ID = 2;

UPDATE [dbo].[Group_Ended_Reasons] SET Description = '(It wasn''t a good fit, so we''re starting over. No biggie...it happens.)'
WHERE Group_Ended_Reason_ID = 3;
GO

If NOT EXISTS(SELECT * FROM [dbo].[Group_Ended_Reasons] WHERE Group_Ended_Reason = 'Other')
BEGIN
INSERT INTO [dbo].[Group_Ended_Reasons] 
(Group_Ended_Reason, Domain_ID) VALUES
('Other'           , 1        );
END