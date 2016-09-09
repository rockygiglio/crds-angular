USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_Giving_List]    Script Date: 9/9/2016 12:57:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[report_CRDS_Giving_List] 
     @startdate DATETIME,
     @enddate DATETIME,
	 @programid as varchar(MAX),
	 @congregationid as varchar(MAX),
	 @donationstatusid as varchar(MAX)
AS 


BEGIN
SET nocount ON;


DECLARE @startdateDATEONLY DATE, @enddateDATEONLY DATE
SET @startdateDATEONLY = CAST (@startdate AS DATE)
SET @enddateDATEONLY = CAST (@enddate AS DATE)


IF OBJECT_ID('tempdb..#cogivers') IS NOT NULL   
	DROP TABLE #cogivers
IF OBJECT_ID('tempdb..#ftdonormaster') IS NOT NULL   
	DROP TABLE #ftdonormaster
IF OBJECT_ID('tempdb..#sddonormaster') IS NOT NULL   
	DROP TABLE #sddonormaster
IF OBJECT_ID('tempdb..#alldonordetail') IS NOT NULL   
	DROP TABLE #alldonordetail

	
--build cogiver/spouse model
CREATE TABLE #cogivers
(
	cid1	int,
	cid2	int,
	hhid	int
)

--store donor list	
CREATE TABLE #ftdonormaster
(
	donor_id int,
	doncontact_id int,
	hhid int
)

--store sc donor list
CREATE TABLE #sddonormaster
(
	sd_donor_id int
)

--first select is to grab regular donor information
insert into #ftdonormaster(donor_id,doncontact_id, hhid)
select distinct d.donor_id,d.contact_id, c.household_id
	from dbo.donors d 
	left join dbo.donations don on don.donor_id = d.donor_id 
	inner join dbo.donation_distributions dd on dd.donation_id = don.donation_id
	inner join dbo.Donation_Statuses ds on ds.donation_status_id = don.donation_status_id
	inner join dbo.Statement_Types st on st.Statement_Type_ID = d.Statement_Type_ID
	inner join dbo.Contacts c on c.Contact_ID = d.Contact_ID
	where 
	    CAST (don.Donation_Date AS DATE) >= @startdateDATEONLY
	and CAST (don.Donation_Date AS DATE) <=  @enddateDATEONLY
	and dd.Soft_Credit_Donor is null
	and don.Donation_Amount > 0
	and dd.program_id IN (SELECT Item FROM dbo.dp_Split(@programid, ','))
	and don.donation_status_id IN (SELECT Item FROM dbo.dp_Split(@donationstatusid, ','))


--get the donor id for the soft credit donor
insert into #sddonormaster(sd_donor_id)
select distinct dd.Soft_Credit_Donor
	from dbo.donors d 
	left join dbo.donations don on don.donor_id = d.donor_id
	inner join dbo.donation_distributions dd on dd.donation_id = don.donation_id
	inner join dbo.Donation_Statuses ds on ds.donation_status_id = don.donation_status_id
	inner join dbo.Statement_Types st on st.Statement_Type_ID = d.Statement_Type_ID
	inner join dbo.Contacts c on c.Contact_ID = d.Contact_ID
	where CAST (don.Donation_Date AS DATE) >= @startdateDATEONLY
	and CAST (don.Donation_Date AS DATE) <=  @enddateDATEONLY
	and dd.Soft_Credit_Donor is not null
	and don.Donation_Amount > 0
	and dd.program_id IN (SELECT Item FROM dbo.dp_Split(@programid, ','))
	and don.donation_status_id IN (SELECT Item FROM dbo.dp_Split(@donationstatusid, ','))

--add soft credit donor contact id and append into ftdonormaster
insert into #ftdonormaster(donor_id,doncontact_id,hhid)
select sd_donor_id, c.Contact_ID, c.household_id
 from #sddonormaster sd
 left join Donors d on sd.sd_donor_id = d.Donor_ID
 left join Contacts c on d.Contact_ID = c.Contact_ID

 --now get the cogiver info
 insert into #cogivers
	select c1.contact_id,c2.contact_id,c1.household_id
	FROM
	#ftdonormaster ftd  
	inner join contacts c1 on ftd.hhid = c1.household_id
	inner join contacts c2 on c1.household_id = c2.household_id
	inner join donors d1 on d1.contact_id = c1.contact_id
	inner join donors d2 on d2.contact_id = c2.contact_id
	where c1.household_position_id = 1
	and c2.household_position_id =1 --head of household	
	and (d1.statement_type_id = 2 -- family
	  or d2.statement_type_id = 2)
	and c1.contact_id <> c2.contact_id

--store/build all the additional detail about the donor
CREATE TABLE #alldonordetail
(
	hhid int,	
	cid1 int,	
	cfname varchar(50),
	clname varchar(50),
	cemail varchar(255),
	cid2 int,
	cgfname varchar(50),
	cglname varchar(50),
	cgemail varchar(255)
)

insert into #alldonordetail(hhid,cid1,cfname,clname,cemail,cid2,cgfname,cglname,cgemail)
select h.Household_ID, c.Contact_ID,
c.First_Name,c.Last_Name,c.Email_Address,c2.Contact_ID, c2.First_Name,c2.Last_Name, c2.Email_Address
	from #ftdonormaster ftdm 
	left join contacts c on c.Contact_ID = ftdm.doncontact_id
	left join #cogivers cg on cg.cid1 = ftdm.doncontact_id
	left join contacts c2 on c2.contact_id = cg.cid2
	left join households h on h.household_id = c.Household_ID
	left join congregations con on con.congregation_id = h.congregation_id
	where con.Congregation_ID IN (SELECT Item FROM dbo.dp_Split(@congregationid, ',')) or con.Congregation_ID is null

update #alldonordetail set cfname ='' where cfname  is null
update #alldonordetail set clname ='' where clname  is null
update #alldonordetail set cemail ='' where cemail  is null

update #alldonordetail set cgfname ='' where cgfname  is null
update #alldonordetail set cglname ='' where cglname  is null
update #alldonordetail set cgemail ='' where cgemail  is null



select f.cid1 as 'Contact Id',f.cfname as 'First Name',f.clname as 'Last Name', f.cemail as 'Email Address',
f.cid2 as 'Spouse Id',f.cgfname as 'Spouse FName',f.cglname as 'Spouse LName', f.cgemail as 'Spouse Email'
from #alldonordetail f order by f.cfname asc

END
GO