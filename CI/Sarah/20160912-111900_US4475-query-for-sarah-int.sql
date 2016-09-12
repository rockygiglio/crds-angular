DECLARE @last_inserted_id int;

SELECT  @last_inserted_id = sarahId FROM  OPENQUERY([MinistryPlatformServer], 'Select MAX(Sarah_ID) as sarahId FROM cr_Echeck_Registrations') ;

IF EXISTS ( SELECT * FROM  OPENQUERY([MinistryPlatformServer], 'Select * FROM cr_Echeck_Registrations'))
BEGIN
	INSERT INTO OPENQUERY([MinistryPlatformServer], 'Select Sarah_ID, Child_ID, Checkin_Date, Checkin_Time FROM cr_Echeck_Registrations') 
		SELECT ID, Child_ID, Checkin_Date, Checkin_Time FROM [eCheckIn].[dbo].[tblKidsClub_Archived_Registrations] WHERE ID > @last_inserted_id AND Checkin_Date is not null
END
ELSE
BEGIN
	INSERT INTO OPENQUERY([MinistryPlatformServer], 'Select Sarah_ID, Child_ID, Checkin_Date, Checkin_Time FROM cr_Echeck_Registrations') 
		SELECT ID, Child_ID, Checkin_Date, Checkin_Time FROM [eCheckIn].[dbo].[tblKidsClub_Archived_Registrations] WHERE Checkin_Date > '2016-01-01'
END