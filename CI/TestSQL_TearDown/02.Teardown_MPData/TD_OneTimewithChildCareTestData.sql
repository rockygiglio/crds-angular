Use [MinistryPlatform]
GO

UPDATE Events SET Parent_Event_ID = null WHERE Event_Title = '(t) 1Time Mason with ChildCare - Childcare';

delete from event_groups where event_id in (select event_id from events where event_title like '(t) 1Time Mason%');

DELETE FROM Event_Groups WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name like '(t) 1Time Mason%');

DELETE from EVENT_Participants where group_id in (select group_id from groups where group_name like '(t) 1Time Mason%');

DELETE FROM Event_Equipment where event_id in (select event_id from events where Event_Title like '(t) 1Time Mason%');

DELETE FROM Events WHERE Event_Title like '(t) 1Time Mason%';

DELETE from group_participants where group_id in (select group_id from groups where group_name like '(t) 1Time Mason Group%');

DELETE from cr_childcare_request_dates where childcare_request_id in (select childcare_request_id from cr_childcare_requests where group_id in (select group_id from groups where group_name like ('(t) 1Time Mason%')));

DELETE FROM cr_childcare_requests WHERE GROUP_ID in (select group_id from groups where group_name like ('(t) 1Time Mason%'));

DELETE FROM Groups WHERE Group_Name like '(t) 1Time Mason%';
GO
