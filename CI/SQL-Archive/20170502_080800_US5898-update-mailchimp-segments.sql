USE [MinistryPlatform]
GO

-- =============================================
-- Author:      John Cleaver
-- Create date: 2017-05-02
-- Description:	Alter Page View 92398 - Segmentation: Has Child of Grade
-- so that it is using the new Grade Group Ids
-- =============================================

UPDATE [MinistryPlatform].[dbo].[dp_Page_Views]
SET [Field_List] = 'dp_Contact_Publications.Contact_Publication_ID, dp_Contact_Publications.Publication_ID, 
	Contact_ID_Table.Contact_ID, 
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173939)) AS HAS_KINDG, 
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173938)) AS HAS_1_GRD,
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173937)) AS HAS_2_GRD,
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173936)) AS HAS_3_GRD,
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173935)) AS HAS_4_GRD,
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173934)) AS HAS_5_GRD, 
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173933)) AS HAS_6_GRD, 
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173932)) AS HAS_7_GRD,
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173931)) AS HAS_8_GRD,
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173930)) AS HAS_9_GRD,
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173929)) AS HAS_10_GRD,
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173928)) AS HAS_11_GRD,
	(select count(*) from contacts c2 where c2.Household_ID = Contact_ID_Table.Household_ID and c2.Contact_Status_ID = 1 
	and exists (select * from group_participants where participant_id = c2.participant_record and group_id = 173927)) AS HAS_12_GRD'
WHERE Page_View_Id=92398
