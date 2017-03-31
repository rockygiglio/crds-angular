USE MinistryPlatform
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [dbo].[report_CRDS_Event_Listing]
    @DomainID VARCHAR(40)
   ,@UserID VARCHAR(40)
   ,@PageID INT
   ,@FromDate DATETIME
   ,@ToDate DATETIME
   ,@CongregationID INT = NULL
   ,@EventTypes nvarchar(MAX) = '0'
   ,@ShowCancelled BIT = 1
   ,@ShowNoRoomsSet BIT = 1
   ,@ListTime INT = 1
   ,@ShowBldgRm INT = 1
   ,@ExcludeRejectedRoomReservation BIT = 1

AS
BEGIN

      SELECT  E.Event_ID
           ,Event_Title
           ,E.[Description]
           ,Minutes_for_Setup
           ,CASE WHEN @FromDate >= E.Event_Start_Date THEN @FromDate ELSE E.Event_Start_Date END AS Event_Date
           ,Event_Start_Date
           ,Event_End_Date
           ,[Minutes_for_Cleanup]
           ,Participants_Expected
           ,C.Display_Name AS Contact_Person
           ,C.Email_Address AS Contact_Email
           ,C.Company_Phone AS Contact_Phone
           ,ISNULL(Congregation_Name, 'Congregation Not Set') AS Congregation_Name
           ,E.Congregation_ID
           ,ISNULL(Room_1, 'No Rooms Set') AS Room_1
           ,Room_2
           ,Room_3
           ,Room_4
           ,Room_5
           ,Room_6
           ,Rooms = ISNULL(Room_1, 'No Rooms Set') + ISNULL(' | ' + Room_2, '') + ISNULL(' | ' + Room_3, '') + ISNULL(' | ' + Room_4, '')
            + ISNULL(' | ' + Room_5, '') + ISNULL(' | ' + Room_6, '')
           ,ISNULL(Rooms, 0) AS Number_of_Rooms
           ,STUFF((SELECT   ' | ' + bs.Building_Name + '/' + rs.Room_Name + ISNULL(' RM' + rs.Room_Number, '')
                   FROM     Event_Rooms ers
                            INNER JOIN Rooms rs
                                ON ers.Event_ID = E.Event_ID
                                AND rs.Room_ID = ers.Room_ID
                            INNER JOIN Buildings bs
                                ON rs.Building_ID = bs.Building_ID
                   GROUP BY bs.Building_Name
							,rs.Room_Name
							,rs.Room_Number
                   ORDER BY bs.Building_Name
                           ,rs.Room_Name
						   ,ISNULL(rs.Room_Number, '')
                   FOR
                   XML PATH('')), 1, 1, '') AS Full_Room_List
           ,RoomNumbers = STUFF((SELECT   ', ' + ISNULL(rs.Room_Number, 'No Rooms Set')
								   FROM     Event_Rooms ers
											INNER JOIN Rooms rs
												ON ers.Event_ID = E.Event_ID
												AND rs.Room_ID = ers.Room_ID
								   GROUP BY rs.Room_Number
								   ORDER BY ISNULL(rs.Room_Number, '')
						   FOR
						   XML PATH('')), 1, 1, '')
           ,M.Ministry_Name
           ,Prog.[Program_Name]
           ,E.Cancelled
           ,E.__Reservation_Start AS ReservationStart
           ,E.__Reservation_End AS ReservationEnd
    INTO #ReportData
    FROM    dbo.Events E
            INNER JOIN dbo.dp_Domains Dom
                ON Dom.Domain_ID = E.Domain_ID
            INNER JOIN dbo.Contacts C
                ON C.Contact_ID = E.Primary_Contact
            INNER JOIN dbo.Programs Prog
                ON Prog.Program_ID = E.Program_ID
            INNER JOIN dbo.Ministries M
                ON M.Ministry_ID = Prog.Ministry_ID
            LEFT OUTER JOIN dbo.Congregations Cong
                ON Cong.Congregation_ID = E.Congregation_ID
            LEFT OUTER JOIN (SELECT Event_ID, _Approved
                                   ,Room_1 = MAX(CASE WHEN Position = 1 THEN Room_Name
                                                 END)
                                   ,Room_2 = MAX(CASE WHEN Position = 2 THEN Room_Name
                                                 END)
                                   ,Room_3 = MAX(CASE WHEN Position = 3 THEN Room_Name
                                                 END)
                                   ,Room_4 = MAX(CASE WHEN Position = 4 THEN Room_Name
                                                 END)
                                   ,Room_5 = MAX(CASE WHEN Position = 5 THEN Room_Name
                                                 END)
                                   ,Room_6 = MAX(CASE WHEN Position = 6 THEN Room_Name
                                                 END)
                                   ,Rooms = COUNT(*)
                             FROM   (SELECT Event_ID, _Approved
                                           ,Position = row_number() over (partition by Event_ID ORDER BY Maximum_Capacity DESC)
                                           ,R.Room_Name + ' RM' + ISNULL(R.Room_Number, '') AS Room_Name
                                           ,B.Building_Name + '/' + R.Room_Name + ' RM' + ISNULL(R.Room_Number, '') AS Room_Name_Building
                                     FROM   Event_Rooms ER
                                            INNER JOIN Rooms R
                                                ON R.Room_ID = ER.Room_ID
                                            INNER JOIN Buildings B
                                                ON B.Building_ID = R.Building_ID) ER1
                             GROUP BY ER1.Event_ID, ER1._Approved) ER2
                ON ER2.Event_ID = E.Event_ID
    WHERE   Dom.Domain_GUID = @DomainID
            AND ISNULL(E.Congregation_ID, 0) = ISNULL(@CongregationID, ISNULL(E.Congregation_ID, 0))
            AND (
					(E.Event_Start_Date >= @FromDate AND E.Event_Start_Date < @ToDate + 1)
					OR @FromDate between E.Event_Start_Date AND E.Event_End_Date
				)
            AND (
					ISNULL(@EventTypes,'0') = '0'
					OR E.Event_Type_ID is NULL
					OR E.Event_Type_ID in (SELECT Item FROM dp_Split(@EventTypes, ','))
				)
          AND (
            @ExcludeRejectedRoomReservation = 0 OR (@ExcludeRejectedRoomReservation = 1 AND ER2._Approved = 1)
        )
	ORDER BY Congregation_Name
           ,Event_Start_Date
           ,Event_Title

