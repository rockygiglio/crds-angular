USE [MinistryPlatform];
PRINT 'Central KY - Facility Setups';

DECLARE @name AS VARCHAR(50);
DECLARE @pastor_id INT;
DECLARE @contact_id INT;
DECLARE @address_id INT;
DECLARE @location_id INT;
DECLARE @congregation_id INT;
DECLARE @building_id INT;
DECLARE @startdate DATETIME;

/* Assert AndoverKY (pastor, location, congregation, building) */
BEGIN TRANSACTION;
BEGIN TRY

SET @name = 'Andover';
PRINT @name;

  -- assert pastor
SELECT @pastor_id = User_ID FROM [dbo].[dp_Users]
 WHERE [User_Email] = 'mark.s@onecity.org';
IF @@ROWCOUNT = 0
  THROW 10000, 'pastor not found', 1;

  -- address
SELECT @address_id = Address_ID FROM [dbo].[Addresses]
 WHERE [Address_Line_1] = '4128 Todds Rd'
   AND [City] = 'Lexington'
   AND [State/Region] = 'KY'
   AND [Postal_Code] = '40509';
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting address for '+@name;
    INSERT INTO [dbo].[Addresses] (
       [Address_Line_1]
      ,[City]
      ,[State/Region]
      ,[Postal_Code]
      ,[Domain_ID]
    ) VALUES (
       '4128 Todds Rd'
      ,'Lexington'
      ,'KY'
      ,'40509'
      ,1
    );
    SET @address_id = SCOPE_IDENTITY();
  END

  -- assert location
SELECT @location_ID = Location_ID FROM [dbo].[Locations]
 WHERE [Location_Name] = @name;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting location for '+@name;
    INSERT INTO [dbo].[Locations] (
       [Location_Name]
      ,[Domain_ID]
      ,[Location_Type_ID]
      ,[Address_ID]
    ) VALUES (
       @name
      ,1
      ,1
      ,@address_id
    );
    SET @location_id = SCOPE_IDENTITY();
  END

  -- assert congregation
SELECT @congregation_id = Congregation_ID FROM [dbo].[Congregations]
 WHERE [Location_ID] = @location_id;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting congregation for '+@name;
	  SET @startdate = '20170101';
    INSERT INTO [dbo].[Congregations] (
       [Congregation_Name]
      ,[Location_ID]
      ,[Start_Date]
      ,[Accounting_Company_ID]
      ,[Contact_ID]
      ,[Pastor]
      ,[Domain_ID]
    ) VALUES (
       @name
      ,@location_id
      ,@startdate
      ,1
      ,7
      ,@pastor_id
      ,1
    );
    SET @congregation_id = SCOPE_IDENTITY();
  END

  -- assert building
SELECT @building_id = Building_ID FROM [dbo].[Buildings]
 WHERE [Location_ID] = @location_id;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting building for '+@name;
    INSERT INTO [dbo].[Buildings] (
       [Building_Name]
      ,[Location_ID]
      ,[Domain_ID]
    ) VALUES (
       @name
      ,@location_id
      ,1
    );
    SET @building_id = SCOPE_IDENTITY();
  END

END TRY
BEGIN CATCH
  PRINT '   Rolling back transaction due to error: ' + ERROR_MESSAGE();
  if @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
  BEGIN
    PRINT '  committing '+@name;
    COMMIT TRANSACTION;
  END


/* Assert GeorgetownKY (pastor, location, congregation, building) */
BEGIN TRANSACTION;
BEGIN TRY

SET @name = 'Georgetown';
PRINT @name;

  -- assert pastor
SELECT @pastor_id = User_ID FROM [dbo].[dp_Users]
 WHERE [User_Email] = 'bstone@xroadschurch.org';
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting pastor for '+@name;
    INSERT INTO [dbo].[Contacts] (
       [First_Name]
      ,[Last_Name]
      ,[Company]
      ,[Display_Name]
      ,[Contact_Status_ID]
      ,[Email_Address]
      ,[Bulk_Email_Opt_Out]
      ,[Bulk_SMS_Opt_Out]
      ,[Contact_GUID]
      ,[Domain_ID]
    ) VALUES (
       'Brad'
      ,'Stone'
      ,0
      ,'Stone, Brad'
      ,1
      ,'bstone@xroadschurch.org'
      ,0
      ,0
      ,NEWID()
      ,1
    );
    SET @contact_id = SCOPE_IDENTITY();
    INSERT INTO [dbo].[dp_Users] (
       [User_Name]
      ,[User_Email]
      ,[Admin]
      ,[Domain_ID]
      ,[Publications_Manager]
      ,[Contact_ID]
      ,[User_GUID]
      ,[Keep_For_Go_Live]
      ,[Read_Permitted]
      ,[Create_Permitted]
      ,[Update_Permitted]
      ,[Delete_Permitted]
    ) VALUES (
       'bstone@xroadschurch.org'
      ,'bstone@xroadschurch.org'
      ,0
      ,1
      ,0
      ,@contact_id
      ,NEWID()
      ,1
      ,1
      ,1
      ,1
      ,1
    );
    SET @pastor_id = SCOPE_IDENTITY();
  END

  -- address
