USE [MinistryPlatform]
GO

--Yay for Sandi's Mind the Gap tool :-)

--Create the report
SET IDENTITY_INSERT dp_Reports ON
insert into dp_Reports (Report_ID, Report_Name, Description, Report_Path, Pass_Selected_Records, Pass_LinkTo_Records,On_Reports_Tab)
values (301, 'Quarterly Giving List', 'Report that lists all donors (including soft) and co-givers for given date, program, congregation',
       '/MPReports/Crossroads/CRDS Quarterly Giving List', 0,0,0)
SET IDENTITY_INSERT dp_Reports OFF

--Add the report to the Donor page
insert into dp_Report_Pages (report_ID,page_ID)
values (301, 299) 

--Security will be done by Mike Fuhrman

GO