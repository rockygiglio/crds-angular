
--delete from ministryplatform.dbo.group_participants
from ministryplatform.[dbo].[Group_Participants] as gp
join ministryplatform.[dbo].[groups] as g
on g.group_ID = gp.group_ID
where g.Group_Type_ID = 4 and g.Ministry_ID = 2



--Bring back all active minor contacts and attempt to assign them a Check-In group by adding them as a group participant
--INSERT INTO [dbo].[Group_Participants] 
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
--5+ year olds
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 12 then 173951 -- Kids Club 5+ Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 11 then 173950 -- Kids Club 5+ Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 10 then 173949 -- Kids Club 5+ Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 9 then 173948 -- Kids Club 5+ Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 8 then 173947 -- Kids Club 5+ Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 7 then 173946 -- Kids Club 5+ Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 6 then 173945 -- Kids Club 5+ Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 5 then 173944 -- Kids Club 5+ Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 4 then 173943 -- Kids Club 5+ Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 3 then 173942 -- Kids Club 5+ Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 2 then 173941 -- Kids Club 5+ Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) > 60 and DatePart(MM,C.Date_of_Birth)= 1 then 173940 -- Kids Club 5+ Year Old Jan
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 173951 -- Kids Club 5+ Year Old Dec 
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 173950 -- Kids Club 5+ Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 173949 -- Kids Club 5+ Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 173948 -- Kids Club 5+ Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 173947 -- Kids Club 5+ Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 173946 -- Kids Club 5+ Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 173945 -- Kids Club 5+ Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 173944 -- Kids Club 5+ Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 173943 -- Kids Club 5+ Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 173942 -- Kids Club 5+ Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 173941 -- Kids Club 5+ Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 173940 -- Kids Club 5+ Year Old Jan
--4 year old
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 173963 -- Kids Club 4 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 173962 -- Kids Club 4 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 173961 -- Kids Club 4 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 173960 -- Kids Club 4 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 173959 -- Kids Club 4 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 173958 -- Kids Club 4 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 173957 -- Kids Club 4 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 173956 -- Kids Club 4 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 173955 -- Kids Club 4 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 173954 -- Kids Club 4 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 173953 -- Kids Club 4 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 60 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 173952 -- Kids Club 4 Year Old Jan
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 12 then 173963 -- Kids Club 4 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 11 then 173962 -- Kids Club 4 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 10 then 173961 -- Kids Club 4 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 9 then 173960 -- Kids Club 4 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 8 then 173959 -- Kids Club 4 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 7 then 173958 -- Kids Club 4 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 6 then 173957 -- Kids Club 4 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 5 then 173956 -- Kids Club 4 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 4 then 173955 -- Kids Club 4 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 3 then 173954 -- Kids Club 4 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 2 then 173953 -- Kids Club 4 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 49 and 59 and DatePart(MM,C.Date_of_Birth)= 1 then 173952 -- Kids Club 4 Year Old Jan
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 173963 -- Kids Club 4 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 173962 -- Kids Club 4 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 173961 -- Kids Club 4 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 173960 -- Kids Club 4 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 173959 -- Kids Club 4 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 173958 -- Kids Club 4 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 173957 -- Kids Club 4 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 173956 -- Kids Club 4 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 173955 -- Kids Club 4 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 173954 -- Kids Club 4 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 173953 -- Kids Club 4 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 173952 -- Kids Club 4 Year Old Jan
--3 year old
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 173975 -- Kids Club 3 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 173974 -- Kids Club 3 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 173973 -- Kids Club 3 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 173972 -- Kids Club 3 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 173971 -- Kids Club 3 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 173970 -- Kids Club 3 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 173969 -- Kids Club 3 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 173968 -- Kids Club 3 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 173967 -- Kids Club 3 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 173966 -- Kids Club 3 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 173965 -- Kids Club 3 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 48 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 173964 -- Kids Club 3 Year Old Jan
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 12 then 173975 -- Kids Club 3 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 11 then 173974 -- Kids Club 3 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 10 then 173973 -- Kids Club 3 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 9 then 173972 -- Kids Club 3 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 8 then 173971 -- Kids Club 3 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 7 then 173970 -- Kids Club 3 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 6 then 173969 -- Kids Club 3 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 5 then 173968 -- Kids Club 3 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 4 then 173967 -- Kids Club 3 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 3 then 173966 -- Kids Club 3 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 2 then 173965 -- Kids Club 3 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 37 and 47 and DatePart(MM,C.Date_of_Birth)= 1 then 173964 -- Kids Club 3 Year Old Jan
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 173975 -- Kids Club 3 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 173974 -- Kids Club 3 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 173973 -- Kids Club 3 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 173972 -- Kids Club 3 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 173971 -- Kids Club 3 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 173970 -- Kids Club 3 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 173969 -- Kids Club 3 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 173968 -- Kids Club 3 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 173967 -- Kids Club 3 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 173966 -- Kids Club 3 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 173965 -- Kids Club 3 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 173964 -- Kids Club 3 Year Old Jan
--2 year old
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 173987 -- Kids Club 2 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 173986 -- Kids Club 2 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 173985 -- Kids Club 2 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 173984 -- Kids Club 2 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 173983 -- Kids Club 2 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 173982 -- Kids Club 2 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 173981 -- Kids Club 2 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 173980 -- Kids Club 2 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 173979 -- Kids Club 2 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 173978 -- Kids Club 2 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 173977 -- Kids Club 2 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 36 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 173976 -- Kids Club 2 Year Old Jan
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 12 then 173987 -- Kids Club 2 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 11 then 173986 -- Kids Club 2 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 10 then 173985 -- Kids Club 2 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 9 then 173984 -- Kids Club 2 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 8 then 173983 -- Kids Club 2 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 7 then 173982 -- Kids Club 2 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 6 then 173981 -- Kids Club 2 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 5 then 173980 -- Kids Club 2 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 4 then 173979 -- Kids Club 2 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 3 then 173978 -- Kids Club 2 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 2 then 173977 -- Kids Club 2 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 25 and 35 and DatePart(MM,C.Date_of_Birth)= 1 then 173976 -- Kids Club 2 Year Old Jan
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 173987 -- Kids Club 2 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 173986 -- Kids Club 2 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 173985 -- Kids Club 2 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 173984 -- Kids Club 2 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 173983 -- Kids Club 2 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 173982 -- Kids Club 2 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 173981 -- Kids Club 2 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 173980 -- Kids Club 2 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 173979 -- Kids Club 2 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 173978 -- Kids Club 2 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 173977 -- Kids Club 2 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 173976 -- Kids Club 2 Year Old Jan
--1 year old
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 173999 -- Kids Club 1 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 173998 -- Kids Club 1 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 173997 -- Kids Club 1 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 173996 -- Kids Club 1 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 173995 -- Kids Club 1 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 173994 -- Kids Club 1 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 173993 -- Kids Club 1 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 173992 -- Kids Club 1 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 173991 -- Kids Club 1 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 173990 -- Kids Club 1 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 173989 -- Kids Club 1 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 24 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 173988 -- Kids Club 1 Year Old Jan
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 12 then 173999 -- Kids Club 1 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 11 then 173998 -- Kids Club 1 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 10 then 173997 -- Kids Club 1 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 9 then 173996 -- Kids Club 1 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 8 then 173995 -- Kids Club 1 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 7 then 173994 -- Kids Club 1 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 6 then 173993 -- Kids Club 1 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 5 then 173992 -- Kids Club 1 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 4 then 173991 -- Kids Club 1 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 3 then 173990 -- Kids Club 1 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 2 then 173989 -- Kids Club 1 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) between 13 and 23 and DatePart(MM,C.Date_of_Birth)= 1 then 173988 -- Kids Club 1 Year Old Jan
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 173999 -- Kids Club 1 Year Old Dec
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 173998 -- Kids Club 1 Year Old Nov
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 173997 -- Kids Club 1 Year Old Oct
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 173996 -- Kids Club 1 Year Old Sep
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 173995 -- Kids Club 1 Year Old Aug
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 173994 -- Kids Club 1 Year Old July
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 173993 -- Kids Club 1 Year Old June
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 173992 -- Kids Club 1 Year Old May
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 173991 -- Kids Club 1 Year Old April
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 173990 -- Kids Club 1 Year Old March
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 173989 -- Kids Club 1 Year Old Feb
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 173988 -- Kids Club 1 Year Old Jan
--11-12 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177026 -- Kids Club 11-12 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177014 -- Kids Club 11-12 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177002 -- Kids Club 11-12 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176990 -- Kids Club 11-12 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176978 -- Kids Club 11-12 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176966 -- Kids Club 11-12 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176954 -- Kids Club 11-12 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176942 -- Kids Club 11-12 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176930 -- Kids Club 11-12 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176918 -- Kids Club 11-12 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176906 -- Kids Club 11-12 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 12 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176894 -- Kids Club 11-12 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177026 -- Kids Club 11-12 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177014 -- Kids Club 11-12 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177002 -- Kids Club 11-12 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176990 -- Kids Club 11-12 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176978 -- Kids Club 11-12 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176966 -- Kids Club 11-12 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176954 -- Kids Club 11-12 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176942 -- Kids Club 11-12 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176930 -- Kids Club 11-12 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176918 -- Kids Club 11-12 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176906 -- Kids Club 11-12 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176894 -- Kids Club 11-12 Month Old (January)
--10-11 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177027 -- Kids Club 10-11 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177015 -- Kids Club 10-11 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177003 -- Kids Club 10-11 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176991 -- Kids Club 10-11 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176979 -- Kids Club 10-11 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176967 -- Kids Club 10-11 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176955 -- Kids Club 10-11 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176943 -- Kids Club 10-11 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176931 -- Kids Club 10-11 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176919 -- Kids Club 10-11 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176907 -- Kids Club 10-11 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 11 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176895 -- Kids Club 10-11 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177027 -- Kids Club 10-11 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177015 -- Kids Club 10-11 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177003 -- Kids Club 10-11 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176991 -- Kids Club 10-11 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176979 -- Kids Club 10-11 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176967 -- Kids Club 10-11 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176955 -- Kids Club 10-11 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176943 -- Kids Club 10-11 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176931 -- Kids Club 10-11 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176919 -- Kids Club 10-11 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176907 -- Kids Club 10-11 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176895 -- Kids Club 10-11 Month Old (January)
--9-10 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177028 -- Kids Club 9-10 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177016 -- Kids Club 9-10 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177004 -- Kids Club 9-10 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176992 -- Kids Club 9-10 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176980 -- Kids Club 9-10 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176968 -- Kids Club 9-10 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176956 -- Kids Club 9-10 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176944 -- Kids Club 9-10 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176932 -- Kids Club 9-10 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176920 -- Kids Club 9-10 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176908 -- Kids Club 9-10 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 10 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176896 -- Kids Club 9-10 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177028 -- Kids Club 9-10 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177016 -- Kids Club 9-10 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177004 -- Kids Club 9-10 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176992 -- Kids Club 9-10 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176980 -- Kids Club 9-10 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176968 -- Kids Club 9-10 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176956 -- Kids Club 9-10 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176944 -- Kids Club 9-10 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176932 -- Kids Club 9-10 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176920 -- Kids Club 9-10 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176908 -- Kids Club 9-10 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176896 -- Kids Club 9-10 Month Old (January)
--8-9 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177029 -- Kids Club 8-9 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177017 -- Kids Club 8-9 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177005 -- Kids Club 8-9 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176993 -- Kids Club 8-9 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176981 -- Kids Club 8-9 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176969 -- Kids Club 8-9 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176957 -- Kids Club 8-9 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176945 -- Kids Club 8-9 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176933 -- Kids Club 8-9 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176921 -- Kids Club 8-9 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176909 -- Kids Club 8-9 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 9 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176897 -- Kids Club 8-9 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177029 -- Kids Club 8-9 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177017 -- Kids Club 8-9 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177005 -- Kids Club 8-9 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176993 -- Kids Club 8-9 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176981 -- Kids Club 8-9 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176969 -- Kids Club 8-9 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176957 -- Kids Club 8-9 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176945 -- Kids Club 8-9 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176933 -- Kids Club 8-9 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176921 -- Kids Club 8-9 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176909 -- Kids Club 8-9 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176897 -- Kids Club 8-9 Month Old (January)
--7-8 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177030 -- Kids Club 7-8 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177018 -- Kids Club 7-8 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177006 -- Kids Club 7-8 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176994 -- Kids Club 7-8 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176982 -- Kids Club 7-8 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176970 -- Kids Club 7-8 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176958 -- Kids Club 7-8 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176946 -- Kids Club 7-8 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176934 -- Kids Club 7-8 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176922 -- Kids Club 7-8 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176910 -- Kids Club 7-8 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 8 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176898 -- Kids Club 7-8 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177030 -- Kids Club 7-8 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177018 -- Kids Club 7-8 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177006 -- Kids Club 7-8 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176994 -- Kids Club 7-8 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176982 -- Kids Club 7-8 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176970 -- Kids Club 7-8 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176958 -- Kids Club 7-8 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176946 -- Kids Club 7-8 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176934 -- Kids Club 7-8 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176922 -- Kids Club 7-8 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176910 -- Kids Club 7-8 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176898 -- Kids Club 7-8 Month Old (January)
--6-7 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177031 -- Kids Club 6-7 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177019 -- Kids Club 6-7 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177007 -- Kids Club 6-7 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176995 -- Kids Club 6-7 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176983 -- Kids Club 6-7 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176971 -- Kids Club 6-7 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176959 -- Kids Club 6-7 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176947 -- Kids Club 6-7 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176935 -- Kids Club 6-7 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176923 -- Kids Club 6-7 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176911 -- Kids Club 6-7 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 7 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176899 -- Kids Club 6-7 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177031 -- Kids Club 6-7 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177019 -- Kids Club 6-7 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177007 -- Kids Club 6-7 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176995 -- Kids Club 6-7 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176983 -- Kids Club 6-7 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176971 -- Kids Club 6-7 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176959 -- Kids Club 6-7 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176947 -- Kids Club 6-7 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176935 -- Kids Club 6-7 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176923 -- Kids Club 6-7 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176911 -- Kids Club 6-7 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176899 -- Kids Club 6-7 Month Old (January)
--5-6 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177032 -- Kids Club 5-6 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177020 -- Kids Club 5-6 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177008 -- Kids Club 5-6 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176996 -- Kids Club 5-6 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176984 -- Kids Club 5-6 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176972 -- Kids Club 5-6 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176960 -- Kids Club 5-6 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176948 -- Kids Club 5-6 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176936 -- Kids Club 5-6 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176924 -- Kids Club 5-6 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176912 -- Kids Club 5-6 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 6 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176900 -- Kids Club 5-6 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177032 -- Kids Club 5-6 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177020 -- Kids Club 5-6 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177008 -- Kids Club 5-6 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176996 -- Kids Club 5-6 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176984 -- Kids Club 5-6 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176972 -- Kids Club 5-6 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176960 -- Kids Club 5-6 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176948 -- Kids Club 5-6 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176936 -- Kids Club 5-6 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176924 -- Kids Club 5-6 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176912 -- Kids Club 5-6 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176900 -- Kids Club 5-6 Month Old (January)
--4-5 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177033 -- Kids Club 4-5 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177021 -- Kids Club 4-5 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177009 -- Kids Club 4-5 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176997 -- Kids Club 4-5 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176985 -- Kids Club 4-5 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176973 -- Kids Club 4-5 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176961 -- Kids Club 4-5 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176949 -- Kids Club 4-5 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176937 -- Kids Club 4-5 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176925 -- Kids Club 4-5 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176913 -- Kids Club 4-5 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 5 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176901 -- Kids Club 4-5 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177033 -- Kids Club 4-5 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177021 -- Kids Club 4-5 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177009 -- Kids Club 4-5 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176997 -- Kids Club 4-5 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176985 -- Kids Club 4-5 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176973 -- Kids Club 4-5 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176961 -- Kids Club 4-5 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176949 -- Kids Club 4-5 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176937 -- Kids Club 4-5 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176925 -- Kids Club 4-5 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176913 -- Kids Club 4-5 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176901 -- Kids Club 4-5 Month Old (January)
--3-4 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177034 -- Kids Club 3-4 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177022 -- Kids Club 3-4 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177010 -- Kids Club 3-4 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176998 -- Kids Club 3-4 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176986 -- Kids Club 3-4 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176974 -- Kids Club 3-4 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176962 -- Kids Club 3-4 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176950 -- Kids Club 3-4 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176938 -- Kids Club 3-4 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176926 -- Kids Club 3-4 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176914 -- Kids Club 3-4 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 4 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176902 -- Kids Club 3-4 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177034 -- Kids Club 3-4 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177022 -- Kids Club 3-4 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177010 -- Kids Club 3-4 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176998 -- Kids Club 3-4 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176986 -- Kids Club 3-4 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176974 -- Kids Club 3-4 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176962 -- Kids Club 3-4 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176950 -- Kids Club 3-4 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176938 -- Kids Club 3-4 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176926 -- Kids Club 3-4 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176914 -- Kids Club 3-4 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176902 -- Kids Club 3-4 Month Old (January)
--2-3 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177035 -- Kids Club 2-3 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177023 -- Kids Club 2-3 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177011 -- Kids Club 2-3 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176999 -- Kids Club 2-3 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176987 -- Kids Club 2-3 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176975 -- Kids Club 2-3 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176963 -- Kids Club 2-3 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176951 -- Kids Club 2-3 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176939 -- Kids Club 2-3 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176927 -- Kids Club 2-3 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176915 -- Kids Club 2-3 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 3 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176903 -- Kids Club 2-3 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177035 -- Kids Club 2-3 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177023 -- Kids Club 2-3 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177011 -- Kids Club 2-3 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 176999 -- Kids Club 2-3 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176987 -- Kids Club 2-3 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176975 -- Kids Club 2-3 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176963 -- Kids Club 2-3 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176951 -- Kids Club 2-3 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176939 -- Kids Club 2-3 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176927 -- Kids Club 2-3 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176915 -- Kids Club 2-3 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176903 -- Kids Club 2-3 Month Old (January)
--1-2 months 
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177036 -- Kids Club 1-2 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177024 -- Kids Club 1-2 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177012 -- Kids Club 1-2 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 177000 -- Kids Club 1-2 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176988 -- Kids Club 1-2 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176976 -- Kids Club 1-2 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176964 -- Kids Club 1-2 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176952 -- Kids Club 1-2 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176940 -- Kids Club 1-2 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176928 -- Kids Club 1-2 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176916 -- Kids Club 1-2 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 2 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176904 -- Kids Club 1-2 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177036 -- Kids Club 1-2 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177024 -- Kids Club 1-2 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177012 -- Kids Club 1-2 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 177000 -- Kids Club 1-2 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176988 -- Kids Club 1-2 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176976 -- Kids Club 1-2 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176964 -- Kids Club 1-2 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176952 -- Kids Club 1-2 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176940 -- Kids Club 1-2 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176928 -- Kids Club 1-2 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176916 -- Kids Club 1-2 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())>=DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176904 -- Kids Club 1-2 Month Old (January)
--0-1 months
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 12 then 177037 -- Kids Club 0-1 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 11 then 177025 -- Kids Club 0-1 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 10 then 177013 -- Kids Club 0-1 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 9 then 177001 -- Kids Club 0-1 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 8 then 176989 -- Kids Club 0-1 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 7 then 176977 -- Kids Club 0-1 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 6 then 176965 -- Kids Club 0-1 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 5 then 176953 -- Kids Club 0-1 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 4 then 176941 -- Kids Club 0-1 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 3 then 176929 -- Kids Club 0-1 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 2 then 176917 -- Kids Club 0-1 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 1 and DatePart(dd,GetDate())<DatePart(DD,C.Date_of_Birth) and DatePart(MM,C.Date_of_Birth)= 1 then 176905 -- Kids Club 0-1 Month Old (January)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 12 then 177037 -- Kids Club 0-1 Month Old (December)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 11 then 177025 -- Kids Club 0-1 Month Old (November)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 10 then 177013 -- Kids Club 0-1 Month Old (October)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 9 then 177001 -- Kids Club 0-1 Month Old (September)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 8 then 176989 -- Kids Club 0-1 Month Old (August)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 7 then 176977 -- Kids Club 0-1 Month Old (July)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 6 then 176965 -- Kids Club 0-1 Month Old (June)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 5 then 176953 -- Kids Club 0-1 Month Old (May)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 4 then 176941 -- Kids Club 0-1 Month Old (April)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 3 then 176929 -- Kids Club 0-1 Month Old (March)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 2 then 176917 -- Kids Club 0-1 Month Old (February)
when DATEDIFF(m,C.Date_of_Birth,GetDate()) = 0 and DatePart(MM,C.Date_of_Birth)= 1 then 176905 -- Kids Club 0-1 Month Old (January)
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
where 
c.[Household_Position_ID] = 2 
and c.[Contact_Status_ID] = 1
and c.Household_id is not null
and (c.[HS_Graduation_Year] is null or c.[HS_Graduation_Year] > 2029)) as T1
where T1.group_id is not null;


--INSERT INTO [dbo].[Group_Participants] 
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
when c.[HS_Graduation_Year] = 2024 then  173934 --5th grade   Kids Club Grade 5
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
