USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_FacilityEventChangeTracking]    Script Date: 6/2/2016 8:31:24 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS
 (SELECT *
  FROM sys.objects
  WHERE object_id = object_id(N'[dbo].[report_CRDS_FacilityEventChangeTracking]')
    AND TYPE IN (N'P',
                  N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_FacilityEventChangeTracking] AS';

END 
GO

ALTER PROCEDURE [dbo].[report_CRDS_FacilityEventChangeTracking]
-- Add the parameters for the stored procedure here
   --@DomainID             VARCHAR(40) = '0FDE7F32-37E3-4E0B-B020-622E0EBD6BF0'  -- = Domain 1
	--,@UserID varchar(40)
	--,@PageID Int
	@startDate datetime
	,@endDate datetime,
	@congregationid AS VARCHAR(MAX)

AS
     BEGIN
         SET NOCOUNT ON;
		 
		 SELECT top 1000 E.Event_Title,rs.Room_Name, C.Display_Name as [Event Primary Contact], CO.Congregation_Name, AD.Field_Name as [Field Changed], AD.Previous_Value as[Previous Date/Time], AD.New_Value as[New Date/Time], AL.Date_Time as [Date/Time Change Was Made], AL.User_Name as [Change Made By]
			FROM dp_Audit_Log  AL
				INNER JOIN dp_Audit_Detail AD on AD.Audit_Item_ID = AL.AUdit_Item_ID
				INNER JOIN Events E on E.Event_ID = AL.Record_ID
				INNER JOIN Contacts C on C.Contact_ID = E.Primary_Contact
				INNER JOIN Congregations CO on CO.Congregation_ID = E.Congregation_ID
				LEFT JOIN Event_Rooms R on R.Event_ID = E.Event_ID
				left join Rooms rs on rs.Room_ID = r.Room_ID
				LEFT JOIN Event_Equipment EQ on EQ.Event_ID = E.Event_ID
				WHERE Audit_Description = 'Updated' AND Table_Name = 'Events' AND (Field_Name = 'Event_Start_Date' or Field_Name = 'Event_End_Date')
					AND CO.congregation_id IN (SELECT Item FROM dbo.dp_Split(@congregationid, ','))
					AND (R._Approved = 1 or EQ._Approved = 1)
					and R.Cancelled =0
					AND (Date_Time >= @startDate AND Date_Time <= @endDate +1)
				group by E.Event_Title,rs.Room_Name,  C.Display_Name, CO.Congregation_Name, AD.Field_Name, AD.Previous_Value, AD.New_Value, AL.Date_Time, AL.User_Name
				order by e.event_title, rs.Room_Name;

 
	END;








GO


