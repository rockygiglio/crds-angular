USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[api_crds_CreateContact]    Script Date: 10/11/2016 10:48:49 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[api_crds_CreateContact]
    @FirstName VARCHAR(100),
	@LastName VARCHAR(100),
	@MiddleName VARCHAR(100),
    @PreferredName VARCHAR(100),
    @Birthdate DATE,
	@Gender INT ,
	@HouseholdId INT,
	@HouseholdPosition INT,
	@SchoolAttending VARCHAR(100)
AS
BEGIN
    DECLARE @RecordID INT
    INSERT INTO [dbo].[Contacts] (
	                [Company],
	                [Display_Name],
					[First_Name],
					[Middle_Name],
					[Last_Name],
					[Date_of_Birth],
					[Gender_ID],
					[Household_ID],
					[Household_Position_ID],
					[Current_School],
					[Domain_ID]

				) VALUES (
				    0,
					@PreferredName,
					@FirstName,
					@MiddleName,
					@LastName,
					@Birthdate,
					@Gender,
					@HouseholdId,
					@HouseholdPosition,
					@SchoolAttending,
					1)

	SELECT @RecordID = @@IDENTITY
	SELECT @RecordID AS RecordID
END
GO


