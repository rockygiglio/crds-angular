USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_BDay_Anniversary_By_Group]    Script Date: 8/11/2016 12:54:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================

ALTER PROCEDURE [dbo].[report_CRDS_BDay_Anniversary_By_Group]
		@startdate DATETIME,
		@enddate   DATETIME,
		@GroupID AS VARCHAR(MAX)
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
	zipcode varchar(20),
	mobilephone nvarchar(25),
	preferredservetime nvarchar(50),
	groupname nvarchar(75)
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
	displayorder int,
	mobilephone nvarchar(25),
	preferredservetime nvarchar(50),
	groupname nvarchar(75)
)	

		-- find which group members are having a bithday or groupiversary in the given time period
		INSERT INTO #GROUPMEMBERS
			SELECT  DISTINCT p.participant_id, c.NickName, c.Last_name, c.date_of_birth,null,null, 
			min(gp.start_date) as groupstartdate, null, null
			,c.Email_Address, a.address_line_1, a.address_line_2,a.city, a.[state/Region],a.Postal_code
			,c.Mobile_Phone, pst.Preferred_Serve_Time, g.Group_Name
			FROM group_participants gp
			JOIN participants p ON p.participant_id = gp.participant_id
			JOIN contacts c ON c.contact_id = p.contact_id
			LEFT JOIN Households h on h.household_id = c.Household_ID
			LEFT JOIN Addresses a on a.address_id = h.address_id
			LEFT JOIN cr_Preferred_Serve_Time pst on pst.Preferred_Serving_Time_ID = gp.Preferred_Serving_Time_ID
			LEFT JOIN groups g on g.Group_ID = gp.Group_ID
			WHERE gp.group_id IN (SELECT Item FROM dbo.dp_Split(@GroupID, ','))
			AND gp.end_date IS NULL
			group by g.Group_Name, p.Participant_ID, c.Nickname,c.Last_Name,c.Date_of_Birth,c.Email_Address, a.address_line_1, a.address_line_2,a.city, a.[state/Region],a.Postal_code, c.Mobile_Phone, pst.Preferred_Serve_Time
			ORDER BY g.Group_Name, last_name


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
		SELECT distinct 'Happy Birthday' as occasion,CAST(MONTH(dob) AS VARCHAR(2)) + '/' +  CAST(DAY(dob) AS VARCHAR(2)) + '/----' AS thedate,fname,lname,email,addr1 ,	addr2,	city ,stte ,zipcode,1 as displayorder, mobilephone, preferredservetime, groupname  FROM #GROUPMEMBERS WHERE startage <> endage
		UNION all
		SELECT CONVERT(VARCHAR(10),endgroupyear) + ' Year Service Anniversary' AS occasion,CAST(MONTH(groupstart) AS VARCHAR(2)) + '/' +  CAST(DAY(groupstart) AS VARCHAR(2)) + '/' + CAST(YEAR(groupstart) AS VARCHAR(4)) AS thedate,fname,lname,email,addr1 ,	addr2,	city ,stte ,zipcode, 4 as displayorder, mobilephone, preferredservetime, groupname FROM #GROUPMEMBERS WHERE startgroupyear <> endgroupyear and startgroupyear >= 0
		UNION ALL
		-- 3 month groupiversarys
		SELECT '3 Month Service Anniversary' AS occasion,CAST(MONTH(groupstart) AS VARCHAR(2)) + '/' +  CAST(DAY(groupstart) AS VARCHAR(2)) + '/' + CAST(YEAR(groupstart) AS VARCHAR(4)) AS thedate,fname,lname,email,addr1 ,	addr2,	city ,stte ,zipcode, 2 as displayorder, mobilephone, preferredservetime, groupname
		FROM #GROUPMEMBERS where DATEADD(MONTH, 3,groupstart) BETWEEN @startdate AND @enddate
		UNION ALL
		-- 6 month groupiversarys
		SELECT '6 Month Service Anniversary' AS occasion,CAST(MONTH(groupstart) AS VARCHAR(2)) + '/' +  CAST(DAY(groupstart) AS VARCHAR(2)) + '/' + CAST(YEAR(groupstart) AS VARCHAR(4)) AS thedate,fname, lname,email,addr1 ,	addr2,	city ,stte ,zipcode, 3 as displayorder, mobilephone, preferredservetime, groupname
		FROM #GROUPMEMBERS WHERE DATEADD(MONTH, 6,groupstart) BETWEEN @startdate AND @enddate

		select gt.occasion,gt.thedate,gt.fname,gt.lname,gt.email,gt.addr1,gt.addr2,gt.city,gt.stte,gt.zipcode,gt.mobilephone,gt.preferredservetime, gt.groupname
		  from #GROUPOUTPUT gt order by groupname, displayorder, lname
	
	DROP TABLE #GROUPMEMBERS
	DROP TABLE #GROUPOUTPUT
END


GO