SELECT @address_id = Address_ID FROM [dbo].[Addresses]
 WHERE [Address_Line_1] = '1696 Oxford Dr'
   AND [City] = 'Georgetown'
   AND [State/Region] = 'KY'
   AND [Postal_Code] = '40324';
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting address for '+@name;
    INSERT INTO [dbo].[Addresses] (
       [Address_Line_1]
      ,[City]
      ,[State/Region]
      ,[Postal_Code]
      ,[Domain_ID]
    ) VALUES (
       '1696 Oxford Dr'
      ,'Georgetown'
      ,'KY'
      ,'40324'
      ,1
    );
    SET @address_id = SCOPE_IDENTITY();
  END

  -- assert location
SELECT @location_ID = Location_ID FROM [dbo].[Locations]
 WHERE [Location_Name] = @name;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting location for '+@name;
    INSERT INTO [dbo].[Locations] (
       [Location_Name]
      ,[Domain_ID]
      ,[Location_Type_ID]
      ,[Address_ID]
    ) VALUES (
       @name
      ,1
      ,1
      ,@address_id
    );
    SET @location_id = SCOPE_IDENTITY();
  END

  -- assert congregation
SELECT @congregation_id = Congregation_ID FROM [dbo].[Congregations]
 WHERE [Location_ID] = @location_id;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting congregation for '+@name;
	  SET @startdate = '20170101';
    INSERT INTO [dbo].[Congregations] (
       [Congregation_Name]
      ,[Location_ID]
      ,[Start_Date]
      ,[Accounting_Company_ID]
      ,[Contact_ID]
      ,[Pastor]
      ,[Domain_ID]
    ) VALUES (
       @name
      ,@location_id
      ,@startdate
      ,1
      ,7
      ,@pastor_id
      ,1
    );
    SET @congregation_id = SCOPE_IDENTITY();
  END

  -- assert building
SELECT @building_id = Building_ID FROM [dbo].[Buildings]
 WHERE [Location_ID] = @location_id;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting building for '+@name;
    INSERT INTO [dbo].[Buildings] (
       [Building_Name]
      ,[Location_ID]
      ,[Domain_ID]
    ) VALUES (
       @name
      ,@location_id
      ,1
    );
    SET @building_id = SCOPE_IDENTITY();
  END

END TRY
BEGIN CATCH
  PRINT '   Rolling back transaction due to error: ' + ERROR_MESSAGE();
  if @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
  BEGIN
    PRINT '  committing '+@name;
    COMMIT TRANSACTION;
  END


/* Assert RichmondKY (pastor, location, congregation, building) */
BEGIN TRANSACTION;
BEGIN TRY

SET @name = 'Richmond';
PRINT @name;

  -- assert pastor
SELECT @pastor_id = User_ID FROM [dbo].[dp_Users]
 WHERE [User_Email] = 'dreichley@xroadschurch.org';
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting pastor for '+@name;
    INSERT INTO [dbo].[Contacts] (
       [First_Name]
      ,[Last_Name]
      ,[Company]
      ,[Display_Name]
      ,[Contact_Status_ID]
      ,[Email_Address]
      ,[Bulk_Email_Opt_Out]
      ,[Bulk_SMS_Opt_Out]
      ,[Contact_GUID]
      ,[Domain_ID]
    ) VALUES (
       'David'
      ,'Reichley'
      ,0
      ,'Reichley, David'
      ,1
      ,'dreichley@xroadschurch.org'
      ,0
      ,0
      ,NEWID()
      ,1
    );
    SET @contact_id = SCOPE_IDENTITY();
    INSERT INTO [dbo].[dp_Users] (
       [User_Name]
      ,[User_Email]
      ,[Admin]
      ,[Domain_ID]
      ,[Publications_Manager]
      ,[Contact_ID]
      ,[User_GUID]
      ,[Keep_For_Go_Live]
      ,[Read_Permitted]
      ,[Create_Permitted]
      ,[Update_Permitted]
      ,[Delete_Permitted]
    ) VALUES (
       'dreichley@xroadschurch.org'
      ,'dreichley@xroadschurch.org'
      ,0
      ,1
      ,0
      ,@contact_id
      ,NEWID()
      ,1
      ,1
      ,1
      ,1
      ,1
    );
    SET @pastor_id = SCOPE_IDENTITY();
  END

  -- address
SELECT @address_id = Address_ID FROM [dbo].[Addresses]
 WHERE [Address_Line_1] = '124 South Keeneland Dr'
   AND [City] = 'Richmond'
   AND [State/Region] = 'KY'
   AND [Postal_Code] = '40475';
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting address for '+@name;
    INSERT INTO [dbo].[Addresses] (
       [Address_Line_1]
      ,[City]
      ,[State/Region]
      ,[Postal_Code]
      ,[Domain_ID]
    ) VALUES (
       '124 South Keeneland Dr'
      ,'Richmond'
      ,'KY'
      ,'40475'
      ,1
    );
    SET @address_id = SCOPE_IDENTITY();
  END

  -- assert location
