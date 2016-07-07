USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM [dbo].[Group_Ended_Reasons] WHERE Group_Ended_Reason = 'We multiplied.')
BEGIN
INSERT INTO [dbo].[Group_Ended_Reasons]
(Group_Ended_Reason, Description                                                                     , Domain_ID) VALUES
('We multiplied'   , 'We recently split off in new directions AND saw solid growth within our group.', 1        );
END

IF NOT EXISTS(SELECT * FROM [dbo].[Group_Ended_Reasons] WHERE Group_Ended_Reason = 'We had a good run.')
BEGIN
INSERT INTO [dbo].[Group_Ended_Reasons]
(Group_Ended_Reason , Description                                                                                    , Domain_ID) VALUES
('We had a good run', 'We didn''t intentionally multiply, or have a fight AND break up. It was just time to move on.', 1        );
END

IF NOT EXISTS(SELECT * FROM [dbo].[Group_Ended_Reasons] WHERE Group_Ended_Reason = 'We didn''t click.')
BEGIN
INSERT INTO [dbo].[Group_Ended_Reasons]
(Group_Ended_Reason , Description                                                              , Domain_ID) VALUES
('We didn''t click.', 'It wasn''t a good fit, so we''re starting over. No biggie...it happens.', 1        );
END

IF NOT EXISTS(SELECT * FROM [dbo].[Group_Ended_Reasons] WHERE Group_Ended_Reason = 'Other')
BEGIN
INSERT INTO [dbo].[Group_Ended_Reasons]
(Group_Ended_Reason , Description        , Domain_ID) VALUES
('Other'            , 'Fill in the blank', 1        );
END