USE [MinistryPlatform]
GO

-- =============================================
-- Author:      John Cleaver
-- Create date: 2017-05-02
-- Description:	Alter Page View 2198 - Segmentation: Has Child of Age
-- so that it is using the new checkin data structure
-- =============================================

UPDATE [MinistryPlatform].[dbo].[dp_Page_Views]
SET [Field_List] = 'dp_Contact_Publications.Contact_Publication_ID, dp_Contact_Publications.Publication_ID, Contact_ID_Table.Contact_ID 
,(select count(*) from contacts c2 where c2.__Age < 1 and c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
and not exists (select * from group_participants where participant_id = c2.participant_record 
and group_id in (173939, 173938, 173937, 173936, 173935, 173934, 173933, 173932, 173931, 173930, 173929, 173928, 173927))) AS HAS_INFANT
,(select count(*) from contacts c2 where c2.__Age = 1 and c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
and not exists (select * from group_participants where participant_id = c2.participant_record 
and group_id in (173939, 173938, 173937, 173936, 173935, 173934, 173933, 173932, 173931, 173930, 173929, 173928, 173927))) AS HAS_1_YEAR
,(select count(*) from contacts c2 where c2.__Age = 2 and c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
and not exists (select * from group_participants where participant_id = c2.participant_record 
and group_id in (173939, 173938, 173937, 173936, 173935, 173934, 173933, 173932, 173931, 173930, 173929, 173928, 173927))) AS HAS_2_YEAR
,(select count(*) from contacts c2 where c2.__Age = 3 and c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
and not exists (select * from group_participants where participant_id = c2.participant_record 
and group_id in (173939, 173938, 173937, 173936, 173935, 173934, 173933, 173932, 173931, 173930, 173929, 173928, 173927))) AS HAS_3_YEAR
,(select count(*) from contacts c2 where c2.__Age = 4 and c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
and not exists (select * from group_participants where participant_id = c2.participant_record 
and group_id in (173939, 173938, 173937, 173936, 173935, 173934, 173933, 173932, 173931, 173930, 173929, 173928, 173927))) AS HAS_PREK_4
,(select count(*) from contacts c2 where c2.__Age = 5 and c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
and not exists (select * from group_participants where participant_id = c2.participant_record 
and group_id in (173939, 173938, 173937, 173936, 173935, 173934, 173933, 173932, 173931, 173930, 173929, 173928, 173927))) AS HAS_PREK_5'
WHERE Page_View_Id=2198
