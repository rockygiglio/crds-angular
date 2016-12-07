USE [MinistryPlatform];

BEGIN TRANSACTION;

DECLARE @AccountingCompanyId int = 2;
DECLARE @AccountingCompanyName nvarchar(75) = 'Crossroads Christian Church (CKY)';
DECLARE @DefaultContactId int = 2; -- ***Default, Contact

DECLARE @CongregationId int = 13;
DECLARE @CongregationName nvarchar(50) = 'Central Kentucky';
DECLARE @CongregationDescription nvarchar(255) = 'Legacy Central Kentucky - all CKY locations, mainly for migrating programs, pledge campaigns, and giving history';
DECLARE @CongregationStartDate datetime = CAST('19870101 00:00:00.000' as DATETIME);
DECLARE @CongregationEndDate datetime = CAST('20161231 00:00:00.000' as DATETIME);

DECLARE @ProgramMinistryId int = 3; -- Finance
DECLARE @ProgramTypeId int = 1; -- Fuel

DECLARE @PledgeCampaignTypeId int = 1; -- Capital Campaign

BEGIN TRY
  -- Setup Legacy Accounting_Company
  IF NOT EXISTS (SELECT 1 FROM Accounting_Companies WHERE Accounting_Company_ID = @AccountingCompanyId)
    BEGIN -- Insert Accounting_Company
      PRINT 'Inserting Accounting Company ' + @AccountingCompanyName;
      SET IDENTITY_INSERT Accounting_Companies ON;
      INSERT INTO [Accounting_Companies] (
        [Accounting_Company_ID]
        ,[Company_Name]
        ,[General_Statement_Settings]
        ,[Pledge_Campaign_ID]
        ,[Company_Contact_ID]
        ,[Statement_Footer]
        ,[List_Non_Cash_Gifts]
        ,[Current_Settings]
        ,[Statement_Letter]
        ,[Online_Settings]
        ,[Online_Sort_Order]
        ,[Show_Online]
        ,[Statement_Cutoff_Date]
        ,[ACH Settings]
        ,[Immediate_Destination_ID]
        ,[Immediate_Origin_ID]
        ,[Immediate_Destination_Name]
        ,[Immediate_Origin_Name]
        ,[Immediate_Routing_Number]
        ,[Receiving_DFI_ID]
        ,[Domain_ID]
      ) VALUES (
        @AccountingCompanyId
        ,@AccountingCompanyName
        ,NULL -- General_Statement_Settings
        ,NULL -- Pledge_Campaign_ID
        ,@DefaultContactId 
        ,NULL -- Statement_Footer
        ,1    -- List_Non_Cash_Gifts
        ,0    -- Current_Settings
        ,NULL -- Statement_Letter
        ,NULL -- Online_Settings
        ,NULL -- Online_Sort_Order
        ,0    -- Show_Online
        ,NULL -- Statement_Cutoff_Date
        ,NULL -- ACH Settings
        ,NULL -- Immediate_Destination_ID
        ,NULL -- Immediate_Origin_ID
        ,NULL -- Immediate_Destination_Name
        ,NULL -- Immediate_Origin_Name
        ,NULL -- Immediate_Routing_Number
        ,NULL -- Receiving_DFI_ID
        ,1    -- Domain_ID
      );
      SET IDENTITY_INSERT Accounting_Companies OFF;
    END -- Insert Accounting_Company
  ELSE
    BEGIN -- Update Accounting_Company
      PRINT 'Updating Accounting Company ' + @AccountingCompanyName;
      UPDATE Accounting_Companies SET
        [Company_Name] = @AccountingCompanyName
        ,[General_Statement_Settings] = NULL
        ,[Pledge_Campaign_ID] = NULL
        ,[Company_Contact_ID] = @DefaultContactId
        ,[Statement_Footer] = NULL
        ,[List_Non_Cash_Gifts] = 1
        ,[Current_Settings] = 0
        ,[Statement_Letter] = NULL
        ,[Online_Settings] = NULL
        ,[Online_Sort_Order] = NULL
        ,[Show_Online] = 0
        ,[Statement_Cutoff_Date] = NULL
        ,[ACH Settings] = NULL
        ,[Immediate_Destination_ID] = NULL
        ,[Immediate_Origin_ID] = NULL
        ,[Immediate_Destination_Name] = NULL
        ,[Immediate_Origin_Name] = NULL
        ,[Immediate_Routing_Number] = NULL
        ,[Receiving_DFI_ID] = NULL
        ,[Domain_ID] = 1
      WHERE
        [Accounting_Company_ID] = @AccountingCompanyId;
    END -- Update Accounting_Company

  -- Setup Legacy Congregation
  IF NOT EXISTS (SELECT 1 FROM Congregations WHERE Congregation_ID = @CongregationId)
    BEGIN -- Insert Congregation
      PRINT 'Inserting Congregation ' + @CongregationName;
      SET IDENTITY_INSERT Congregations ON;
      INSERT INTO [Congregations] (
        [Congregation_ID]
        ,[Congregation_Name]
        ,[Description]
        ,[Start_Date]
        ,[End_Date]
        ,[Accounting_Company_ID]
        ,[Available_Online]
        ,[Location_ID]
        ,[Contact_ID]
        ,[Pastor]
        ,[Domain_ID]
        ,[__ChurchSiteID]
        ,[Online_Sort_Order]
        ,[Front_Desk_SMS_Phone]
        ,[Childcare_Contact]
      ) VALUES (
        @CongregationId
        ,@CongregationName
        ,@CongregationDescription
        ,@CongregationStartDate
        ,@CongregationEndDate
        ,@AccountingCompanyId
        ,0    -- Available_Online
        ,NULL -- Location_ID
        ,@DefaultContactId
        ,@DefaultContactId
        ,1    -- Domain_ID
        ,NULL -- __ChurchSiteID
        ,NULL -- Online_Sort_Order
        ,NULL -- Front_Desk_SMS_Phone
        ,NULL -- Childcare_Contact
      );
      SET IDENTITY_INSERT Congregations OFF;
    END -- Insert Congregation
  ELSE
    BEGIN -- Update Congregation
      PRINT 'Updating Congregation ' + @CongregationName;
      UPDATE Congregations SET
        [Congregation_Name] = @CongregationName
        ,[Description] = @CongregationDescription
        ,[Start_Date] = @CongregationStartDate
        ,[End_Date] = @CongregationEndDate
        ,[Accounting_Company_ID] = @AccountingCompanyId
        ,[Available_Online] = 0
        ,[Location_ID] = NULL
        ,[Contact_ID] = @DefaultContactId
        ,[Pastor] = @DefaultContactId
        ,[Domain_ID] = 1
        ,[__ChurchSiteID] = NULL
        ,[Online_Sort_Order] = NULL
        ,[Front_Desk_SMS_Phone] = NULL
        ,[Childcare_Contact] = NULL
      WHERE
        [Congregation_ID] = @CongregationId;
    END -- Update Congregation

  DECLARE @Programs TABLE (
    Program_Name nvarchar(130),
    Start_Date nvarchar(20),
    End_Date nvarchar(20),
    Statement_Title nvarchar(50),
    Statement_Header_ID int,
    Pledge_Campaign_Name nvarchar(50)
  );

  INSERT INTO @Programs VALUES
    ('2011 Giving', '1/1/2011', '12/31/2011', '2011 Giving', 1, NULL),
    ('2012 Giving', '1/1/2012', '12/31/2012', '2012 Giving', 1, NULL),
    ('2013 Giving', '1/1/2013', '12/31/2013', '2013 Giving', 1, NULL),
    ('2014 1st Half Giving', '1/1/2014', '6/30/2014', '2014 1st Half Giving', 1, NULL),
    ('ReImagine', '1/1/2014', '12/31/2014', 'ReImagine', 6, NULL),
    ('Bolivia Mission', '7/1/2014', '12/31/2016', 'Bolivia Mission', 1, NULL),
    ('Bahama Mission Trip', '7/1/2014', '12/31/2016', 'Bahama Mission Trip', 1, NULL),
    ('Africa Mission', '7/1/2014', '12/31/2016', 'Africa Mission', 1, NULL),
    ('Andover Giving', '7/1/2014', '12/31/2016', 'Andover Giving', 1, NULL),
    ('Downtown Lex Giving', '7/1/2014', '12/31/2016', 'Downtonw Lex Giving', 1, NULL),
    ('Georgetown Giving', '7/1/2014', '12/31/2016', 'Georgetown Giving', 1, NULL),
    ('Richmond Giving', '7/1/2014', '12/31/2016', 'Richmond Giving', 1, NULL),
    ('Other Giving', '7/1/2014', '12/31/2016', 'Other Giving', 6, NULL),
    ('Game Change - Andover', '1/1/2016', '12/31/2016', 'Game Change - Andover', 3, 'Game Change - Andover'),
    ('Game Change - Downtown Lex', '1/1/2016', '12/31/2016', 'Game Change - Downtown Lex', 3, 'Game Change - Downtown Lex'),
    ('Game Change - Georgetown', '1/1/2016', '12/31/2016', 'Game Change - Georgetown', 3, 'Game Change - Georgetown'),
    ('Game Change - Richmond', '1/1/2016', '12/31/2016', 'Game Change - Richmond', 3, 'Game Change - Richmond'),
    ('Game Change - No Campus', '1/1/2016', '12/31/2016', 'Game Change - No Campus', 3, 'Game Change - No Campus'),
    ('10 for 10 (Student Ministries)', '7/1/2014', '12/31/2016', '10 for 10 (Student Ministries)', 6, NULL);

  DECLARE @Pledge_Campaigns TABLE (
    Campaign_Name nvarchar(50),
    Nickname nvarchar(50),
    Description nvarchar(500),
    Program_Name nvarchar(50)
  );

  INSERT INTO @Pledge_Campaigns VALUES
    ('Game Change - Andover', 'Game Change - Andover', 'Game Change - Andover', 'Game Change - Andover'),
    ('Game Change - Downtown Lex', 'Game Change - Downtown Lex', 'Game Change - Downtown Lex', 'Game Change - Downtown Lex'),
    ('Game Change - Georgetown', 'Game Change - Georgetown', 'Game Change - Georgetown', 'Game Change - Georgetown'),
    ('Game Change - Richmond', 'Game Change - Richmond', 'Game Change - Richmond', 'Game Change - Richmond'),
    ('Game Change - No Campus', 'Game Change - No Campus', 'Game Change - No Campus', 'Game Change - No Campus');

  DECLARE @Program_Name nvarchar(130), @Start_Date nvarchar(20), @End_Date nvarchar(20), @Statement_Title nvarchar(50), @Statement_Header_ID int, @Pledge_Campaign_Name nvarchar(50);

  DECLARE @PC_Campaign_Name nvarchar(50), @PC_Nickname nvarchar(50), @PC_Description nvarchar(500), @PC_Program_Name nvarchar(50);

  DECLARE @Program_ID int, @Pledge_Campaign_ID int;

  DECLARE Programs_Cursor CURSOR FOR SELECT * FROM @Programs;
  OPEN Programs_Cursor;
  FETCH NEXT FROM Programs_Cursor INTO
    @Program_Name, @Start_Date, @End_Date, @Statement_Title, @Statement_Header_ID, @Pledge_Campaign_Name;
  WHILE @@FETCH_STATUS = 0
    BEGIN -- Fetch Programs
      SELECT @Program_ID=Program_ID FROM Programs WHERE Program_Name = @Program_Name;

      IF @@ROWCOUNT <= 0
        BEGIN -- Insert Program
          PRINT 'Inserting Program ' + @Program_Name;
          INSERT INTO Programs (
            Program_Name
            , Congregation_ID
            , Ministry_ID
            , Start_Date
            , End_Date
            , Program_Type_ID
            , Primary_Contact
            , Tax_Deductible_Donations
            , Statement_Title
            , Statement_Header_ID
            , Allow_Online_Giving
            , On_Donation_Batch_Tool
            , Domain_ID
            , Available_Online
            , Allow_Recurring_Giving
          ) VALUES (
            @Program_Name
            , @CongregationId
            , @ProgramMinistryId
            , CAST(@Start_Date + ' 00:00:00.000' AS datetime)
            , CAST(@End_Date + ' 00:00:00.000' AS datetime)
            , @ProgramTypeId
            , @DefaultContactId
            , 1 -- Tax_Deductible_Donations
            , @Statement_Title
            , @Statement_Header_ID
            , 0 -- Allow_Online_Giving
            , 0 -- On_Donation_Batch_Tool
            , 1 -- Domain_ID
            , 0 -- Available_Online
            , 0 -- Allow_Recurring_Giving
          );

          SET @Program_ID = SCOPE_IDENTITY();
        END -- Insert Program
      ELSE
        BEGIN -- Update Program
          PRINT 'Updating Program ' + @Program_Name;
          UPDATE Programs SET
              Program_Name = @Program_Name
            , Congregation_ID = @CongregationId
            , Ministry_ID = @ProgramMinistryId
            , Start_Date = CAST(@Start_Date + ' 00:00:00.000' AS datetime)
            , End_Date = CAST(@End_Date + ' 00:00:00.000' AS datetime)
            , Program_Type_ID = @ProgramTypeId
            , Primary_Contact = @DefaultContactId
            , Tax_Deductible_Donations = 1
            , Statement_Title = @Statement_Title
            , Statement_Header_ID = @Statement_Header_ID
            , Allow_Online_Giving = 0
            , On_Donation_Batch_Tool = 0
            , Domain_ID = 1
            , Available_Online = 0
            , Allow_Recurring_Giving = 0
          WHERE
            Program_ID = @Program_ID;
        END -- Update Program

      IF @Pledge_Campaign_Name IS NOT NULL
      BEGIN -- Program has a Pledge_Campaign
        SELECT @Pledge_Campaign_ID=Pledge_Campaign_ID FROM Pledge_Campaigns WHERE Campaign_Name = @Pledge_Campaign_Name;
        IF @@ROWCOUNT <= 0
          BEGIN -- Insert Pledge_Campaign for Program
            PRINT 'Inserting Pledge Campaign ' + @Pledge_Campaign_Name;
            SELECT @PC_Campaign_Name=Campaign_Name, @PC_Nickname=Nickname, @PC_Description=Description FROM @Pledge_Campaigns WHERE Campaign_Name = @Pledge_Campaign_Name;
            INSERT INTO Pledge_Campaigns (
              Campaign_Name
              , Nickname
              , Pledge_Campaign_Type_ID
              , Description
              , Campaign_Goal
              , Start_Date
              , End_Date
              , Domain_ID
              , Program_ID
              , Allow_Online_Pledge
              , Pledge_Beyond_End_Date
              , Show_On_My_Pledges
            ) VALUES ( 
              @PC_Campaign_Name
              , @PC_Nickname
              , @PledgeCampaignTypeId
              , @PC_Description
              , 0 -- Campaign_Goal
              , '01/01/2016' -- Start_Date
              , '12/31/2016' -- End_Date
              , 1 -- Domain_ID
              , @Program_ID
              , 0 -- Allow_Online_Pledges
              , 1 -- Pledge_Beyond_End_Date
              , 1 -- Show_On_My_Pledges
            );

            SET @Pledge_Campaign_ID = SCOPE_IDENTITY();
          END -- Insert Pledge_Campaign for Program
        ELSE
          BEGIN -- Update Pledge_Campaign for Program
            PRINT 'Updating Pledge Campaign ' + @Pledge_Campaign_Name;
            UPDATE Pledge_Campaigns SET
              Campaign_Name = @PC_Campaign_Name
              , Nickname = @PC_Nickname
              , Pledge_Campaign_Type_ID = @PledgeCampaignTypeId
              , Description = @PC_Description
              , Campaign_Goal = 0
              , Start_Date = '01/01/2016'
              , End_Date = '12/31/2016'
              , Domain_ID = 1
              , Program_ID = @Program_ID
              , Allow_Online_Pledge = 0
              , Pledge_Beyond_End_Date = 1
              , Show_On_My_Pledges = 1
            WHERE
              Pledge_Campaign_ID = @Pledge_Campaign_ID;
          END -- Update Pledge_Campaign for Program

        PRINT 'Adding Pledge Campaign ' + @Pledge_Campaign_Name + ' to Program ' + @Program_Name;
        UPDATE Programs SET Pledge_Campaign_ID = @Pledge_Campaign_ID WHERE Program_ID = @Program_ID;
      END; -- Program has a Pledge_Campaign

      FETCH NEXT FROM Programs_Cursor INTO
        @Program_Name, @Start_Date, @End_Date, @Statement_Title, @Statement_Header_ID, @Pledge_Campaign_Name;
    END -- Fetch Programs

  CLOSE Programs_Cursor;

  DECLARE Pledge_Campaigns_Cursor CURSOR FOR SELECT * FROM @Pledge_Campaigns WHERE Program_Name IS NULL;
  OPEN Pledge_Campaigns_Cursor;
  FETCH NEXT FROM Pledge_Campaigns_Cursor INTO
    @PC_Campaign_Name, @PC_Nickname, @PC_Description, @PC_Program_Name;

  WHILE @@FETCH_STATUS = 0
    BEGIN -- Fetch Pledge_Campaigns
      SELECT @Pledge_Campaign_ID=Pledge_Campaign_ID FROM Pledge_Campaigns WHERE Campaign_Name = @PC_Campaign_Name;
      IF @@ROWCOUNT <= 0
        BEGIN -- Insert Pledge_Campaign
          PRINT 'Inserting Pledge Campaign ' + @Pledge_Campaign_Name;
          INSERT INTO Pledge_Campaigns (
            Campaign_Name
            , Nickname
            , Pledge_Campaign_Type_ID
            , Description
            , Campaign_Goal
            , Start_Date
            , End_Date
            , Domain_ID
            , Program_ID
            , Allow_Online_Pledge
            , Pledge_Beyond_End_Date
            , Show_On_My_Pledges
          ) VALUES ( 
            @PC_Campaign_Name
            , @PC_Nickname
            , @PledgeCampaignTypeId
            , @PC_Description
            , 0 -- Campaign_Goal
            , '01/01/2016' -- Start_Date
            , '12/31/2016' -- End_Date
            , 1 -- Domain_ID
            , NULL -- Program_ID
            , 0 -- Allow_Online_Pledges
            , 1 -- Pledge_Beyond_End_Date
            , 1 -- Show_On_My_Pledges
          );
        END -- Insert Pledge_Campaign
      ELSE
        BEGIN -- Update Pledge_Campaign
          PRINT 'Updating Pledge Campaign ' + @Pledge_Campaign_Name;
          UPDATE Pledge_Campaigns SET
            Campaign_Name = @PC_Campaign_Name
            , Nickname = @PC_Nickname
            , Pledge_Campaign_Type_ID = @PledgeCampaignTypeId
            , Description = @PC_Description
            , Campaign_Goal = 0
            , Start_Date = '01/01/2016'
            , End_Date = '12/31/2016'
            , Domain_ID = 1
            , Program_ID = @Program_ID
            , Allow_Online_Pledge = 0
            , Pledge_Beyond_End_Date = 1
            , Show_On_My_Pledges = 1
          WHERE
            Pledge_Campaign_ID = @Pledge_Campaign_ID;
        END; -- Update Pledge_Campaign

      FETCH NEXT FROM Pledge_Campaigns_Cursor INTO
        @PC_Campaign_Name, @PC_Nickname, @PC_Description, @PC_Program_Name;
    END -- Fetch Pledge_Campaigns

  CLOSE Pledge_Campaigns_Cursor;
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0
    BEGIN
      PRINT 'Rolling back transaction due to error ' + ERROR_MESSAGE();
      ROLLBACK TRANSACTION;
    END
END CATCH

IF @@TRANCOUNT > 0
  BEGIN
    PRINT 'Final Results';
    SELECT * FROM Accounting_Companies WHERE Accounting_Company_ID = @AccountingCompanyId;
    SELECT * FROM Congregations WHERE Congregation_ID = @CongregationId;
    SELECT * FROM Programs WHERE Congregation_ID = @CongregationId;
    SELECT * FROM Pledge_Campaigns WHERE Campaign_Name LIKE 'Game Change - %';
    
    PRINT 'Committing transaction';
    COMMIT TRANSACTION;
  END
