
delete from ministryplatform.dbo.group_participants
from ministryplatform.[dbo].[Group_Participants] as gp
join ministryplatform.[dbo].[groups] as g
on g.group_ID = gp.group_ID
where g.Group_Type_ID = 4 and g.Ministry_ID = 2 and g.group_name not like 'Kids Club Grade 5'


--Bring back all active minor contacts and attempt to assign them a Check-In group by adding them as a group participant
INSERT INTO [dbo].[Group_Participants] 
([Group_ID],[Participant_ID],[Group_Role_ID],[Domain_ID],[Start_Date],[Employee_Role],[Notes],[Child_Care_Requested],[Need_Book])
Select G.Group_ID
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
--5+ year olds
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 5+ Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 5+ Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 5+ Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 5+ Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 5+ Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 5+ Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 5+ Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 5+ Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 5+ Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 5+ Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 5+ Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 5+ Year Old Jan'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 5+ Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 5+ Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 5+ Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 5+ Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 5+ Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 5+ Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 5+ Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 5+ Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 5+ Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 5+ Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 5+ Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 5+ Year Old Jan'
--4 year old
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 4 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 4 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 4 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 4 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 4 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 4 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 4 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 4 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 4 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 4 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 4 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 4 Year Old Jan'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 4 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 4 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 4 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 4 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 4 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 4 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 4 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 4 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 4 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 4 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 4 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 4 Year Old Jan'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 4 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 4 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 4 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 4 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 4 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 4 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 4 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 4 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 4 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 4 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 4 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 4 Year Old Jan'
--3 year old
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 3 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 3 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 3 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 3 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 3 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 3 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 3 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 3 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 3 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 3 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 3 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 3 Year Old Jan'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 3 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 3 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 3 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 3 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 3 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 3 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 3 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 3 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 3 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 3 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 3 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 3 Year Old Jan'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 3 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 3 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 3 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 3 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 3 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 3 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 3 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 3 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 3 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 3 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 3 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 3 Year Old Jan'
--2 year old
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 2 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 2 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 2 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 2 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 2 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 2 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 2 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 2 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 2 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 2 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 2 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 2 Year Old Jan'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 2 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 2 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 2 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 2 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 2 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 2 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 2 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 2 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 2 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 2 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 2 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 2 Year Old Jan'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 2 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 2 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 2 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 2 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 2 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 2 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 2 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 2 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 2 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 2 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 2 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 2 Year Old Jan'
--1 year old
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 1 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 1 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 1 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 1 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 1 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 1 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 1 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 1 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 1 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 1 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 1 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 1 Year Old Jan'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 1 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 1 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 1 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 1 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 1 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 1 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 1 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 1 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 1 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 1 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 1 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 1 Year Old Jan'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 1 Year Old Dec'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 1 Year Old Nov'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 1 Year Old Oct'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 1 Year Old Sep'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 1 Year Old Aug'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 1 Year Old July'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 1 Year Old June'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 1 Year Old May'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 1 Year Old April'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 1 Year Old March'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 1 Year Old Feb'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 1 Year Old Jan'
--11-12 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 11-12 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 11-12 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 11-12 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 11-12 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 11-12 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 11-12 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 11-12 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 11-12 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 11-12 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 11-12 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 11-12 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 11-12 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 11-12 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 11-12 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 11-12 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 11-12 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 11-12 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 11-12 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 11-12 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 11-12 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 11-12 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 11-12 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 11-12 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 11-12 Month Old (January)'
--10-11 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 10-11 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 10-11 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 10-11 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 10-11 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 10-11 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 10-11 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 10-11 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 10-11 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 10-11 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 10-11 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 10-11 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 10-11 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 10-11 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 10-11 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 10-11 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 10-11 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 10-11 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 10-11 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 10-11 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 10-11 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 10-11 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 10-11 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 10-11 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 10-11 Month Old (January)'
--9-10 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 9-10 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 9-10 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 9-10 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 9-10 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 9-10 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 9-10 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 9-10 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 9-10 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 9-10 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 9-10 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 9-10 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 9-10 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 9-10 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 9-10 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 9-10 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 9-10 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 9-10 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 9-10 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 9-10 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 9-10 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 9-10 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 9-10 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 9-10 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 9-10 Month Old (January)'
--8-9 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 8-9 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 8-9 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 8-9 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 8-9 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 8-9 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 8-9 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 8-9 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 8-9 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 8-9 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 8-9 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 8-9 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 8-9 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 8-9 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 8-9 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 8-9 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 8-9 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 8-9 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 8-9 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 8-9 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 8-9 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 8-9 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 8-9 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 8-9 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 8-9 Month Old (January)'
--7-8 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 7-8 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 7-8 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 7-8 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 7-8 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 7-8 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 7-8 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 7-8 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 7-8 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 7-8 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 7-8 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 7-8 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 7-8 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 7-8 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 7-8 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 7-8 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 7-8 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 7-8 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 7-8 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 7-8 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 7-8 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 7-8 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 7-8 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 7-8 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 7-8 Month Old (January)'
--6-7 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 6-7 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 6-7 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 6-7 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 6-7 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 6-7 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 6-7 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 6-7 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 6-7 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 6-7 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 6-7 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 6-7 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 6-7 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 6-7 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 6-7 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 6-7 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 6-7 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 6-7 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 6-7 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 6-7 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 6-7 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 6-7 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 6-7 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 6-7 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 6-7 Month Old (January)'
--5-6 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 5-6 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 5-6 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 5-6 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 5-6 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 5-6 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 5-6 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 5-6 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 5-6 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 5-6 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 5-6 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 5-6 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 5-6 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 5-6 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 5-6 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 5-6 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 5-6 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 5-6 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 5-6 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 5-6 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 5-6 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 5-6 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 5-6 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 5-6 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 5-6 Month Old (January)'
--4-5 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 4-5 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 4-5 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 4-5 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 4-5 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 4-5 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 4-5 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 4-5 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 4-5 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 4-5 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 4-5 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 4-5 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 4-5 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 4-5 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 4-5 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 4-5 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 4-5 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 4-5 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 4-5 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 4-5 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 4-5 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 4-5 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 4-5 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 4-5 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 4-5 Month Old (January)'
--3-4 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 3-4 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 3-4 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 3-4 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 3-4 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 3-4 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 3-4 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 3-4 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 3-4 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 3-4 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 3-4 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 3-4 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 3-4 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 3-4 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 3-4 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 3-4 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 3-4 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 3-4 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 3-4 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 3-4 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 3-4 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 3-4 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 3-4 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 3-4 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 3-4 Month Old (January)'
--2-3 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 2-3 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 2-3 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 2-3 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 2-3 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 2-3 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 2-3 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 2-3 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 2-3 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 2-3 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 2-3 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 2-3 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 2-3 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 2-3 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 2-3 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 2-3 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 2-3 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 2-3 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 2-3 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 2-3 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 2-3 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 2-3 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 2-3 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 2-3 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 2-3 Month Old (January)'
--1-2 months 
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 1-2 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 1-2 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 1-2 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 1-2 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 1-2 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 1-2 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 1-2 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 1-2 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 1-2 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 1-2 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 1-2 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 1-2 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 1-2 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 1-2 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 1-2 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 1-2 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 1-2 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 1-2 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 1-2 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 1-2 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 1-2 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 1-2 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 1-2 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 1-2 Month Old (January)'
--0-1 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 0-1 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 0-1 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 0-1 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 0-1 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 0-1 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 0-1 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 0-1 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 0-1 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 0-1 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 0-1 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 0-1 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 0-1 Month Old (January)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 12 then 'Kids Club 0-1 Month Old (December)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 11 then 'Kids Club 0-1 Month Old (November)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 10 then 'Kids Club 0-1 Month Old (October)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 9 then 'Kids Club 0-1 Month Old (September)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 8 then 'Kids Club 0-1 Month Old (August)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 7 then 'Kids Club 0-1 Month Old (July)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 6 then 'Kids Club 0-1 Month Old (June)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 5 then 'Kids Club 0-1 Month Old (May)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 4 then 'Kids Club 0-1 Month Old (April)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 3 then 'Kids Club 0-1 Month Old (March)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 2 then 'Kids Club 0-1 Month Old (February)'
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 1 then 'Kids Club 0-1 Month Old (January)'
else null end as Group_name
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
from ministryplatform.[dbo].[Contacts] as c (nolock)
join ministryplatform.[dbo].[Participants] as p (nolock)
on p.contact_id = c.contact_id 
where 
c.[Household_Position_ID] = 2 
and c.[Contact_Status_ID] = 1
and c.Household_id is not null
and (c.[HS_Graduation_Year] is null or c.[HS_Graduation_Year] > 2029)) as T1
join ministryplatform.[dbo].[Groups] as g
on g.group_name = T1.Group_name
and g.Group_Type_ID = 4
and g.Ministry_ID = 2
where T1.group_name is not null;


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
--Assign by rule 3
--when c.[HS_Graduation_Year] = 2024 then  173934 --5th grade   Kids Club Grade 5
when c.[HS_Graduation_Year] = 2025 then  173935 --4th grade   Kids Club Grade 4
when c.[HS_Graduation_Year] = 2026 then  173936 --3rd grade   Kids Club Grade 3
when c.[HS_Graduation_Year] = 2027 then  173937 --2nd grade   Kids Club Grade 2
when c.[HS_Graduation_Year] = 2028 then  173938 --1st grade   Kids Club Grade 1
when c.[HS_Graduation_Year] = 2029 then  173939 --K grade   Kids Club Kindergarten
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
from ministryplatform.[dbo].[Contacts] as c (nolock)
join ministryplatform.[dbo].[Participants] as p (nolock)
on p.contact_id = c.contact_id  
where c.[Household_Position_ID] = 2 
and c.[Contact_Status_ID] = 1
and c.Household_id is not null) as T1
where T1.group_id is not null
