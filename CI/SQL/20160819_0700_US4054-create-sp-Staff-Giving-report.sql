USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS
 (SELECT *
  FROM sys.objects
  WHERE object_id = object_id(N'[dbo].[report_CRDS_Staff_Giving]')
    AND TYPE IN (N'P',
                  N'PC')) BEGIN EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[report_CRDS_Staff_Giving] AS';

END 
GO

ALTER PROCEDURE [dbo].[report_CRDS_Staff_Giving] 
     @startdate DATETIME,
     @enddate DATETIME,
	 @programid AS VARCHAR(MAX),
	 @donationstatusid as varchar(MAX)
AS 


BEGIN
SET nocount ON;

IF OBJECT_ID('tempdb..#StaffDonators') IS NOT NULL   
	DROP TABLE #StaffDonators
IF OBJECT_ID('tempdb..#StaffDonations') IS NOT NULL   
	DROP TABLE #StaffDonations
IF OBJECT_ID('tempdb..#StaffDonationstotals') IS NOT NULL
	DROP TABLE #StaffDonationstotals
IF OBJECT_ID('tempdb..#OtherDonators') IS NOT NULL
	DROP TABLE #OtherDonators
IF OBJECT_ID('tempdb..#OtherDonations') IS NOT NULL   
	DROP TABLE #OtherDonations
IF OBJECT_ID('tempdb..#OtherDonationstotals') IS NOT NULL
	DROP TABLE #OtherDonationstotals


SET DATEFIRST 1;
SET @startdate = DATEADD(dd, DATEDIFF(dd, 0, @startdate),0);
SET @enddate   =  DATEADD(ms,86399998, (DATEADD(dd, DATEDIFF(dd, 0, @enddate),0)))

create table #StaffDonators
(
	contactid int,
	hhid int,
	hhpos int,
	cdisplayname varchar(100),
	clname varchar(50),
	cfname varchar(50),
	givetype varchar(15)
)

insert into #StaffDonators
	select c.Contact_ID
	,c.Household_ID
	,c.Household_Position_ID
	,c.Display_Name
	,c.Last_Name
	,c.First_Name
	,'Staff' as givetype
	from contacts c
	left join contact_attributes ca on ca.contact_id = c.contact_id
	where ca.Attribute_ID = 7088
	AND (end_date >= SYSDATETIME () or end_date is null)

CREATE TABLE #StaffDonations
(
	contactid int,
	donorid int, 
	hhid int,
	hhpos int,
	scdonorid int,
	amount	money,
	exclude int,
	dondate	datetime
)

insert into #StaffDonations
	select c.contact_id
	,don.Donor_ID
	,c.Household_ID
	,c.Household_Position_ID
	,dd.Soft_Credit_Donor
	,dd.Amount
	,case 
		when c.household_position_id <> 1 then 1 
	end
	,don.donation_date
		from Donations don
		join donation_distributions dd on dd.donation_id = don.donation_id
		left join donors d on d.donor_id = don.donor_id
		left join contacts c on c.contact_id=d.contact_id
		join #StaffDonators sd on sd.contactid = c.contact_Id and sd.hhid = c.Household_ID
		where dd.Program_ID in (SELECT Item FROM dbo.dp_Split(@programid, ','))
		AND don.donation_status_id in (SELECT Item FROM dbo.dp_Split(@donationstatusid, ','))
		and don.Donation_Date between @startdate AND @enddate
		and dd.Soft_Credit_Donor is null
	union all
	select c.contact_id
	,don.Donor_ID
	,c.Household_ID
	,c.Household_Position_ID
	,dd.Soft_Credit_Donor
	,dd.Amount
	,case 
		when c.household_position_id <> 1 then 1 
	 end
	,don.Donation_Date
		from Donations don
		join donation_distributions dd on dd.donation_id = don.donation_id
		left join donors d on d.donor_id = dd.Soft_Credit_Donor
		left join contacts c on c.contact_id=d.contact_id
		join #StaffDonators sd on sd.contactid = c.contact_Id and sd.hhid = c.Household_ID
		where dd.Program_ID in (SELECT Item FROM dbo.dp_Split(@programid, ','))
		AND don.donation_status_id in (SELECT Item FROM dbo.dp_Split(@donationstatusid, ','))
		and don.Donation_Date between @startdate AND @enddate
		and dd.Soft_Credit_Donor is not null

update #Staffdonations set exclude = 0 where exclude is null

create table #StaffDonationstotals
(
	contactid int,
	cdisplayname varchar(100),
	clname varchar(50),
	cfname varchar(50),
	amount money default 0,
	otheramount money default 0,
	totaldonation money default 0,
	totalhhdonation money default 0,
	householdid int,
	exclude  int
)

insert into #StaffDonationstotals
	select sds.contactid
	,sds.cdisplayname
	,sds.clname
	,sds.cfname
	,sum(sd.amount)
	,0
	,0
	,0
	,sds.hhid
	,sd.exclude 
	from #staffdonations sd
	full join #StaffDonators sds on sds.contactid = sd.contactid
	where sds.contactid is not null
	group by sds.contactid,sds.cdisplayname,sds.clname,sds.cfname, sds.hhid,sd.exclude


