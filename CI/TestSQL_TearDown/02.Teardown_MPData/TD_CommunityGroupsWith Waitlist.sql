USE MinistryPlatform
GO	

DELETE from [dbo].Event_Participants where Group_Participant_id in (select GROUP_PARTICIPANT_ID from group_participants where GROUP_Id in (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Fathers Oakley CG'));

DELETE FROM Group_Participants WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Fathers Oakley CG');

DELETE from [dbo].Event_Participants where Group_Participant_id in (select GROUP_PARTICIPANT_ID from group_participants where GROUP_Id in (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Fathers Oakley CG - Waitlist'));

DELETE FROM Group_Participants WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Fathers Oakley CG - Waitlist');

UPDATE Groups SET Parent_Group = NULL  WHERE Group_Name = '(t) Fathers Oakley CG - Waitlist';

DELETE FROM [dbo].EVENT_GROUPS WHERE GROUP_ID = (select GROUP_ID from GROUPS where GROUP_NAME = '(t) Fathers Oakley CG');

DELETE FROM Groups WHERE  Group_Name = '(t) Fathers Oakley CG';

DELETE FROM Groups WHERE  Group_Name = '(t) Fathers Oakley CG - Waitlist';