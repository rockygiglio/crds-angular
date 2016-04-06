USE MinistryPlatform
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
  (SELECT *
   FROM sys.objects
   WHERE object_id = object_id(N'[dbo].[report_CRDS_Giving_By_Congregation]')
     AND TYPE IN (N'P',
                   N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Giving_By_Congregation] AS';

END 
GO
ALTER PROCEDURE [dbo].[report_CRDS_Giving_By_Congregation] 
     @startdate DATETIME ,
     @enddate DATETIME,
	 @programid AS VARCHAR(MAX) ,
	 @congregationid AS VARCHAR(MAX)
AS 
BEGIN
SET nocount ON;


SET DATEFIRST 1;
SET @startdate = DATEADD(dd, DATEDIFF(dd, 0, @startdate),0);
SET @enddate   =  DATEADD(mi,1439, (DATEADD(dd, DATEDIFF(dd, 0, @enddate),0)));

IF OBJECT_ID('tempdb..#GIVING') IS NOT NULL   
	DROP TABLE #GIVING

CREATE TABLE #GIVING
(
	congregation_name			varchar(50),
	Total						money,
	MonthLabel					varchar(50),
	MonthNumber					int,
	YearNumber					int,
	program_name				varchar(50),
	soft_credit_donor			int
)

INSERT INTO #GIVING
	SELECT 
	  CASE
	   WHEN (dd.soft_credit_donor IS NULL) THEN COALESCE(con.congregation_name,'Not Specified')  
	   ELSE  'Not Specified'
	  END AS congregation_name,
	  dd.amount AS Total,
	  DATENAME(mm,don.donation_date) MonthLabel,
	  DATEPART(mm,don.donation_date) MonthNumber,
	  DATEPART(YEAR,don.donation_date) YearNumber,
	  p.program_name as program_name,
	  dd.soft_credit_donor
	FROM donations don 
	JOIN donors d ON d.donor_id = don.donor_id
	JOIN Donation_Distributions dd on dd.donation_id= don.donation_id
	JOIN contacts c ON c.contact_id = d.contact_id
	JOIN programs p on p.program_id = dd.program_id
	LEFT JOIN congregations con ON con.congregation_id = dd.congregation_id
	WHERE don.donation_date BETWEEN @startdate AND @enddate
	AND dd.program_id IN (SELECT Item FROM dbo.dp_Split(@programid, ','))
	AND dd.congregation_id IN (SELECT Item FROM dbo.dp_Split(@congregationid, ','))

	--update the congregation for the soft credit donations
	UPDATE g
	 SET g.congregation_name = con.congregation_name
	FROM #GIVING g
	JOIN donors d on d.donor_id = g.soft_credit_donor
	JOIN contacts c on c.contact_id = d.contact_id
	JOIN households h on h.household_id = c.household_id
	JOIN congregations con on con.congregation_id = h.congregation_id
	WHERE g.soft_credit_donor is not null


	select congregation_name,
		SUM(Total) as Total,
		MonthLabel,
		MonthNumber,
		YearNumber,
		program_name 
		from #GIVING 
		GROUP BY congregation_name, MonthLabel,MonthNumber,YearNumber,program_name
		ORDER BY program_name,YearNumber, MonthNumber , congregation_name


DROP TABLE #GIVING
END
GO


