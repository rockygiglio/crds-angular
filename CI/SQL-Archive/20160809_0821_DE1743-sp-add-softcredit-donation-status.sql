USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Weekly_Giving_By_Program]    Script Date: 8/9/2016 8:17:05 AM ******/
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
	 @programid AS VARCHAR(MAX),
	 @donationstatusid as varchar(MAX)
AS 


BEGIN
SET nocount ON;


--Per Finance - change is needed to capture millisecond data on donations
SET DATEFIRST 1;
SET @startdate = DATEADD(dd, DATEDIFF(dd, 0, @startdate),0);
SET @enddate   =  DATEADD(ms,86399998, (DATEADD(dd, DATEDIFF(dd, 0, @enddate),0)))


declare @programs table(progname varchar(50))
INSERT INTO @programs
  SELECT program_name from Programs
    WHERE program_id in (SELECT Item FROM dbo.dp_Split(@programid, ','))

IF OBJECT_ID('tempdb..#TMPTOTS') IS NOT NULL   
	DROP TABLE #TMPTOTS


CREATE TABLE #TMPTOTS
(
	program_name			varchar(50),
	Total						money,
	WeekEnding					datetime,
	soft_credit_donor				int,
	congregation_name		varchar(50)
)


INSERT INTO #TMPTOTS (program_name,Total,WeekEnding)
	select distinct c.progname , 0,  DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE)) [WeekEnding]
	from @programs c
	cross join donations don
	WHERE don.donation_date BETWEEN @startdate AND @enddate
	order by WeekEnding, progname;

INSERT INTO #TMPTOTS
	SELECT 
	  p.program_name,
	  dd.amount AS Total,
	  DATEADD(DAY, 7 - DATEPART(WEEKDAY, don.donation_date), CAST(don.donation_date AS DATE)) [WeekEnding],
	  dd.soft_credit_donor,
	  CASE
	   WHEN (dd.soft_credit_donor IS NULL) THEN COALESCE(con.congregation_name,'Not Specified')  
	   ELSE  'Not Specified'
	  END AS congregation_name
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


	select program_name,
		SUM(Total) as Total,
		WeekEnding
		from #TMPTOTS 
		GROUP BY program_name,WeekEnding
		ORDER BY WeekEnding, program_name;

DROP TABLE #TMPTOTS;

END



GO


