USE [eCheckIn]
GO

/****** Object:  StoredProcedure [dbo].[crds_Echeck_ETL_Load]    Script Date: 9/12/2016 10:08:59 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_Echeck_ETL_Load]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
-- =============================================
-- Author:      Charlie Retzler
-- Create date: 12/17/2015
-- Description: Runs the ETL load process from Ministry Platform to eCheckIn

-- Updated on 09/12/2016 to include populating MP table with echeck table
-- =============================================
ALTER PROCEDURE [dbo].[crds_Echeck_ETL_Load]
AS
BEGIN
	SET NOCOUNT ON;

	TRUNCATE TABLE dbo.tblPerson
	TRUNCATE TABLE dbo.tblRelationship
	TRUNCATE TABLE dbo.tblAdditionalGuardians

	INSERT INTO dbo.tblPerson
		(Person_ID, Person_LName, Person_FName, Person_Nick_Name, Person_Birth_Date, Person_ClassOf, Person_House_ID)	
		SELECT Contact_Id, Last_Name, First_Name, Nickname, Date_of_Birth, HS_Graduation_Year, Person_House_ID 
			FROM OPENQUERY([MinistryPlatformServer], ''EXECUTE MinistryPlatform.dbo.crds_Echeck_ETL_tblPerson'')

	INSERT INTO dbo.tblRelationship 
		(Party1ID, Party2ID)
		SELECT Parent_Contact_ID, Child_Contact_ID 
			FROM OPENQUERY([MinistryPlatformServer], ''EXECUTE MinistryPlatform.dbo.crds_Echeck_ETL_tblRelationship'')

	INSERT INTO dbo.tblAdditionalGuardians 
		(Child_ID, Guardian_ID)
		SELECT Party2ID, Party1ID 
			FROM dbo.tblRelationship 
			
			
	DECLARE @last_inserted_id int;
	SELECT  @last_inserted_id = sarahId FROM  OPENQUERY([MinistryPlatformServer], ''Select MAX(Sarah_ID) as sarahId FROM cr_Echeck_Registrations'') ;
	IF EXISTS ( SELECT * FROM  OPENQUERY([MinistryPlatformServer], ''Select * FROM cr_Echeck_Registrations''))
	BEGIN
		INSERT INTO OPENQUERY([MinistryPlatformServer], ''Select Sarah_ID, Child_ID, Checkin_Date, Checkin_Time FROM cr_Echeck_Registrations'') 
			SELECT ID, Child_ID, Checkin_Date, Checkin_Time FROM [eCheckIn].[dbo].[tblKidsClub_Archived_Registrations] WHERE ID > @last_inserted_id AND Checkin_Date is not null
	END
	ELSE
	BEGIN
		INSERT INTO OPENQUERY([MinistryPlatformServer], ''Select Sarah_ID, Child_ID, Checkin_Date, Checkin_Time FROM cr_Echeck_Registrations'') 
			SELECT ID, Child_ID, Checkin_Date, Checkin_Time FROM [eCheckIn].[dbo].[tblKidsClub_Archived_Registrations] WHERE Checkin_Date > ''2016-01-01''
	END
		
END

' 
END
GO


