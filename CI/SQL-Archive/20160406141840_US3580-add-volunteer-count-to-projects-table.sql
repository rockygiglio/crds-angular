USE [MinistryPlatform]
GO

ALTER TABLE cr_Projects
    ADD [_Volunteer_Count]  AS ([dbo].[crds_GoVolunteerProjectMemberCount](Project_ID));