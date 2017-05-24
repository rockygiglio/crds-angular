USE MinistryPlatform;
GO

SET IDENTITY_INSERT dbo.programs ON;  
GO 

IF NOT EXISTS (SELECT * FROM dbo.programs WHERE Program_ID = 270 AND Program_Name='Childcare')
BEGIN
  INSERT INTO dbo.programs(
		Program_ID,
		Program_Name,
		Congregation_ID,
		Ministry_ID,
		Start_Date,
		End_Date,
		Program_Type_ID,
		Primary_Contact,
		Tax_Deductible_Donations,
		Statement_Title,
		Statement_Header_ID,
		Allow_Online_Giving,
		On_Donation_Batch_Tool,
		Domain_ID,
		Allow_Recurring_Giving)
	VALUES(
		270,
		'Childcare',
		5,
		2,
		'2016-6-29 00:00',
		NULL,
		4,
		2,
		0,
		'Childcare',
		6,
		0,
		0,
		1,
		0)
END

SET IDENTITY_INSERT dbo.programs OFF;  
GO 

