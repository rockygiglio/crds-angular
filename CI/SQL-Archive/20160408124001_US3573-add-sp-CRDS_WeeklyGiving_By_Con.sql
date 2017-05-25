
USE MinistryPlatform
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
  (SELECT *
   FROM sys.objects
   WHERE object_id = object_id(N'[dbo].[report_CRDS_Weekly_Giving_By_Congregation]')
     AND TYPE IN (N'P',
                   N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Weekly_Giving_By_Congregation] AS';

END 
GO
ALTER PROCEDURE [dbo].[report_CRDS_Weekly_Giving_By_Congregation] 
     @startdate DATETIME,
     @enddate DATETIME,
     @congregationid AS VARCHAR(MAX),
	 @programid AS VARCHAR(MAX)
AS 



BEGIN
SET nocount ON;


SET DATEFIRST 1; --Sets the First Day of the week to Monday
SET @startdate = DATEADD(dd, DATEDIFF(dd, 0, @startdate),0);
SET @enddate   =  DATEADD(mi,1439, (DATEADD(dd, DATEDIFF(dd, 0, @enddate),0)));

DECLARE @congregations TABLE(conname varchar(50))
INSERT INTO @congregations
  SELECT congregation_name from Congregations
    WHERE congregation_id in (SELECT Item FROM dbo.dp_Split(@congregationid, ','))

IF OBJECT_ID('tempdb..#TOTALS') IS NOT NULL   
	DROP TABLE #TOTALS
IF OBJECT_ID('tempdb..#TMPTOTS') IS NOT NULL   
	DROP TABLE #TMPTOTS

CREATE TABLE #TMPTOTS
(
	congregation_name			varchar(50),
	Total						money,
	WeekEnding					datetime
)
CREATE TABLE #TOTALS
(
	congregation_name			varchar(50),
	Total						money,
	WeekEnding					datetime
)

INSERT INTO #TOTALS
	SELECT DISTINCT c.conname , 0,  DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE)) [WeekEnding]
	FROM @congregations c
	CROSS JOIN donations don
	WHERE don.donation_date BETWEEN @startdate AND @enddate
	ORDER BY WeekEnding, conname

INSERT INTO #TMPTOTS
	SELECT 
	  con.congregation_name,
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
	GROUP BY con.congregation_name, DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE))
	ORDER BY DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE)), con.congregation_name desc
	-- need to deal with soft credits here
	

UPDATE t
SET t.Total = tmp.Total
FROM #TOTALS t
JOIN #TMPTOTS tmp ON tmp.congregation_name = t.congregation_name AND tmp.WeekEnding = t.WeekEnding

SELECT * FROM #TOTALS
ORDER BY WeekEnding, congregation_name


DROP TABLE #TOTALS
DROP TABLE #TMPTOTS
END