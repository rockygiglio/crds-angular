USE [MinistryPlatform]
GO

UPDATE [dbo].[Group_Ended_Reasons] 
SET Description = '(We''ve seen solid growth, so we''re splitting off to start new groups.)',
Group_Ended_Reason = 'We''re branching out.'
WHERE Group_Ended_Reason_ID = 1;

UPDATE [dbo].[Group_Ended_Reasons] SET Description = '(We''re not fighting, breaking up or splitting off into new groups. It''s just time to move on.)',
Group_Ended_Reason = 'We had a good run.'
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