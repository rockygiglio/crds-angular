
USE MinistryPlatform
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
  (SELECT *
   FROM sys.objects
   WHERE object_id = object_id(N'[dbo].[report_CRDS_Weekly_Giving_By_Program]')
     AND TYPE IN (N'P',
                   N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Weekly_Giving_By_Program] AS';

END 
GO
ALTER PROCEDURE [dbo].[report_CRDS_Weekly_Giving_By_Program] 
     @startdate DATETIME,
     @enddate DATETIME,
     @congregationid AS VARCHAR(MAX),
	 @programid AS VARCHAR(MAX)
AS 


 --   declare @startdate DATETIME  = '2016-1-1'
 --   declare @enddate DATETIME = '2016-1-31'
 --   declare @congregationid AS VARCHAR(MAX) = '1,2,5,6,7,8,11,15,16'
	--declare @programid AS VARCHAR(MAX) = '3,146'

BEGIN
SET nocount ON;


SET DATEFIRST 1; --Sets the First Day of the week to Monday
SET @startdate = DATEADD(dd, DATEDIFF(dd, 0, @startdate),0);
SET @enddate   =  DATEADD(mi,1439, (DATEADD(dd, DATEDIFF(dd, 0, @enddate),0)));

declare @congregations table(conname varchar(50))
INSERT INTO @congregations
  SELECT congregation_name from Congregations
    WHERE congregation_id in (SELECT Item FROM dbo.dp_Split(@congregationid, ','))

declare @programs table(progname varchar(50))
INSERT INTO @programs
  SELECT program_name from Programs
    WHERE program_id in (SELECT Item FROM dbo.dp_Split(@programid, ','))

IF OBJECT_ID('tempdb..#TOTALS') IS NOT NULL   
	DROP TABLE #TOTALS
IF OBJECT_ID('tempdb..#TMPTOTS') IS NOT NULL   
	DROP TABLE #TMPTOTS

CREATE TABLE #TMPTOTS
(
	program_name			varchar(50),
	Total						money,
	WeekEnding					datetime
)
CREATE TABLE #TOTALS
(
	program_name			varchar(50),
	Total						money,
	WeekEnding					datetime
)

INSERT INTO #TOTALS
	select distinct c.progname , 0,  DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE)) [WeekEnding]
	from @programs c
	cross join donations don
	WHERE don.donation_date BETWEEN @startdate AND @enddate
	order by WeekEnding, progname



INSERT INTO #TMPTOTS
	SELECT 
	  p.program_name,
	  SUM(dd.amount) AS Total,
	  --DATEPART(ww,don.donation_date) WeekNumber,
	  DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE)) [WeekEnding]
	FROM donations don 
	JOIN donors d ON d.donor_id = don.donor_id
	JOIN contacts c ON c.contact_id = d.contact_id
	JOIN donation_distributions dd on dd.donation_id = don.donation_id
	JOIN programs p on p.program_id = dd.program_id
	LEFT JOIN households h ON h.household_id = c.household_id
	LEFT JOIN congregations con ON con.congregation_id = dd.congregation_id
	WHERE don.donation_date BETWEEN @startdate AND @enddate
	AND dd.program_id IN (SELECT Item FROM dbo.dp_Split(@programid, ','))
	AND dd.congregation_id IN (SELECT Item FROM dbo.dp_Split(@congregationid, ','))
	GROUP BY p.program_name, DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE))
	--ORDER BY DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE)), con.congregation_name desc
	-- need to deal with soft credits here
	

UPDATE t
SET t.Total = tmp.Total
FROM #TOTALS t
JOIN #TMPTOTS tmp ON tmp.program_name = t.program_name AND tmp.WeekEnding = t.WeekEnding

SELECT * FROM #TOTALS
ORDER BY WeekEnding, program_name

--UNION 
--SELECT 'All',
--	SUM(don.donation_amount) AS Total,
--	DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE)) [WeekEnding]
--FROM donations don 
--WHERE don.donation_date BETWEEN @startdate AND @enddate
--GROUP BY DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE))
--ORDER BY DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE))

DROP TABLE #TOTALS
DROP TABLE #TMPTOTS
END