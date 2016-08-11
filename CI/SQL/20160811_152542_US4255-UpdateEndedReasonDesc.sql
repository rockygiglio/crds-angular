USE [MinistryPlatform]
GO

UPDATE [dbo].[Group_Ended_Reasons] SET Description = '(We recently split off in new directions and saw solid growth within our group.)'
WHERE Group_Ended_Reason_ID = 1;

UPDATE [dbo].[Group_Ended_Reasons] SET Description = '(We didn''t intentionally multiply, or have a fight and break up. It was just time to move on.)'
WHERE Group_Ended_Reason_ID = 2;

UPDATE [dbo].[Group_Ended_Reasons] SET Description = '(It wasn''t a good fit, so we''re starting over. No biggie...it happens.)'
WHERE Group_Ended_Reason_ID = 3;
GO
