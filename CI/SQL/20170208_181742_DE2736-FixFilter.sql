USE [MinistryPlatform]
GO
/****** Object:  StoredProcedure [dbo].[api_crds_GetReservedAndAvailableRoomsByLocation]    Script Date: 2/14/2017 4:25:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      Ken Baum
-- Create date: 2016-12-07
-- Description:  Returns all bookable rooms by 
--               location id, plus whether they 
--               are currently reserved for a 
--               particular time or not
-- =============================================
ALTER PROCEDURE [dbo].[api_crds_GetReservedAndAvailableRoomsByLocation]

    @StartDate DateTime,
    @EndDate DateTime,
    @LocationId int = 1
AS
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
            and b.Location_ID = @LocationId

    --Available bookable rooms
    SELECT *, RoomStatus = null 
    into #ALLROOMS 
    from #TEMPROOMS 
    where Approved is null and ExistingReservation is null;

    --Pending Rooms
    INSERT INTO #ALLROOMS SELECT *, RoomStatus = 0 FROM #TEMPROOMS WHERE Approved is null AND ExistingReservation = 1;

    --Already Approved Room Reservations
    INSERT INTO #ALLROOMS SELECT *, RoomStatus = 1 FROM #TEMPROOMS WHERE Approved = 1 AND ExistingReservation = 1

    SELECT * FROM #ALLROOMS ORDER BY RoomId;

END