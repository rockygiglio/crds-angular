USE [MinistryPlatform]
GO


IF EXISTS ( SELECT * FROM  sys.objects WHERE object_id = OBJECT_ID(N'api_crds_Create_Medical_Info_For_Contacts') AND type IN ( N'P', N'PC' ) ) 
BEGIN
	DROP PROCEDURE api_crds_Create_Medical_Info_For_Contacts
END
GO

-- =============================================
-- Author:		Phil Lachmann
-- Create date: 10/25/2016
-- Description:	Create a Medical Information Record and associate it with a Contact
--				 
-- =============================================
CREATE PROCEDURE [dbo].[api_crds_Create_Medical_Info_For_Contacts] 
	-- Add the parameters for the stored procedure here
	@ContactId INT,
	@InsuranceCompany NVARCHAR(256),
	@PolicyHolder NVARCHAR(256),
	@PhysicianName NVARCHAR(256),
	@PhysicianPhone NVARCHAR(256)

AS
BEGIN
	
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @MedInfoId INT
	SET @MedInfoId = (SELECt medicalinformation_id FROM contacts WHERE contact_id = @ContactId)

	IF (@MedInfoId IS NULL)
	BEGIN
		INSERT INTO cr_Medical_Information(InsuranceCompany,PolicyHolderName,PhysicianName,PhysicianPhone,Domain_ID)
			VALUES(@InsuranceCompany,@PolicyHolder,@PhysicianName,@PhysicianPhone,1)
	
		SET @MedInfoId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE cr_Medical_Information SET InsuranceCompany = @InsuranceCompany, PolicyHolderName = @PolicyHolder, PhysicianName = @PhysicianName, PhysicianPhone = @PhysicianPhone
		  WHERE medicalinformation_id = @MedInfoId
	END 

	UPDATE Contacts SET MedicalInformation_ID = @MedInfoId WHERE Contact_ID = @ContactId;

	
END


GO


