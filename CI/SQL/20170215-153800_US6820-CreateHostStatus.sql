USE [MinistryPlatform]
GO

-- if the table doesn't exists, we need to do the work
IF NOT EXISTS (SELECT *
     FROM INFORMATION_SCHEMA.TABLES
     WHERE Table_Schema = 'dbo'
     AND Table_Name='cr_Host_Statuses')
BEGIN
  -- create new lookup table
  CREATE TABLE [dbo].[cr_Host_Statuses](
    [Host_Status_ID] [int] IDENTITY(1,1) NOT NULL,
    [Display_Name] [nvarchar](32),
    [Domain_ID] [int] NOT NULL,
    [Sorting] [int] NOT NULL,
    CONSTRAINT PK_Host_Statuses_Host_Status_ID PRIMARY KEY (Host_Status_ID),
    CONSTRAINT FK_Domain_ID FOREIGN KEY (Domain_ID) References dbo.cr_Host_Statuses
  );

  -- add the lookup values
  SET IDENTITY_INSERT [dbo].[cr_Host_Statuses] ON;
  INSERT INTO [dbo].[cr_Host_Statuses] (Host_Status_ID, Display_Name, Domain_ID, Sorting)
    VALUES(0, 'Not Applied',  1, 100),
          (1, 'Pending',      1, 200),
          (2, 'Unapproved',   1, 300),
          (3, 'Approved',     1, 400);
  SET IDENTITY_INSERT [dbo].[cr_Host_Statuses] OFF;

  -- fasten it to the MP undercarriage
  SET IDENTITY_INSERT [dbo].[dp_Pages] ON;
  INSERT INTO [dbo].[dp_Pages]
    (Page_ID, Display_Name, Singular_Name, Description, View_Order, Table_Name, Primary_Key, Display_Search, Default_Field_List, Selected_Record_Expression, Filter_Clause, Start_Date_Field, End_Date_Field, Contact_ID_Field, Default_View, Pick_List_View, Image_Name, Direct_Delete_Only, System_Name, Date_Pivot_Field, Custom_Form_Name, Display_Copy)
  VALUES
    (625, 'Host Statuses', 'Host Status', 'The status of the connect host approval process', 99, 'cr_Host_Statuses', 'Host_Status_ID', 1, 'Host_Status_ID, Display_Name, Sorting', 'Host_Status_ID', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1);
  SET IDENTITY_INSERT [dbo].[dp_Pages] OFF;

  INSERT INTO [dbo].[dp_Page_Section_Pages] (Page_ID, Page_Section_ID, User_ID)
  VALUES (625, 4, NULL);

  -- get rid of old column
  --IF COL_LENGTH('Participants','Approved_Host') IS NOT NULL
  --BEGIN
  --  ALTER TABLE [dbo].[Participants] DROP CONSTRAINT [DF__Participa__Appro__393C81E6];
  --  ALTER TABLE [dbo].[Participants] DROP COLUMN [Approved_Host];
  --END
  -- Approved_Host is not being removed because it already had other uses. Once we figure
  -- out what should really be done, we'll do it.

  -- add new column, with 'Not Applied' as the default
  IF COL_LENGTH('Participants','Host_Status_ID') IS NULL
  BEGIN
    ALTER TABLE [dbo].[Participants]
      ADD [Host_Status_ID] [int] DEFAULT 0;
    ALTER TABLE [dbo].[Participants]
      ADD CONSTRAINT FK_Host_Status FOREIGN KEY (Host_Status_ID) References dbo.cr_Host_Statuses;
  END
END