SELECT @location_ID = Location_ID FROM [dbo].[Locations]
 WHERE [Location_Name] = @name;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting location for '+@name;
    INSERT INTO [dbo].[Locations] (
       [Location_Name]
      ,[Domain_ID]
      ,[Location_Type_ID]
      ,[Address_ID]
    ) VALUES (
       @name
      ,1
      ,1
      ,@address_id
    );
    SET @location_id = SCOPE_IDENTITY();
  END

  -- assert congregation
SELECT @congregation_id = Congregation_ID FROM [dbo].[Congregations]
 WHERE [Location_ID] = @location_id;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting congregation for '+@name;
	  SET @startdate = '20170101';
    INSERT INTO [dbo].[Congregations] (
       [Congregation_Name]
      ,[Location_ID]
      ,[Start_Date]
      ,[Accounting_Company_ID]
      ,[Contact_ID]
      ,[Pastor]
      ,[Domain_ID]
    ) VALUES (
       @name
      ,@location_id
      ,@startdate
      ,1
      ,7
      ,@pastor_id
      ,1
    );
    SET @congregation_id = SCOPE_IDENTITY();
  END

  -- assert building
SELECT @building_id = Building_ID FROM [dbo].[Buildings]
 WHERE [Location_ID] = @location_id;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting building for '+@name;
    INSERT INTO [dbo].[Buildings] (
       [Building_Name]
      ,[Location_ID]
      ,[Domain_ID]
    ) VALUES (
       @name
      ,@location_id
      ,1
    );
    SET @building_id = SCOPE_IDENTITY();
  END

END TRY
BEGIN CATCH
  PRINT '   Rolling back transaction due to error: ' + ERROR_MESSAGE();
  if @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
  BEGIN
    PRINT '  committing '+@name;
    COMMIT TRANSACTION;
  END


/* Assert LexingtonKY (pastor, location, congregation, building) */
BEGIN TRANSACTION;
BEGIN TRY

SET @name = 'Downtown';
PRINT @name;

  -- assert pastor
SELECT @pastor_id = User_ID FROM [dbo].[dp_Users]
 WHERE [User_Email] = 'britome@crossroads.net';
IF @@ROWCOUNT = 0
  THROW 10000, 'pastor not found', 1;

  -- address
SELECT @address_id = Address_ID FROM [dbo].[Addresses]
 WHERE [Address_Line_1] = '811 Brian Ave'
   AND [City] = 'Lexington'
   AND [State/Region] = 'KY'
   AND [Postal_Code] = '40505';
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting address for '+@name;
    INSERT INTO [dbo].[Addresses] (
       [Address_Line_1]
      ,[City]
      ,[State/Region]
      ,[Postal_Code]
      ,[Domain_ID]
    ) VALUES (
       '811 Brian Ave'
      ,'Lexington'
      ,'KY'
      ,'40505'
      ,1
    );
    SET @address_id = SCOPE_IDENTITY();
  END

  -- assert location
SELECT @location_ID = Location_ID FROM [dbo].[Locations]
 WHERE [Location_Name] = @name;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting location for '+@name;
    INSERT INTO [dbo].[Locations] (
       [Location_Name]
      ,[Domain_ID]
      ,[Location_Type_ID]
      ,[Address_ID]
    ) VALUES (
       @name
      ,1
      ,1
      ,@address_id
    );
    SET @location_id = SCOPE_IDENTITY();
  END

  -- assert congregation
SELECT @congregation_id = Congregation_ID FROM [dbo].[Congregations]
 WHERE [Location_ID] = @location_id;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting congregation for '+@name;
	  SET @startdate = '20170101';
    INSERT INTO [dbo].[Congregations] (
       [Congregation_Name]
      ,[Location_ID]
      ,[Start_Date]
      ,[Accounting_Company_ID]
      ,[Contact_ID]
      ,[Pastor]
      ,[Domain_ID]
    ) VALUES (
       @name
      ,@location_id
      ,@startdate
      ,1
      ,7
      ,@pastor_id
      ,1
    );
    SET @congregation_id = SCOPE_IDENTITY();
  END

  -- assert building
SELECT @building_id = Building_ID FROM [dbo].[Buildings]
 WHERE [Location_ID] = @location_id;
IF @@ROWCOUNT = 0
  BEGIN
    PRINT '    inserting building for '+@name;
    INSERT INTO [dbo].[Buildings] (
       [Building_Name]
      ,[Location_ID]
      ,[Domain_ID]
    ) VALUES (
       @name
      ,@location_id
      ,1
    );
    SET @building_id = SCOPE_IDENTITY();
  END

END TRY
BEGIN CATCH
  PRINT '   Rolling back transaction due to error: ' + ERROR_MESSAGE();
  if @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
  BEGIN
    PRINT '  committing '+@name;
    COMMIT TRANSACTION;
  END
