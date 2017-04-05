--Bring back all active minor MSM/HSM age contacts and attempt to assign them a Check-In group by adding them as a group participant
INSERT INTO [dbo].[Group_Participants]
([Group_ID],[Participant_ID],[Group_Role_ID],[Domain_ID],[Start_Date],[Employee_Role],[Notes],[Child_Care_Requested],[Need_Book])
Select T1.Group_ID
,T1.Participant_ID
,T1.Group_Role_ID
,T1.Domain_ID
,T1.Start_Date
,T1.Employee_Role
,T1.Notes
,T1.Child_Care_Requested
,T1.Need_Book
from (
select 
CASE 
when c.[HS_Graduation_Year] = 2017 then 173927  --12th grade   High School Grade 12
when c.[HS_Graduation_Year] = 2018 then 173928  --11th grade   High School Grade 11
when c.[HS_Graduation_Year] = 2019 then 173929  --10th grade   High School Grade 10
when c.[HS_Graduation_Year] = 2020 then 173930  --9th grade    High School Grade 9
when c.[HS_Graduation_Year] = 2021 then 173931  --8th grade    Student Ministry Grade 8
when c.[HS_Graduation_Year] = 2022 then 173932  --7th grade    Student Ministry Grade 7
when c.[HS_Graduation_Year] = 2023 then 173933  --6th grade    Student Ministry Grade 6

else null end as Group_ID
--,c.[Contact_ID]
--,c.[Date_of_Birth]
--,c.[HS_Graduation_Year]
--,c.[Display_Name]
--,c.[First_Name]
--,c.[Middle_Name]
--,c.[Last_Name]
--,c.[Nickname]
,p.Participant_ID
,16 as Group_Role_ID
,1 as Domain_ID
,getdate() as Start_Date
,0 as Employee_Role
,'Initial Check-In load 2017' as Notes
,0 as Child_Care_Requested
,0 as Need_Book
from [dbo].[Contacts] as c (nolock)
join [dbo].[Participants] as p (nolock)
on p.contact_id = c.contact_id  
where c.[Household_Position_ID] = 2
and not exists (select * from groups inner join group_participants
	on groups.group_id = group_participants.group_id where group_participants.participant_id = p.Participant_ID
	and groups.group_type_id = 4 and group_participants.end_date is null)  
and c.[Contact_Status_ID] = 1
and c.Household_id is not null) as T1
where T1.group_id is not null