create table #OtherDonators
(
	contactid int,
	hhid int,
	hhpos int,
	cdisplayname varchar(100),
	clname varchar(50),
	cfname varchar(50),
	givetype varchar(15),
	staffconid int
)

insert into #OtherDonators
	select distinct c.contact_id
	,c.Household_ID
	,c.Household_Position_ID
	,c.Display_Name
	,c.Last_Name
	,c.First_Name
	,'familymember' as givetype
	,sd.contactid
	from contacts c 
	join #StaffDonators sd on sd.hhid = c.household_id
	where c.Household_Position_ID =1
	and (c.Contact_ID not in (select contactid from #StaffDonators)
	and c.Household_ID in (select hhid from #StaffDonators))
	and c.contact_status_id=1
	and sd.hhpos =1

create table #OtherDonations
(
	contactid int,
	donorid int,
	otheramount money,
	dondate datetime,
	scdonorid int
)

insert into #OtherDonations
	select c.contact_id as othercontact
	,d.Donor_ID
	,dd.Amount as otheramount
	,don.Donation_Date
	,dd.Soft_Credit_Donor
	from donations don
	join donation_distributions dd on dd.donation_id = don.donation_id
	left join donors d on d.Donor_ID = don.Donor_ID
	left join contacts c on c.Contact_ID = d.Contact_ID
	join #OtherDonators od on od.contactid = c.contact_Id and od.hhid = c.Household_ID
	where dd.Program_ID in (SELECT Item FROM dbo.dp_Split(@programid, ','))
		AND don.donation_status_id in (SELECT Item FROM dbo.dp_Split(@donationstatusid, ','))
		and don.Donation_Date between @startdate AND @enddate
		and dd.Soft_Credit_Donor is null
	union all
	select c.contact_id as othercontact
	,d.Donor_ID
	,dd.Amount as otheramount 
	,don.Donation_Date
	,dd.Soft_Credit_Donor
	from donations don
	join donation_distributions dd on dd.donation_id = don.donation_id
	left join donors d on d.donor_id = dd.Soft_Credit_Donor
	left join contacts c on c.contact_id=d.contact_id
	join #OtherDonators od on od.contactid = c.contact_Id and od.hhid = c.Household_ID
	where dd.Program_ID in (SELECT Item FROM dbo.dp_Split(@programid, ','))
		AND don.donation_status_id in (SELECT Item FROM dbo.dp_Split(@donationstatusid, ','))
		and don.Donation_Date between @startdate AND @enddate
		and dd.Soft_Credit_Donor is not null


create table #OtherDonationstotals
(
	contactid int,
	cdisplayname varchar(100),
	clname varchar(50),
	cfname varchar(50),
	otheramounttotal money,
	householdid int,
	staffconid int
)

insert into #OtherDonationstotals
	select ods.contactid
	,ods.cdisplayname
	,ods.clname
	,ods.cfname
	,sum(od.otheramount)
	,ods.hhid
	,ods.staffconid 
	from #otherdonations od
	full join #OtherDonators ods on ods.contactid = od.contactid
	group by ods.contactid,ods.cdisplayname, ods.clname, ods.cfname, ods.hhid, ods.staffconid

update sd
set sd.otheramount = od.totamount
from #StaffDonationstotals sd inner join (select sum(od.otheramounttotal)as totamount,householdid from #OtherDonationstotals od group by householdid) as od
on od.householdid = sd.householdid

UPDATE #StaffDonationstotals set amount = 0 where amount is null

UPDATE #StaffDonationstotals set otheramount = 0 where otheramount is null or exclude = 1

UPDATE #StaffDonationstotals set exclude = 0 where exclude is null

update sdt set sdt.totaldonation = sdt.amount + sdt.otheramount 
from #StaffDonationstotals sdt 

update #StaffDonationstotals set totalhhdonation = sdt.householddonation 
from (
select sum(s.totaldonation) as householddonation, s.householdid as hhid from #StaffDonationstotals s where exclude <>1 group by householdid) sdt
where householdid = sdt.hhid


update #StaffDonationstotals set totalhhdonation = sdt.householddonation 
from (
select sum(s.totaldonation) as householddonation, s.contactid as cid from #StaffDonationstotals s where exclude =1 group by s.contactid) sdt
where contactid = sdt.cid

select sdt.totalhhdonation as 'Giving'
,sdt.cdisplayname as 'Display Name'
,sdt.clname as 'Last Name'
,sdt.cfname as 'First Name'
,sdt.householdid as 'Household ID'
from #StaffDonationstotals sdt
order by sdt.cdisplayname
 
DROP TABLE #StaffDonators 
DROP TABLE #StaffDonations
DROP TABLE #StaffDonationstotals
DROP TABLE #OtherDonators  
DROP TABLE #OtherDonations
DROP TABLE #OtherDonationstotals
 
END
GO