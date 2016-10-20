USE [MinistryPlatform]
GO


IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='cr_Quick_Donation_Amounts' and xtype='U')
	BEGIN
		CREATE TABLE cr_Quick_Donation_Amounts
		(
			ID INT NOT NULL IDENTITY(1,1),
			Amount INT NOT NULL,
			Domain_ID INT NOT NULL,
			PRIMARY KEY (ID),
			FOREIGN KEY (Domain_ID) REFERENCES dp_Domains(Domain_ID)
		);

		INSERT INTO cr_Quick_Donation_Amounts (Amount, Domain_ID)
		VALUES (5, 1), (10, 1), (25, 1), (50, 1), (100, 1), (500, 1)
	END
GO

IF EXISTS ( SELECT * FROM sysobjects WHERE NAME = 'api_Get_Quick_Donation_Amounts' AND type = 'P')
     DROP PROCEDURE api_Get_Quick_Donation_Amounts
GO

CREATE PROCEDURE api_Get_Quick_Donation_Amounts
AS
SELECT Amount from [cr_Quick_Donation_Amounts]