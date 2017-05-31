USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_BDay_Anniversary_By_Group]    Script Date: 6/7/2016 1:57:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================================
-- Author:		Mike Roberts
-- Create date: 6/7/2016
-- Description:	SP to show Birthday & Anniversaries by date
-- Inputs: start date, end date, congregations, groups 
-- ===================================================================================
IF NOT EXISTS
 (SELECT *
  FROM sys.objects
  WHERE object_id = object_id(N'[dbo].[report_CRDS_BDay_Anniversary_By_Group]')
    AND TYPE IN (N'P',
                  N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_BDay_Anniversary_By_Group] AS';

END 
GO
-- =============================================

alter PROCEDURE [dbo].[report_CRDS_BDay_Anniversary_By_Group]
		@startdate DATETIME,
		@enddate   DATETIME,
		@groupid INT
AS
    BEGIN
		
IF OBJECT_ID('tempdb..#GROUPMEMBERS') IS NOT NULL   
	DROP TABLE #GROUPMEMBERS
IF OBJECT_ID('tempdb..#GROUPOUTPUT') IS NOT NULL   
	DROP TABLE #GROUPOUTPUT

CREATE TABLE #GROUPMEMBERS
(
	pid	INT,
	fname VARCHAR(50),
	lname VARCHAR(50),
	dob DATETIME,
	startage INT,
	endage INT,
	groupstart DATETIME,
	startgroupyear INT,
	endgroupyear INT,
	email varchar(50),
	addr1 varchar(50),
	addr2 varchar(50),
	city varchar(50),
	stte varchar(50),
	zipcode varchar(20)
)
CREATE TABLE #GROUPOUTPUT
(
	occasion varchar(50),
	thedate vaRCHAR(50),
	fname varchar(50),
	lname VARCHAR(50),
	email varchar(50),
	addr1 varchar(50),
	addr2 varchar(50),
	city varchar(50),
	stte varchar(50),
	zipcode varchar(20),
	displayorder int
)		

		-- find which group members are having a bithday or groupiversary in the given time period
		INSERT INTO #GROUPMEMBERS
			SELECT  DISTINCT p.participant_id, c.NickName, c.Last_name, c.date_of_birth,null,null, 
			min(gp.start_date) as groupstartdate, null, null
			,c.Email_Address, a.address_line_1, a.address_line_2,a.city, a.[state/Region],a.Postal_code
			FROM group_participants gp
			JOIN participants p ON p.participant_id = gp.participant_id
			JOIN contacts c ON c.contact_id = p.contact_id
			LEFT JOIN Households h on h.household_id = c.Household_ID
			LEFT JOIN Addresses a on a.address_id = h.address_id
			WHERE gp.group_id = @groupid
			AND gp.end_date IS NULL
			group by p.Participant_ID, c.Nickname,c.Last_Name,c.Date_of_Birth,c.Email_Address, a.address_line_1, a.address_line_2,a.city, a.[state/Region],a.Postal_code
			ORDER BY last_name


		UPDATE 
		 #GROUPMEMBERS 
		 SET startage = 
		 DATEDIFF(YY,dob,@startdate)-
		 CASE
			WHEN DATEADD(YY,DATEDIFF(YY,dob,@startdate),dob) > @startdate THEN 1
			ELSE 0
		END

		UPDATE 
		 #GROUPMEMBERS 
		 SET endage = 
		 DATEDIFF(YY,dob,@enddate)-
		 CASE
			WHEN DATEADD(YY,DATEDIFF(YY,dob,@enddate),dob) > @enddate THEN 1
			ELSE 0
		END

		UPDATE 
		 #GROUPMEMBERS 
		 SET startgroupyear = 
		 DATEDIFF(YY,groupstart,@startdate)-
		 CASE
			WHEN DATEADD(YY,DATEDIFF(YY,groupstart,@startdate),groupstart) > @startdate THEN 1
			ELSE 0
		END

		UPDATE 
		 #GROUPMEMBERS 
		 SET endgroupyear = 
		 DATEDIFF(YY,groupstart,@enddate)-
		 CASE
			WHEN DATEADD(YY,DATEDIFF(YY,groupstart,@enddate),groupstart) > @enddate THEN 1
			ELSE 0
		END

		
		insert into #GROUPOUTPUT
		SELECT distinct 'Happy Birthday' as occasion,CAST(MONTH(dob) AS VARCHAR(2)) + '/' +  CAST(DAY(dob) AS VARCHAR(2)) + '/----' AS thedate,fname,lname,email,addr1 ,	addr2,	city ,stte ,zipcode,1 as displayorder FROM #GROUPMEMBERS WHERE startage <> endage
		UNION all
		SELECT CONVERT(VARCHAR(10),endgroupyear) + ' Year Service Anniversary' AS occasion,CAST(MONTH(groupstart) AS VARCHAR(2)) + '/' +  CAST(DAY(groupstart) AS VARCHAR(2)) + '/' + CAST(YEAR(groupstart) AS VARCHAR(4)) AS thedate,fname,lname,email,addr1 ,	addr2,	city ,stte ,zipcode, 4 as displayorder FROM #GROUPMEMBERS WHERE startgroupyear <> endgroupyear and startgroupyear >= 0
		UNION ALL
		-- 3 month groupiversarys
		SELECT '3 Month Service Anniversary' AS occasion,CAST(MONTH(groupstart) AS VARCHAR(2)) + '/' +  CAST(DAY(groupstart) AS VARCHAR(2)) + '/' + CAST(YEAR(groupstart) AS VARCHAR(4)) AS thedate,fname,lname,email,addr1 ,	addr2,	city ,stte ,zipcode, 2 as displayorder
		FROM #GROUPMEMBERS where DATEADD(MONTH, 3,groupstart) BETWEEN @startdate AND @enddate
		UNION ALL
		-- 6 month groupiversarys
		SELECT '6 Month Service Anniversary' AS occasion,CAST(MONTH(groupstart) AS VARCHAR(2)) + '/' +  CAST(DAY(groupstart) AS VARCHAR(2)) + '/' + CAST(YEAR(groupstart) AS VARCHAR(4)) AS thedate,fname, lname,email,addr1 ,	addr2,	city ,stte ,zipcode, 3 as displayorder
		FROM #GROUPMEMBERS WHERE DATEADD(MONTH, 6,groupstart) BETWEEN @startdate AND @enddate

		select gt.occasion,gt.thedate,gt.fname,gt.lname,gt.email,gt.addr1,gt.addr2,gt.city,gt.stte,gt.zipcode from #GROUPOUTPUT gt order by displayorder, lname
	
	DROP TABLE #GROUPMEMBERS
	DROP TABLE #GROUPOUTPUT
END

GO


