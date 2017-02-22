USE [MinistryPlatform]
GO

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE Table_Schema = N'dbo' AND Table_Name=N'cr_Host_Statuses')
BEGIN
  CREATE TABLE [dbo].[cr_Host_Statuses](
    [Host_Status_ID] [int] IDENTITY(1,1) NOT NULL,
    [Description] [nvarchar](75) NOT NULL,
    [Domain_ID] [int] NOT NULL,
    [Sort_Order] [int] NOT NULL,
	CONSTRAINT PK_Host_Statuses_Host_Status_ID PRIMARY KEY (Host_Status_ID),
	CONSTRAINT [FK_Domain_ID] FOREIGN KEY ([Domain_ID]) REFERENCES dbo.dp_Domains(Domain_ID)
  )
  
  SET IDENTITY_INSERT [dbo].[cr_Host_Statuses] ON;
  INSERT INTO [dbo].[cr_Host_Statuses] (Host_Status_ID, Description, Domain_ID, Sort_Order)
    VALUES(0, 'Not Applied', 1, 100),
          (1, 'Pending',     1, 200),
          (2, 'Unapproved',  1, 300),
          (3, 'Approved',    1, 400);
  SET IDENTITY_INSERT [dbo].[cr_Host_Statuses] OFF;
END

  IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES  WHERE TABLE_NAME = N'Participants')
   AND NOT EXISTS(select top 1 * from sys.columns  WHERE Name = N'Host_Status_ID' AND Object_ID = Object_ID(N'Participants'))
  BEGIN
    ALTER TABLE [dbo].[Participants]
    ADD [Host_Status_ID] INT NOT NULL DEFAULT(0) FOREIGN KEY REFERENCES dbo.cr_Host_Statuses(Host_Status_ID)
  END

  IF NOT EXISTS (SELECT * FROM [dbo].[dp_Pages]  WHERE Page_ID = 625)
    BEGIN
	SET IDENTITY_INSERT [dbo].[dp_Pages] ON;
    INSERT INTO [dbo].[dp_Pages]
      (Page_ID, Display_Name, Singular_Name, [Description], View_Order, Table_Name, Primary_Key, Display_Search, Default_Field_List, Selected_Record_Expression, Filter_Clause
   	  , Start_Date_Field, End_Date_Field, Contact_ID_Field, Default_View, Pick_List_View, Image_Name, Direct_Delete_Only, System_Name, Date_Pivot_Field, Custom_Form_Name, Display_Copy
 	  )
    VALUES
      (625
	  , 'Host Statuses'
	  , 'Host Status'
	  , 'The status of the connect host approval process'
	  , 99
	  , 'cr_Host_Statuses'
	  , 'Host_Status_ID'
	  , 1
	  , 'Host_Status_ID, Description, Sort_Order'
	  , 'Description'
	  , NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
	  , 1)
    SET IDENTITY_INSERT [dbo].[dp_Pages] OFF

    INSERT INTO [dbo].[dp_Page_Section_Pages] (Page_ID, Page_Section_ID)
    VALUES (625, 4)
  END
