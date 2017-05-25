USE [MinistryPlatform]
GO

IF NOT EXISTS(SELECT *
              FROM sys.columns
              WHERE Name = N'Source_Information'
              AND Object_ID = Object_ID(N'Donations'))
BEGIN
    ALTER TABLE [dbo].[Donations]
    ADD Source_Information dp_Separator NULL;
END

IF NOT EXISTS ( SELECT *
				FROM sys.columns 
				WHERE Name = N'Source_Url'
				AND Object_ID = Object_ID(N'Donations') )
	BEGIN
		ALTER TABLE [dbo].[Donations]
		ADD Source_Url nvarchar(512)
	END
GO

IF NOT EXISTS ( SELECT *
				FROM sys.columns 
				WHERE Name = N'Predefined_Amount'
				AND Object_ID = Object_ID(N'Donations') )
	BEGIN
		ALTER TABLE [dbo].[Donations]
		ADD Predefined_Amount decimal(6,2)
	END
GO

IF EXISTS(SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = 297)
	BEGIN
		UPDATE dp_Pages
		SET Default_Field_List = 
        'Donations.Donation_Date
		,Donor_ID_Table_Contact_ID_Table.Display_Name
		,Donor_ID_Table_Contact_ID_Table.Nickname
		,Donor_ID_Table_Contact_ID_Table.First_Name
		,Donations.Donation_Amount
		,Payment_Type_ID_Table.Payment_Type
		,Item_Number
		,Transaction_Code
		,Subscription_Code
		,Batch_ID_Table.Batch_ID
		,Batch_ID_Table.Setup_Date
		,Donations.Registered_Donor
		,Processor_Fee_Amount
		,Donor_ID_Table.Donor_ID
		,Donation_Status_ID_Table.Donation_Status
		,Donation_Status_Notes
		,Donations.Check_Scanner_Batch
		,Is_Recurring_Gift
		,Donation_Status_ID_Table.Donation_Status_ID
		,Donations.Source_Url
		,Donations.Predefined_Amount'
		WHERE page_id = 297
	END
GO

IF NOT EXISTS ( SELECT *
				FROM sys.columns 
				WHERE Name = N'Source_Url'
				AND Object_ID = Object_ID(N'Recurring_Gifts') )
	BEGIN
		ALTER TABLE [dbo].[Recurring_Gifts]
		ADD Source_Url nvarchar(512)
	END
GO

IF NOT EXISTS ( SELECT *
				FROM sys.columns 
				WHERE Name = N'Predefined_Amount'
				AND Object_ID = Object_ID(N'Recurring_Gifts') )
	BEGIN
		ALTER TABLE [dbo].[Recurring_Gifts]
		ADD Predefined_Amount decimal(6,2)
	END
GO

IF EXISTS(SELECT 1 FROM [dbo].[dp_Pages] WHERE Page_ID = 517)
	BEGIN
		UPDATE dp_Pages
		SET Default_Field_List = 
			'Donor_ID_Table_Contact_ID_Table.[Display_Name]
			,Donor_ID_Table_Contact_ID_Table_User_Account_Table.[User_Email]
			,Frequency_ID_Table.[Frequency]
			,CASE(Frequency_ID_Table.Frequency_ID)
			  WHEN 1
				THEN
				  CONCAT(
					''Every '',
					Day_Of_Week_ID_Table.Day_Of_Week
				  )
			  ELSE
				CONCAT(
				  CAST(Day_Of_Month AS VARCHAR),
				  CASE(Day_Of_Month % 10)
					WHEN 1 THEN ''st''
					WHEN 2 THEN ''nd''
					WHEN 3 THEN ''rd''
					ELSE ''th''
				  END,
				  '' of the month''
				)
			  END AS Recurrence
			,Recurring_Gifts.[Start_Date]
			,Recurring_Gifts.[End_Date]
			,Recurring_Gifts.[Amount]
			,Program_ID_Table.[Program_Name]
			,Congregation_ID_Table.[Congregation_Name]
			,CONCAT(Donor_Account_ID_Table_Account_Type_ID_Table.[Account_Type], ''/'', Donor_Account_ID_Table.[Account_Number]
			,''/'', Donor_Account_ID_Table.[Institution_Name]) AS [Donor_Account]
			,Recurring_Gifts.[Subscription_ID]
			,Recurring_Gifts.Source_Url
			,Recurring_Gifts.Predefined_Amount'
		WHERE page_id = 517
	END
GO