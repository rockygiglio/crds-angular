USE [MinistryPlatform]
GO

ALTER PROCEDURE [dbo].[api_crds_GetReservedAndAvailableRoomsByLocation]
    @StartDate DateTime,
    @EndDate DateTime,
    @LocationId int = 1,
    @DomainID varchar(40) = NULL, -- make report happy
    @UserID varchar(40) = NULL,   -- make report happy
	@PageID Int = NULL,           -- make report happy
	@Time Int = NULL,
	@EndTime Int = NULL,
	@BuildingID Int = NULL,
	@DayofWeek INT = NULL,
	@RoomsID varchar(MAX) = '0'
AS
    IF (@Time IS NOT NULL AND @EndTime IS NOT NULL AND @BuildingID IS NOT NULL)   -- Call from report
        BEGIN
            DECLARE @Blocks Int, @Hours Int
            SET @Hours = DATEDIFF(hh,DateAdd(hh, @Time, @StartDate),DateAdd(hh, @EndTime, @EndDate))

            SET @Blocks = @Hours*2


            DECLARE @Hrs Table (HrStart DateTime,HrEnd DateTime)

                WHILE @Blocks > 0
                BEGIN
                INSERT INTO @Hrs VALUES (DateAdd(n,(@Blocks-1)*30,(DateAdd(hh, @Time, @StartDate))),DateAdd(n,@Blocks*30,(DateAdd(hh, @Time, @StartDate))))
                SET @Blocks = @Blocks - 1
                END

            SELECT @StartDate AS StartDate
            , Booked = ISNULL((
                SELECT SUM(Convert(Int, ER._Approved))
                 FROM Event_Rooms ER
                   INNER JOIN Events E ON E.Event_ID = ER.Event_ID
                 WHERE ER.Room_ID = List.Room_ID
                       AND ER.Cancelled = 0  -- fix for DE2733, consider cancelled events
                       AND (List.HRStart BETWEEN E.__Reservation_Start AND E.__Reservation_End OR List.HREnd BETWEEN E.__Reservation_Start AND E.__Reservation_End))
            ,-1)
            , List.HrStart
            , List.HrEnd
            , List.Room_Name
            , List.Room_ID
            , List.Room_Number
            , List.RowNum
            , B.Building_Name
            , B.Building_ID
            FROM (SELECT H.HrStart, H.HrEnd, R.Room_Name, Room_ID, Building_ID, Room_Number, Row_Number() OVER (PARTITION BY Room_ID ORDER BY HrStart) AS RowNum
                  FROM @Hrs H
                    CROSS JOIN Rooms R
                  WHERE R.Building_ID = @BuildingID
                    AND Bookable = 1) List
             INNER JOIN Buildings B ON List.Building_ID = B.Building_ID
             INNER JOIN dp_Domains Dom ON Dom.Domain_ID = B.Domain_ID
            WHERE Dom.Domain_GUID = @DomainID
            AND DATEPART(DW,List.HrStart) = ISNULL(@DayofWeek,DATEPART(DW,List.HrStart))
            AND (DATEPART(hh,List.HrStart) >= @Time
                AND CASE WHEN DATEPART(hh,List.HrEnd) = 0 THEN 24 ELSE DATEPART(hh,List.HrEnd) END <= CASE WHEN @EndTime = 0 THEN 24 ELSE @EndTime END)
            AND (List.Room_ID = @RoomsID OR @RoomsID = '0')
            ORDER BY List.RowNum

        END

    ELSE        -- call from scrummy bear app
        BEGIN
            WITH Reserved_Rooms (Room_ID, Approved, ExistingReservation, DisplayName, ReservationStart, ReservationEnd, ReservationId, ReservationEvent)
            AS
            (
                SELECT DISTINCT r.Room_ID,
                                er._Approved AS Approved,
                                ExistingReservation = 1,
                                c.Display_Name  AS DisplayName,
                                e.Event_Start_Date as ReservationStart,
                                e.Event_End_Date as ReservationEnd,
                                er.Event_Room_ID as ReservationId,
                                er.Event_ID as ReservationEvent
                FROM dbo.Events e
                    inner join dbo.Event_Rooms er on e.Event_ID = er.Event_ID
                    inner join dbo.Rooms r on er.Room_ID = r.Room_ID
                    inner join dbo.contacts c on e.primary_contact = c.contact_id
                WHERE ( (@StartDate BETWEEN DATEADD(minute, 1, e.Event_Start_Date)  AND  DATEADD(minute, -1, e.Event_End_Date) )
              OR (@EndDate BETWEEN DATEADD(minute, 1, e.Event_Start_Date) AND DATEADD(minute, -1, e.Event_End_Date)) or (e.Event_Start_Date >= @StartDate AND e.Event_Start_Date < @EndDate)
                    OR (e.Event_End_Date > @StartDate AND e.Event_End_Date <= @EndDate)  )
                    AND er.Cancelled = 0
                    AND e.Congregation_ID = (SELECT TOP 1 CONGREGATION_ID from CONGREGATIONS WHERE LOCATION_ID = @LocationId) AND (er._Approved is null OR er._Approved = 1)
            )
            SELECT DISTINCT r.Room_ID AS RoomId,
                            r.Room_Name AS RoomName,
                            r.Room_Number AS RoomNumber,
                            b.Building_ID AS BuildingId,
                            b.Building_Name AS BuildingName,
                            b.Location_ID AS LocationId,
                            r.Description,
                            ISNULL(r.Theater_Capacity, 0) AS TheaterCapacity,
                            ISNULL(r.Banquet_Capacity,0) AS BanquetCapacity,
                            rr.Approved,
                            rr.ExistingReservation,
                            rr.DisplayName,
                            rr.ReservationStart,
                            rr.ReservationEnd,
                            rr.ReservationId,
                            rr.ReservationEvent
            INTO #TEMPROOMS
            FROM dbo.Rooms r
                INNER JOIN dbo.Buildings b on b.Building_ID = r.Building_ID
                LEFT OUTER JOIN Reserved_Rooms rr on rr.Room_ID = r.Room_ID
            WHERE r.Bookable = 1
                    and b.Location_ID = @LocationId;

            --Available bookable rooms
            SELECT *, RoomStatus = null
            into #ALLROOMS
            from #TEMPROOMS
            where Approved is null and ExistingReservation is null;

            --Pending Rooms
            INSERT INTO #ALLROOMS SELECT *, RoomStatus = 0 FROM #TEMPROOMS WHERE Approved is null AND ExistingReservation = 1;

            --Already Approved Room Reservations
            INSERT INTO #ALLROOMS SELECT *, RoomStatus = 1 FROM #TEMPROOMS WHERE Approved = 1 AND ExistingReservation = 1;

            SELECT * FROM #ALLROOMS ORDER BY RoomId;
        END

GO