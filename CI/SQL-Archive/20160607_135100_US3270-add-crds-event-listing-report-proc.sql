USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Event_Listing]    Script Date: 6/7/2016 2:07:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_Event_Listing]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Event_Listing] AS' 
END
GO

ALTER procedure [dbo].[report_CRDS_Event_Listing]
    @DomainID varchar(40)
   ,@UserID varchar(40)
   ,@PageID int
   ,@FromDate datetime
   ,@ToDate datetime
   ,@CongregationID int = null
   ,@EventTypes nvarchar(max) = '0'
   ,@ShowCancelled bit = 1
   ,@ShowNoRoomsSet bit = 1
   ,@ListTime int = 1
   ,@ShowBldgRm int = 1
   
as
begin


    select  E.Event_ID
           ,Event_Title
           ,E.[Description]
           ,Minutes_for_Setup
           ,case when @FromDate >= E.Event_Start_Date then @FromDate else E.Event_Start_Date end as Event_Date
           ,Event_Start_Date
           ,Event_End_Date
           ,[Minutes_for_Cleanup]
           ,Participants_Expected
           ,C.Display_Name as Contact_Person
           ,C.Email_Address as Contact_Email
           ,C.Company_Phone as Contact_Phone
           ,isnull(Congregation_Name, 'Congregation Not Set') as Congregation_Name
           ,E.Congregation_ID
           ,isnull(Room_1, 'No Rooms Set') as Room_1
           ,Room_2
           ,Room_3
           ,Room_4
           ,Room_5
           ,Room_6
           ,Rooms = isnull(Room_1, 'No Rooms Set') + isnull(' | ' + Room_2, '') + isnull(' | ' + Room_3, '') + isnull(' | ' + Room_4, '')
            + isnull(' | ' + Room_5, '') + isnull(' | ' + Room_6, '')
           ,isnull(Rooms, 0) as Number_of_Rooms
           ,stuff((select   ' | ' + bs.Building_Name + '/' + rs.Room_Name + isnull(' RM' + rs.Room_Number, '')
                   from     Event_Rooms ers
                            inner join Rooms rs
                                on ers.Event_ID = E.Event_ID
                                and rs.Room_ID = ers.Room_ID
                            inner join Buildings bs
                                on rs.Building_ID = bs.Building_ID
                   group by bs.Building_Name
							,rs.Room_Name
							,rs.Room_Number
                   order by bs.Building_Name
                           ,rs.Room_Name
						   ,isnull(rs.Room_Number, '')
                  for
                   xml path('')), 1, 1, '') as Full_Room_List
           ,RoomNumbers = stuff((select   ', ' + isnull(rs.Room_Number, 'No Rooms Set')
								   from     Event_Rooms ers
											inner join Rooms rs
												on ers.Event_ID = E.Event_ID
												and rs.Room_ID = ers.Room_ID
								   group by rs.Room_Number
								   order by isnull(rs.Room_Number, '')
						  for
						   xml path('')), 1, 1, '')
           ,M.Ministry_Name
           ,Prog.[Program_Name]
           ,E.Cancelled
           ,E.__Reservation_Start AS ReservationStart
           ,E.__Reservation_End AS ReservationEnd
    into #ReportData
    from    dbo.Events E
            inner join dbo.dp_Domains Dom
                on Dom.Domain_ID = E.Domain_ID
            inner join dbo.Contacts C
                on C.Contact_ID = E.Primary_Contact
            inner join dbo.Programs Prog
                on Prog.Program_ID = E.Program_ID
            inner join dbo.Ministries M
                on M.Ministry_ID = Prog.Ministry_ID
            left outer join dbo.Congregations Cong
                on Cong.Congregation_ID = E.Congregation_ID
            left outer join (select Event_ID
                                   ,Room_1 = max(case when Position = 1 then Room_Name
                                                 end)
                                   ,Room_2 = max(case when Position = 2 then Room_Name
                                                 end)
                                   ,Room_3 = max(case when Position = 3 then Room_Name
                                                 end)
                                   ,Room_4 = max(case when Position = 4 then Room_Name
                                                 end)
                                   ,Room_5 = max(case when Position = 5 then Room_Name
                                                 end)
                                   ,Room_6 = max(case when Position = 6 then Room_Name
                                                 end)
                                   ,Rooms = count(*)
                             from   (select Event_ID
                                           ,Position = row_number() over (partition by Event_ID order by Maximum_Capacity desc)
                                           ,R.Room_Name + ' RM' + isnull(R.Room_Number, '') as Room_Name
                                           ,B.Building_Name + '/' + R.Room_Name + ' RM' + isnull(R.Room_Number, '') as Room_Name_Building
                                     from   Event_Rooms ER
                                            inner join Rooms R
                                                on R.Room_ID = ER.Room_ID
                                            inner join Buildings B
                                                on B.Building_ID = R.Building_ID) ER1
                             group by ER1.Event_ID) ER2
                on ER2.Event_ID = E.Event_ID
    where   Dom.Domain_GUID = @DomainID
            and isnull(E.Congregation_ID, 0) = isnull(@CongregationID, isnull(E.Congregation_ID, 0))
            and (
					(E.Event_Start_Date >= @FromDate and E.Event_Start_Date < @ToDate + 1)
					or @FromDate between E.Event_Start_Date and E.Event_End_Date
				)
            and (
					ISNULL(@EventTypes,'0') = '0' 
					or E.Event_Type_ID is null
					or E.Event_Type_ID in (select Item from dp_Split(@EventTypes, ','))
				)
	order by Congregation_Name
           ,Event_Start_Date
           ,Event_Title

