
DECLARE @last_inserted_id int;
SELECT  @last_inserted_id = sarahId FROM OPENQUERY([MinistryPlatformServer], 'Select MAX(Sarah_ID) as sarahId FROM cr_Echeck_Registrations');
IF EXISTS ( SELECT * FROM  OPENQUERY([MinistryPlatformServer], 'Select * FROM cr_Echeck_Registrations'))
BEGIN
	INSERT INTO OPENQUERY([MinistryPlatformServer], 'Select Sarah_ID, Child_ID, Checkin_Date, Checkin_Time, [Service_Day], [Service_Time], [Building_Name] FROM cr_Echeck_Registrations') 
		SELECT registrations.ID, Child_ID, Checkin_Date, Checkin_Time, [Service_Day], [Service_Time], [Building_Location_Name] FROM [eCheckIn].[dbo].[tblKidsClub_Archived_Registrations] registrations
			JOIN [eCheckIn].[dbo].[tblKidsClub_Archived_Services] srv on srv.[ID] = registrations.[Service] 
			JOIN [eCheckIn].[dbo].[tblKidsClub_Building_Locations] loc on loc.[ID] = registrations.[Building_Location_ID] 
			WHERE registrations.ID > @last_inserted_id AND Checkin_Date is not null
END
ELSE
BEGIN
	INSERT INTO OPENQUERY([MinistryPlatformServer], 'Select Sarah_ID, Child_ID, Checkin_Date, Checkin_Time, [Service_Day], [Service_Time], [Building_Name] FROM cr_Echeck_Registrations') 
		SELECT registrations.ID, Child_ID, Checkin_Date, Checkin_Time, [Service_Day], [Service_Time], [Building_Location_Name] FROM [eCheckIn].[dbo].[tblKidsClub_Archived_Registrations] registrations
			JOIN [eCheckIn].[dbo].[tblKidsClub_Archived_Services] srv on srv.[ID] = registrations.[Service] 
			JOIN [eCheckIn].[dbo].[tblKidsClub_Building_Locations] loc on loc.[ID] = registrations.[Building_Location_ID]
			WHERE Checkin_Date > '2016-01-01'
END