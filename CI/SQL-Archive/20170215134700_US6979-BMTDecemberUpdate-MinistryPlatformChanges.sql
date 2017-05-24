use [MinistryPlatform]
GO

DECLARE @NewPageId int;
DECLARE @SetupPagesSectionId int;
SELECT @SetupPagesSectionId = Page_Section_ID FROM dp_Page_Sections WHERE Page_Section = 'System Setup';


-- Create Batch Manager's security roles
INSERT INTO dp_Roles(Role_Name, [Description], Domain_ID, _AdminRole)
SELECT 'Batch Manager Administrators', 'Allows to use Batch Manager with administrative privileges.', d.Domain_ID, 0
FROM dp_Domains d 
WHERE NOT EXISTS (SELECT 1 FROM dp_Roles WHERE Role_Name='Batch Manager Administrators' AND Domain_ID=d.Domain_ID);

INSERT INTO dp_Roles(Role_Name, [Description], Domain_ID, _AdminRole)
SELECT 'Batch Manager Operators', 'Allows to use Batch Manager with minimal privileges.', d.Domain_ID, 0
FROM dp_Domains d 
WHERE NOT EXISTS (SELECT 1 FROM dp_Roles WHERE Role_Name='Batch Manager Operators' AND Domain_ID=d.Domain_ID);

-- Add domain administrators to Batch Manager Administrators role
INSERT INTO dp_User_Roles([User_ID], Role_ID, Domain_ID)
SELECT u.[User_ID], r.Role_ID, r.Domain_ID
FROM dp_Users u
INNER JOIN dp_Roles r ON u.Domain_ID=r.Domain_ID AND r.Role_Name='Batch Manager Administrators'
WHERE u.Setup_Admin=1 AND NOT EXISTS (SELECT 1 FROM dp_User_Roles WHERE [User_ID]=u.[User_ID] AND Role_ID=r.Role_ID AND Domain_ID=r.Domain_ID);

-- Change dp_Tasks.Description to be nvarchar(max) so it will use rich text editor
IF NOT EXISTS (SELECT * FROM sys.columns WHERE name='Description' AND max_length=-1 AND object_id=OBJECT_ID('dp_Tasks'))
BEGIN
	ALTER TABLE dp_Tasks ALTER COLUMN [Description] nvarchar(max) NULL;
END

-- Add Donations.Original_MICR column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE name='Original_MICR' AND object_id=OBJECT_ID('Donations'))
BEGIN
	ALTER TABLE Donations ADD Original_MICR nvarchar(100) NULL;
END

-- Add Batches.Default_Program_ID_List column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE name='Default_Program_ID_List' AND object_id=OBJECT_ID('Batches'))
BEGIN
	ALTER TABLE [Batches] ADD [Default_Program_ID_List] nvarchar(32) NULL;
END

-- Create User Congregations page, add to Administrators role
IF NOT EXISTS (SELECT * FROM dp_Pages WHERE Table_Name = 'User_Congregations')
BEGIN
	INSERT INTO dp_Pages (
		Display_Name
		,Singular_Name
		,System_Name
		,[Description]
		,View_Order
		,Table_Name
		,Primary_Key
		,Display_Search
		,Default_Field_List
		,Selected_Record_Expression)
	VALUES
		('User Congregations'
		, 'User Congregation'
		, NULL
		, 'List of congregations which are allowed for specific user'  
		, 10    
		, 'User_Congregations' 
		, 'User_Congregation_ID'
		, 1    
		, 'User_Congregation_ID, User_ID_Table.User_Name, Congregation_ID_Table.Congregation_Name, Default_Congregation, Discontinued'
		, 'Congregation_ID_Table.Congregation_Name');

	SET @NewPageId = SCOPE_IDENTITY();      

	-- Add "User Congregations" page to "Administrators" role
	INSERT INTO dp_Role_Pages (Role_ID,Page_ID,Access_Level,File_Attacher,Data_Importer,Data_Exporter,Allow_Comments)
	SELECT Role_Id,@NewPageId,3,1,0,1,1
	FROM dp_Roles
	WHERE Role_Name = 'Administrators' AND NOT EXISTS (SELECT * FROM dp_Role_Pages WHERE Role_ID = dp_Roles.Role_ID AND Page_ID = @NewPageId);

	-- Add "User Congregations" page to "System Setup" page section
	IF NOT EXISTS (SELECT * FROM dp_Page_Section_Pages WHERE Page_Section_ID = @SetupPagesSectionId AND Page_ID = @NewPageId)
		INSERT INTO dp_Page_Section_Pages(Page_Section_ID,Page_ID) VALUES(@SetupPagesSectionId,@NewPageId);