--Add any dates between date ranges to print the individual date


    SELECT DATEDIFF("dd"
                    ,CONVERT(DATE,CASE WHEN CONVERT(DATE,@FromDate,101)>= CONVERT(DATE,Event_Start_Date,101) THEN CONVERT(DATE,@FromDate,101) ELSE CONVERT(DATE,Event_Start_Date,101) END,101)
                    ,CONVERT(DATE,CASE WHEN CONVERT(DATE,Event_End_Date,101) <= CONVERT(DATE,@ToDate,101) THEN CONVERT(DATE,Event_End_Date,101) ELSE CONVERT(DATE,@ToDate,101) END,101)
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
    INTO #RoomsAddDate
    FROM #ReportData
    WHERE CONVERT(DATE,Event_Start_Date,101) <> CONVERT(DATE,Event_End_Date,101)

    DECLARE @i INT = 1
            ,@NumDays INT = (SELECT MAX(NumDaysToAdd) FROM #RoomsAddDate)+1

    WHILE @i < @NumDays
    BEGIN
    INSERT INTO #ReportData
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
            SELECT Event_ID
                ,Event_Title
                ,[Description]
                ,Minutes_for_Setup
                ,DATEADD(DAY,@i,Event_Date) AS Event_Date
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
            FROM #RoomsAddDate
            WHERE NumDaysToAdd >= @i

    SET @i= @i + 1
    END

    SELECT Event_ID
                ,Event_Title
                ,[Description]
                ,Minutes_for_Setup
                ,CONVERT(datetime,CONVERT(VARCHAR(10),Event_Date,101)) AS Event_Date
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
    FROM #ReportData
    WHERE Cancelled IN (@ShowCancelled,0)
        AND ((@ShowNoRoomsSet = 0 AND @ShowBldgRm = 3 AND ISNULL(RoomNumbers,'No Rooms Set') NOT LIKE '%No Rooms Set')
                OR (@ShowNoRoomsSet = 0 AND @ShowBldgRm = 2 AND ISNULL(Rooms,'No Rooms Set') NOT LIKE '%No Rooms Set')
                OR (@ShowNoRoomsSet = 0 AND @ShowBldgRm = 1 AND ISNULL(Full_Room_List,'No Rooms Set') NOT LIKE '%No Rooms Set')
                OR @ShowNoRoomsSet = 1
            )
    ORDER BY Congregation_Name
            ,Event_Date
            ,CASE WHEN @ListTime = 1 THEN CONVERT(TIME,Event_Start_Date) ELSE CONVERT(TIME,ReservationStart) END
            ,CASE WHEN @ListTime = 1 THEN CONVERT(TIME,Event_End_Date) ELSE CONVERT(TIME,ReservationEnd) END
            ,Event_Title

    --CLEAN UP
    DROP TABLE #ReportData
    DROP TABLE #RoomsAddDate

END