--Add any dates between date ranges to print the individual date


select DATEDIFF("dd"
				,convert(date,case when convert(date,@FromDate,101)>= convert(date,Event_Start_Date,101) then convert(date,@FromDate,101) else convert(date,Event_Start_Date,101) end,101)
				,convert(date,case when convert(date,Event_End_Date,101) <= convert(date,@ToDate,101) then convert(date,Event_End_Date,101) else convert(date,@ToDate,101)end,101)
				) NumDaysToAdd
			,Event_ID
			,Event_Title
			,[Description]
			,Minutes_for_Setup
			,Event_Date
			,Event_Start_Date
			,Event_End_Date
			,Minutes_for_Cleanup
			,Participants_Expected
			,Contact_Person
			,Contact_Email
			,Contact_Phone
			,Congregation_Name
			,Congregation_ID
			,Room_1
			,Room_2
			,Room_3
			,Room_4
			,Room_5
			,Room_6
			,Rooms
			,Number_of_Rooms
			,Full_Room_List
			,RoomNumbers
			,Ministry_Name
			,[Program_Name]
			,Cancelled
			,ReservationStart
			,ReservationEnd
into #RoomsAddDate
from #ReportData 
where convert(date,Event_Start_Date,101) <> convert(date,Event_End_Date,101)

declare @i int = 1
		,@NumDays int = (select MAX(NumDaysToAdd) from #RoomsAddDate)+1

while @i < @NumDays
begin
 insert into #ReportData
			(Event_ID
			,Event_Title
			,[Description]
			,Minutes_for_Setup
			,Event_Date
			,Event_Start_Date
			,Event_End_Date
			,Minutes_for_Cleanup
			,Participants_Expected
			,Contact_Person
			,Contact_Email
			,Contact_Phone
			,Congregation_Name
			,Congregation_ID
			,Room_1
			,Room_2
			,Room_3
			,Room_4
			,Room_5
			,Room_6
			,Rooms
			,Number_of_Rooms
			,Full_Room_List
			,RoomNumbers
			,Ministry_Name
			,[Program_Name]
			,Cancelled
			,ReservationStart
			,ReservationEnd
			)
		select Event_ID
			,Event_Title
			,[Description]
			,Minutes_for_Setup
			,DATEADD(DAY,@i,Event_Date)as Event_Date
			,Event_Start_Date
			,Event_End_Date
			,Minutes_for_Cleanup
			,Participants_Expected
			,Contact_Person
			,Contact_Email
			,Contact_Phone
			,Congregation_Name
			,Congregation_ID
			,Room_1
			,Room_2
			,Room_3
			,Room_4
			,Room_5
			,Room_6
			,Rooms
			,Number_of_Rooms
			,Full_Room_List
			,RoomNumbers
			,Ministry_Name
			,[Program_Name]
			,Cancelled
			,ReservationStart
			,ReservationEnd
		from #RoomsAddDate
		where NumDaysToAdd >= @i

 set @i= @i + 1
end	

select Event_ID
			,Event_Title
			,[Description]
			,Minutes_for_Setup
			,convert(datetime,convert(varchar(10),Event_Date,101)) As Event_Date
			,Event_Start_Date
			,Event_End_Date
			,Minutes_for_Cleanup
			,Participants_Expected
			,Contact_Person
			,Contact_Email
			,Contact_Phone
			,Congregation_Name
			,Congregation_ID
			,Room_1
			,Room_2
			,Room_3
			,Room_4
			,Room_5
			,Room_6
			,Rooms
			,Number_of_Rooms
			,Full_Room_List
			,RoomNumbers
			,Ministry_Name
			,[Program_Name]
			,Cancelled
			,ReservationStart
			,ReservationEnd 
from #ReportData
where Cancelled in (@ShowCancelled,0)
	and ((@ShowNoRoomsSet = 0 and @ShowBldgRm = 3 and ISNULL(RoomNumbers,'No Rooms Set') not like '%No Rooms Set')
			OR (@ShowNoRoomsSet = 0 and @ShowBldgRm = 2 and ISNULL(Rooms,'No Rooms Set') not like '%No Rooms Set')
			OR (@ShowNoRoomsSet = 0 and @ShowBldgRm = 1 and ISNULL(Full_Room_List,'No Rooms Set') not like '%No Rooms Set')
			OR @ShowNoRoomsSet = 1
		)
order by Congregation_Name
		,Event_Date
		,case when @ListTime = 1 then convert(time,Event_Start_Date) else convert(time,ReservationStart) end
		,case when @ListTime = 1 then convert(time,Event_End_Date) else convert(time,ReservationEnd) end
		,Event_Title	

--CLEAN UP
drop table #ReportData
drop table #RoomsAddDate

end

GO