END

-- Add Donations.Position column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE name='Position' AND object_id=OBJECT_ID('Donations'))
BEGIN
	ALTER TABLE Donations ADD Position int NULL
	EXEC('
		DECLARE @BatchId int
		DECLARE TMP_Cursor CURSOR STATIC READ_ONLY FORWARD_ONLY
			FOR SELECT Batch_Id FROM Batches

		OPEN TMP_Cursor

		FETCH NEXT FROM TMP_Cursor INTO @BatchId

		WHILE @@FETCH_STATUS = 0
		BEGIN
	
			UPDATE d
			SET Position = q.Position
			FROM Donations d
			JOIN (SELECT ROW_NUMBER() OVER(ORDER BY rd.Donation_Id) Position, rd.Donation_ID
					FROM Donations rd 
					WHERE rd.Batch_ID = @BatchId) q ON q.Donation_Id = d.Donation_ID
	
			FETCH NEXT FROM TMP_Cursor INTO @BatchId
		END

		CLOSE TMP_Cursor
		DEALLOCATE TMP_Cursor
	')
END

-- Add Donations.OCR_Data column
IF NOT EXISTS (SELECT * FROM sys.columns WHERE name='OCR_Data' AND object_id=OBJECT_ID('Donations'))
BEGIN
	ALTER TABLE Donations ADD OCR_Data nvarchar(1000) NULL;
END

-- Add new batch entry type and modify config setting
DECLARE @Batch_Entry_Type_ID int
SELECT @Batch_Entry_Type_ID = Batch_Entry_Type_ID FROM [dbo].[Batch_Entry_Types] WHERE [Batch_Entry_Type] = 'Batch Manager Tool'

IF @Batch_Entry_Type_ID IS NULL
BEGIN
	INSERT INTO [dbo].[Batch_Entry_Types] ([Batch_Entry_Type], [Description]) 
	VALUES('Batch Manager Tool', 'A batch created by the Batch Manager Tool')

	SET @Batch_Entry_Type_ID = SCOPE_IDENTITY()
END

IF EXISTS(SELECT 1 FROM [dbo].[dp_Configuration_Settings] WHERE Application_Code = 'BMT' AND [Key_Name] = 'BatchEntryTypeID')
BEGIN
	UPDATE [dbo].[dp_Configuration_Settings]
	SET Value = @Batch_Entry_Type_ID
	WHERE Application_Code = 'BMT' AND [Key_Name] = 'BatchEntryTypeID'
END
ELSE
BEGIN
	INSERT [dbo].[dp_Configuration_Settings] ([Application_Code], [Key_Name], [Value], [Domain_ID])
	SELECT 'BMT', 'BatchEntryTypeID', @Batch_Entry_Type_ID, Domain_ID
	FROM dp_Domains
END

--Copy HouseholdSourceID setting from existing DBT setting
IF NOT EXISTS(SELECT 1 FROM dp_Configuration_Settings WHERE Application_Code = 'BMT' AND [Key_Name] = 'HouseholdSourceID')
BEGIN
	INSERT INTO dp_Configuration_Settings
	SELECT 'BMT' AS Application_Code, [Key_Name], [Value], [Description], [Domain_ID], [_Warning]
	FROM dp_Configuration_Settings WHERE Application_Code = 'DBT' AND [Key_Name] = 'HouseholdSourceID'
END

--Copy CompanyHouseholdPositionID setting from existing DBT setting
IF NOT EXISTS(SELECT 1 FROM dp_Configuration_Settings WHERE Application_Code = 'BMT' AND [Key_Name] = 'CompanyHouseholdPositionID')
BEGIN
	INSERT INTO dp_Configuration_Settings
	SELECT 'BMT' AS Application_Code, [Key_Name], [Value], [Description], [Domain_ID], [_Warning]
	FROM dp_Configuration_Settings WHERE Application_Code = 'DBT' AND [Key_Name] = 'CompanyHouseholdPositionID'
END

--Copy DonorHistoryVisibility setting from existing DBT setting
IF NOT EXISTS(SELECT 1 FROM dp_Configuration_Settings WHERE Application_Code = 'BMT' AND [Key_Name] = 'DonorHistoryVisibility')
BEGIN
	INSERT INTO dp_Configuration_Settings
	SELECT 'BMT' AS Application_Code, [Key_Name], [Value], [Description], [Domain_ID], [_Warning]
	FROM dp_Configuration_Settings WHERE Application_Code = 'DBT' AND [Key_Name] = 'DonorHistoryVisibility'
END

--Copy PhoneValidationMask setting from existing DBT setting
IF NOT EXISTS(SELECT 1 FROM dp_Configuration_Settings WHERE Application_Code = 'BMT' AND [Key_Name] = 'PhoneValidationMask')
BEGIN
	INSERT INTO dp_Configuration_Settings
	SELECT 'BMT' AS Application_Code, [Key_Name], [Value], [Description], [Domain_ID], [_Warning]
	FROM dp_Configuration_Settings WHERE Application_Code = 'DBT' AND [Key_Name] = 'PhoneValidationMask'
END

--Copy PrependAccountNumber setting from existing DBT setting
IF NOT EXISTS(SELECT 1 FROM dp_Configuration_Settings WHERE Application_Code = 'BMT' AND [Key_Name] = 'PrependAccountNumber')
BEGIN
	INSERT INTO dp_Configuration_Settings
	SELECT 'BMT' AS Application_Code, [Key_Name], [Value], [Description], [Domain_ID], [_Warning]
	FROM dp_Configuration_Settings WHERE Application_Code = 'DBT' AND [Key_Name] = 'PrependAccountNumber'
END

--Copy HideBankAndBranch setting from existing DBT setting
IF NOT EXISTS(SELECT 1 FROM dp_Configuration_Settings WHERE Application_Code = 'BMT' AND [Key_Name] = 'HideBankAndBranch')
BEGIN
	INSERT INTO dp_Configuration_Settings
	SELECT 'BMT' AS Application_Code, [Key_Name], [Value], [Description], [Domain_ID], [_Warning]
	FROM dp_Configuration_Settings WHERE Application_Code = 'DBT' AND [Key_Name] = 'HideBankAndBranch'
END

--Copy RemitPlusDecryptionKey setting from existing DBT setting
IF NOT EXISTS(SELECT 1 FROM dp_Configuration_Settings WHERE Application_Code = 'BMT' AND [Key_Name] = 'RemitPlusDecryptionKey')
BEGIN
	INSERT INTO dp_Configuration_Settings
	SELECT 'BMT' AS Application_Code, [Key_Name], [Value], [Description], [Domain_ID], [_Warning]
	FROM dp_Configuration_Settings WHERE Application_Code = 'DBT' AND [Key_Name] = 'RemitPlusDecryptionKey'
END

--Copy RemitPlusDecryptionIV setting from existing DBT setting
IF NOT EXISTS(SELECT 1 FROM dp_Configuration_Settings WHERE Application_Code = 'BMT' AND [Key_Name] = 'RemitPlusDecryptionIV')
BEGIN
	INSERT INTO dp_Configuration_Settings
	SELECT 'BMT' AS Application_Code, [Key_Name], [Value], [Description], [Domain_ID], [_Warning]
	FROM dp_Configuration_Settings WHERE Application_Code = 'DBT' AND [Key_Name] = 'RemitPlusDecryptionIV'
END

--Copy DefaultPaymentTypeID setting from existing DBT setting
IF NOT EXISTS(SELECT 1 FROM dp_Configuration_Settings WHERE Application_Code = 'BMT' AND [Key_Name] = 'DefaultPaymentTypeID')
BEGIN
	INSERT INTO dp_Configuration_Settings
	SELECT 'BMT' AS Application_Code, [Key_Name], [Value], [Description], [Domain_ID], [_Warning]
	FROM dp_Configuration_Settings WHERE Application_Code = 'DBT' AND [Key_Name] = 'DefaultPaymentTypeID'
END



