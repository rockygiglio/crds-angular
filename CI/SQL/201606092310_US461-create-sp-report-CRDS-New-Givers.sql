USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_New_Givers]    Script Date: 6/9/2016 3:30:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ====================================================================================
-- Author:		Mike Roberts
-- Create date: 6/9/2016
-- Description:	SP to show New Givers
-- Inputs: start date, end date, program, congregations, donation status 
-- ===================================================================================
IF NOT EXISTS
 (SELECT *
  FROM sys.objects
  WHERE object_id = object_id(N'[dbo].[report_CRDS_New_Givers]')
    AND TYPE IN (N'P',
                  N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_New_Givers] AS';

END 
GO

ALTER PROCEDURE [dbo].[report_CRDS_New_Givers] 
     @startdate DATETIME,
     @enddate DATETIME,
	 @programid as varchar(MAX),
	 @congregationid as varchar(MAX),
	 @donationstatusid as varchar(MAX)
AS 


BEGIN
SET nocount ON;


IF OBJECT_ID('tempdb..#cogivers') IS NOT NULL   
	DROP TABLE #cogivers
IF OBJECT_ID('tempdb..#ftdonormaster') IS NOT NULL   
	DROP TABLE #ftdonormaster
IF OBJECT_ID('tempdb..#ftdonordetail') IS NOT NULL   
	DROP TABLE #ftdonordetail

CREATE TABLE #cogivers
(
	cid1	int,
	cbday   date,
	cid2	int,
	cgbday  date,
	hhid	int
)

insert into #cogivers
	select c1.contact_id,c1.Date_of_Birth, c2.contact_id, c2.Date_of_Birth,c1.household_id
	FROM contacts c1 
	JOIN contacts c2 on c1.household_id = c2.household_id
	join donors d1 on d1.contact_id = c1.contact_id
	join donors d2 on d2.contact_id = c2.contact_id
	where c1.household_position_id = 1
	and c2.household_position_id =1 --head of household
	and d1.statement_type_id = 2 -- family
	and d2.Statement_Type_ID = 2
	and c1.contact_id <> c2.contact_id
	and c1.Contact_Status_ID = 1 --active
	and c2.Contact_Status_ID = 1

CREATE TABLE #ftdonormaster
(
	donor_id int,
	doncontact_id int,
	donation_id int,
	donation_amount money,
	firstdondate datetime,
	program_id int,
	statementtype varchar(25),
	donationstatus varchar(25)
)

insert into #ftdonormaster(donor_id,doncontact_id,donation_id,donation_amount,program_id,firstdondate,statementtype,donationstatus)
select distinct d.donor_id,d.contact_id, don.donation_id, don.donation_amount,dd.program_id,min(d._first_donation_date) as firstdonationdate, st.statement_type,ds.donation_status
	from dbo.donors d 
	left join dbo.donations don on don.donor_id = d.donor_id and don.donation_date = d._first_donation_date
	inner join dbo.donation_distributions dd on dd.donation_id = don.donation_id
	join dbo.Donation_Statuses ds on ds.donation_status_id = don.donation_status_id
	join dbo.Statement_Types st on st.Statement_Type_ID = d.Statement_Type_ID
	where d._first_donation_date between @startdate and @enddate+1 
	and dd.program_id IN (SELECT Item FROM dbo.dp_Split(@programid, ','))
	and don.donation_status_id IN (SELECT Item FROM dbo.dp_Split(@donationstatusid, ','))
group by d.donor_id, d.contact_id,don.donation_id, don.donation_amount,program_id,st.statement_type, ds.donation_status

CREATE TABLE #ftdonordetail
(
	hhid int,
	donation_id int,
	donation_date datetime,
	donation_amount money,
	programname varchar(150),
	cid1 int,
	cbday date,
	cdisplayname varchar(150),
	cfname varchar(50),
	clname varchar(50),
	cid2 int,
	cgbday date,
	cgdisplayname varchar(150),
	cgfname varchar(50),
	cglname varchar(50),
	addr1 varchar(150),
	addr2 varchar(150),
	city varchar(150),
	st varchar(150),
	zip varchar(150),
	site varchar(150),
	to_remove int,
	statementtype varchar(25),
	donationstatus varchar(25)
)

insert into #ftdonordetail(hhid,donation_id,donation_date,donation_amount,programname,cid1,cbday,cdisplayname,cfname,clname,cid2,cgbday,cgdisplayname,cgfname,cglname,addr1,addr2,city,st,zip,site,statementtype,donationstatus)
select h.Household_ID,ftdm.donation_id,ftdm.firstdondate,ftdm.donation_amount,
(select distinct p.program_name from dbo.programs p join #ftdonormaster on p.program_id = ftdm.program_id) as programname,
c.Contact_ID,c.Date_of_Birth,c.Display_Name,c.First_Name,c.Last_Name,cg.cid2,cg.cgbday,c2.display_name as cogivername,c2.First_Name,c2.Last_Name,
Address_Line_1,Address_Line_2,City,[State/Region],Postal_Code,con.congregation_name, ftdm.statementtype,ftdm.donationstatus
	from #ftdonormaster ftdm 
	left join contacts c on c.Contact_ID = ftdm.doncontact_id
	left join #cogivers cg on cg.cid1 = ftdm.doncontact_id
	left join contacts c2 on c2.contact_id = cg.cid2
	left join households h on h.household_id = c.Household_ID
	left join congregations con on con.congregation_id = h.congregation_id
	join addresses a on a.address_id = h.address_id
	where con.Congregation_ID IN (SELECT Item FROM dbo.dp_Split(@congregationid, ','))


update #ftdonordetail set cbday='' where cbday is null
update #ftdonordetail set cgbday='' where cgbday is null
update #ftdonordetail set cdisplayname='' where cdisplayname is null
update #ftdonordetail set cfname ='' where cfname  is null
update #ftdonordetail set clname ='' where clname  is null
update #ftdonordetail set cid2 ='' where cid2  is null
update #ftdonordetail set cgdisplayname ='' where cgdisplayname  is null
update #ftdonordetail set cgfname ='' where cgfname  is null
update #ftdonordetail set cglname ='' where cglname  is null
update #ftdonordetail set addr1='' where addr1 is null
update #ftdonordetail set addr2='' where addr2 is null
update #ftdonordetail set city='' where city is null
update #ftdonordetail set st='' where st is null
update #ftdonordetail set zip='' where zip is null


select f.hhid as HHID,f.donation_id as 'First Donation ID',f.donation_date as 'Donation Date',f.donation_amount as 'Donation Amount',
f.programname as Program,f.cid1 as 'Contact Id',f.cbday as 'Birth Day',f.cdisplayname as 'Display Name',f.cfname as 'First Name',f.clname as 'Last Name',
f.addr1 as 'Address1', f.addr2 as 'Address2',f.city as 'City',f.st as 'State',f.zip as 'Zip',f.site as 'Site',f.statementtype as 'Statement Type',f.donationstatus as 'Donation Status' from #ftdonordetail f
order by f.donation_date asc

END


GO