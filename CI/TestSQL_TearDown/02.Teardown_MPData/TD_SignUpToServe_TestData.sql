Use [MinistryPlatform]
GO

DELETE FROM Responses where opportunity_id in (SELECT opportunity_id FROM opportunities where opportunity_title like '(t) KC%');

DELETE FROM Responses where opportunity_id in (SELECT opportunity_id FROM Opportunities where opportunity_title like '(t) Superbowl%');

DELETE FROM Responses where event_id in (select event_id from events where event_title like '(t) Superbowl Oakley%');

DELETE FROM Responses where event_id in (SELECT event_id from events where event_title like '(t) KC Nursery Oakley%');

DELETE FROM Opportunities WHERE Add_to_Group IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) KidsClub Oakley Group');

DELETE FROM Opportunities WHERE Add_to_Group IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Superbowl Oakley Group');

DELETE FROM Event_Groups WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) KidsClub Oakley Group');

DELETE from Event_Groups where EVENT_ROOM_ID in (select EVENT_room_id from Event_Rooms WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00')));

DELETE FROM Event_Rooms WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00'));

DELETE FROM Event_Equipment WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00'));

DELETE FROM Event_Participants WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00'));

DELETE FROM Event_Rooms WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 1:00'));

DELETE FROM Event_Equipment WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 1:00'));

DELETE FROM Event_Participants WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 1:00'));

DELETE FROM Event_Groups WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Superbowl Oakley Group');

DELETE FROM Event_Rooms WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00'));

DELETE FROM Event_Equipment WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00'));

DELETE FROM Event_Participants WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00'));

DELETE FROM Event_Rooms WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 3:00'));

DELETE FROM Event_Equipment WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 3:00'));

DELETE FROM Event_Participants WHERE Event_ID IN (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 3:00'));

DELETE FROM EVENT_GROUPS where EVENT_ID in (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00'));

DELETE FROM EVENT_GROUPS where EVENT_ID in (SELECT Event_ID FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00'));

DELETE FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00');

DELETE FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 3:00');

DELETE FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00');

DELETE FROM Events WHERE Event_Type_ID IN (SELECT Event_Type_ID FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 1:00');

DELETE FROM Group_Participants WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) KidsClub Oakley Group');

DELETE FROM Group_Participants WHERE Group_ID IN (SELECT Group_ID FROM Groups WHERE Group_Name = '(t) Superbowl Oakley Group');

DELETE from [dbo].cr_childcare_request_dates where child_care_request_id in (select child_care_request_id from cr_childcare_requests where group_id in (select group_id from groups where group_name in ('t) KidsClub Oakley Group','(t) Superbowl Oakley Group')));

DELETE FROM [dbo].cr_childcare_requests WHERE GROUP_ID in (select group_id from groups where group_name in ('(t) KidsClub Oakley Group','(t) Superbowl Oakley Group'));

DELETE FROM Groups WHERE Group_Name = '(t) KidsClub Oakley Group';

DELETE FROM Groups WHERE Group_Name = '(t) Superbowl Oakley Group';

DELETE FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 10:00';

DELETE FROM Event_Types WHERE Event_Type = '(t) Superbowl Oakley daily 3:00';

DELETE FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 11:00';

DELETE FROM Event_Types WHERE Event_Type = '(t) KC Nursery Oakley weekly 1:00';

