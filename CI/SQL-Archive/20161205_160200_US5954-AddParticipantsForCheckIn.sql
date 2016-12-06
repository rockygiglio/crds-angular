--This is an INSERT script to add active minor children to the Participants table that are not currently there
INSERT INTO  [MinistryPlatform].[dbo].[Participants] 
([Contact_ID],[Participant_Type_ID],[Participant_Start_Date],[Notes],[Domain_ID],[Approved_Small_Group_Leader])
select 
c.Contact_ID, 
2 as Participant_Type_ID, 
getdate() as Participant_Start_Date, 
'Initial Check-In load 2017' as Notes, --NEED TO DECIDE WHAT THIS SHOULD SAY
1 as Domain_ID, 
0 as Approved_Small_Group_Leader
from [dbo].[Contacts] as c (nolock)
left join [dbo].[Participants] as p (nolock)
on p.contact_id = c.contact_id
where c.[Household_Position_ID] = 2 
and c.[Contact_Status_ID] = 1
and p.[Participant_ID] is null
and c.household_id is not null;