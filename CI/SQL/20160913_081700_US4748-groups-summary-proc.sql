USE MinistryPlatform
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ===============================================================
-- Author: John Cleaver <john.cleaver@ingagepartners.com>
-- Create date: 9/13/2016
-- Description:	Gets data for CRDS Groups Summary Report
-- (Based off of Ministry Platform report_Group_Summary proc)
-- ===============================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_report_Group_Summary]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[crds_report_Group_Summary] AS' 
END
GO

ALTER PROCEDURE [dbo].[crds_report_Group_Summary]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@GTID Int  = NULL -- Group type id
	,@RepDate DateTime -- Report date

AS
BEGIN


SELECT Groups = 1
, G.Group_Name 
, (SELECT TOP(1) Congregation_Name FROM Congregations c INNER JOIN Households h ON c.Congregation_ID
= h.Congregation_ID INNER JOIN contacts ct ON ct.Household_ID = h.Household_ID where ct.Contact_ID=g.Primary_Contact) AS Congregation_Name
, GT.Group_Type
, ISNULL(PG.Group_Name,'Com. Not Set') AS Community
, ISNULL(PPG.Group_Name, 'Area Not Set') AS Area
, CASE G.[Group_Is_Full] WHEN 0 THEN 'No' WHEN 1 THEN 'Yes' ELSE 'Unknown' END AS [Group_Is_Full]
, Adr.City
, RIGHT(G.Meeting_Time, 8) AS Meeting_Time
, ISNULL(RIGHT(MD.Meeting_Day, DataLength(MD.Meeting_Day)-3), 'Day Not Set') AS Meeting_Day
, Ministry_Name
, G.Start_Date
, ISNULL(P.Priority_Name, 'Not Assigned') AS Priority_Name
, GP.All_Members
, GP.Leaders
, GP.Servants
, GP.Participants
, (SELECT TOP(1) Congregation_Name FROM Congregations c INNER JOIN Households h ON c.Congregation_ID
= h.Congregation_ID INNER JOIN contacts ct ON ct.Household_ID = h.Household_ID where ct.Contact_ID=g.Primary_Contact) AS Leader_Site
, CASE G.[Available_Online] WHEN 0 THEN 'Private' WHEN 1 THEN 'Public' ELSE 'Unknown' END AS [Public]
, DATEADD(DAY, 1-(DATEPART( dw, G.Start_Date )) % 7 - 1, G.Start_Date) AS Start_Week_Date

FROM Groups G
 INNER JOIN Group_Types GT ON GT.Group_Type_ID = G.Group_Type_ID
 INNER JOIN dp_Domains Dom ON Dom.Domain_ID = G.Domain_ID
 LEFT OUTER JOIN Congregations C ON C.Congregation_ID = G.Congregation_ID
 LEFT OUTER JOIN Ministries M ON M.Ministry_ID = G.Ministry_ID
 LEFT OUTER JOIN Groups PG ON PG.Group_ID = G.Parent_Group
 LEFT OUTER JOIN Groups PPG ON PPG.Group_ID = PG.PArent_Group
 LEFT OUTER JOIN Addresses Adr ON Adr.Address_ID = G.Offsite_Meeting_Address
 LEFT OUTER JOIN Meeting_Days MD ON MD.Meeting_Day_ID = G.Meeting_Day_ID
 LEFT OUTER JOIN Priorities P ON P.Priority_ID = G.Priority_ID
 LEFT OUTER JOIN (SELECT Group_ID, Count(*) AS All_Members, SUM(CASE WHEN GR.Group_Role_Type_ID = 1 THEN 1 ELSE 0 END) AS Leaders, SUM(CASE WHEN GR.Group_Role_Type_ID = 3 THEN 1 ELSE 0 END) AS Servants, SUM(CASE WHEN GR.Group_Role_Type_ID = 2 THEN 1 ELSE 0 END) AS Participants FROM Group_Participants GP INNER JOIN Group_Roles GR ON GR.Group_Role_ID = GP.Group_Role_ID WHERE ((@RepDate BETWEEN GP.Start_Date AND GP.End_Date) OR (@RepDate >= GP.Start_Date AND GP.End_Date IS NULL)) Group BY Group_ID) gp on gp.Group_ID = G.Group_ID
WHERE ((@RepDate BETWEEN G.Start_Date AND G.End_Date) OR (@RepDate >= G.Start_Date AND G.End_Date IS NULL))--Show groups AS OF @RepDate
AND G.Group_Type_ID = ISNULL(@GTID,G.Group_Type_ID)
AND Dom.Domain_GUID = @DomainID
AND g.Available_Online IS NOT NULL

END

GO


