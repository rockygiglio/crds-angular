USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Weekly_Giving_By_Congregation]    Script Date: 8/10/2016 7:49:31 AM ******/
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
	 @programid AS VARCHAR(MAX),
	 @donationstatusid as varchar(MAX)
AS 



BEGIN
SET nocount ON;



SET DATEFIRST 1;
SET @startdate = DATEADD(dd, DATEDIFF(dd, 0, @startdate),0);
SET @enddate   =  DATEADD(ms,86399998, (DATEADD(dd, DATEDIFF(dd, 0, @enddate),0)))

DECLARE @congregations TABLE(conname varchar(50))
INSERT INTO @congregations
  SELECT congregation_name from Congregations
    WHERE congregation_id in (SELECT Item FROM dbo.dp_Split(@congregationid, ','))


IF OBJECT_ID('tempdb..#TMPTOTS') IS NOT NULL   
	DROP TABLE #TMPTOTS

CREATE TABLE #TMPTOTS
(
	congregation_name			varchar(50),
	Total						money,
	WeekEnding					datetime,
	soft_credit_donor				int
)


INSERT INTO #TMPTOTS (congregation_name,Total,WeekEnding)
	select distinct c.conname , 0,  DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE)) [WeekEnding]
	from @congregations c
	cross join donations don
	WHERE don.donation_date BETWEEN @startdate AND @enddate
	order by WeekEnding, conname;


INSERT INTO #TMPTOTS
	SELECT 
	  CASE
	   WHEN (dd.soft_credit_donor IS NULL) THEN COALESCE(con.congregation_name,'Not Specified')  
	   ELSE  'Not Specified'
	  END AS congregation_name,
	  dd.amount AS Total,
	  DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE)) [WeekEnding],
	  dd.soft_credit_donor
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
	AND don.donation_status_id IN (SELECT Item FROM dbo.dp_Split(@donationstatusid, ','));
	
	UPDATE g
	 SET g.congregation_name = con.congregation_name
	FROM #TMPTOTS g
	JOIN donors d on d.donor_id = g.soft_credit_donor
	JOIN contacts c on c.contact_id = d.contact_id
	JOIN households h on h.household_id = c.household_id
	JOIN congregations con on con.congregation_id = h.congregation_id
	WHERE g.soft_credit_donor is not null;
	

select congregation_name,
		SUM(Total) as Total,
		WeekEnding
		from #TMPTOTS 
		GROUP BY congregation_name,WeekEnding
		ORDER BY WeekEnding, congregation_name;


DROP TABLE #TMPTOTS
END
GO


