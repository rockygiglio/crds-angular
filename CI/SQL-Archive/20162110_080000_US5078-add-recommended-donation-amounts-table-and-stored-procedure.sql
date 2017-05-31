USE [MinistryPlatform]
GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='cr_Predefined_Donation_Amounts' and xtype='U')
	BEGIN
		CREATE TABLE cr_Predefined_Donation_Amounts
		(
			ID INT NOT NULL IDENTITY(1,1),
			Amount INT NOT NULL,
			Domain_ID INT NOT NULL,
			PRIMARY KEY (ID),
			FOREIGN KEY (Domain_ID) REFERENCES dp_Domains(Domain_ID)
		);

		INSERT INTO cr_Predefined_Donation_Amounts (Amount, Domain_ID)
		VALUES (5, 1), (10, 1), (25, 1), (50, 1), (100, 1), (500, 1)
	END
GO

IF NOT EXISTS (SELECT * FROM dp_Pages WHERE Page_ID=612)
	BEGIN
	    SET IDENTITY_INSERT dp_Pages ON;
        INSERT INTO [MinistryPlatform].[dbo].[dp_Pages] ([Page_ID],[Display_Name],[Singular_Name],[Description],[View_Order]
                                                        ,[Table_Name],[Primary_Key],[Display_Search],[Default_Field_List]
                                                        ,[Selected_Record_Expression],[Filter_Clause],[Start_Date_Field]
                                                        ,[End_Date_Field],[Contact_ID_Field],[Default_View],[Pick_List_View]
                                                        ,[Image_Name],[Direct_Delete_Only],[System_Name],[Date_Pivot_Field]
                                                        ,[Custom_Form_Name],[Display_Copy])
        VALUES (612, 'Predefined Donation Amounts', 'Predefined Donation Amounts', 'A list of recommended donation amounts', 99,
                'cr_Predefined_Donation_Amounts', 'ID', NULL,
                'cr_Predefined_Donation_Amounts.ID, cr_Predefined_Donation_Amounts.Amount, cr_Predefined_Donation_Amounts.Domain_ID',
                'Amount', NULL, NULL, NULL, NULL, NULL,
                NULL, NULL, NULL, NULL, NULL, NULL, 1);

        SET IDENTITY_INSERT dp_Pages OFF;
	END
GO

IF NOT EXISTS (SELECT * FROM dp_Page_Section_Pages WHERE Page_ID=612)
	BEGIN
        INSERT INTO dp_Page_Section_Pages (Page_ID, Page_Section_ID)
        VALUES (612, 9);
	END
GO


