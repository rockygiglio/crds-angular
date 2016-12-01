USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF EXISTS ( SELECT * FROM   sysobjects WHERE  id = object_id(N'[dbo].[api_crds_Create_Medical_Info_For_Contacts]') and OBJECTPROPERTY(id, N'IsProcedure') = 1 )
BEGIN
    DROP PROCEDURE [dbo].[api_crds_Create_Medical_Info_For_Contacts]
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

	INSERT INTO cr_Medical_Information(InsuranceCompany,PolicyHolderName,PhysicianName,PhysicianPhone,Domain_ID)
		VALUES(@InsuranceCompany,@PolicyHolder,@PhysicianName,@PhysicianPhone,1)
	
	DECLARE @MedInfoId INT = SCOPE_IDENTITY()

	UPDATE Contacts SET MedicalInformation_ID = @MedInfoId WHERE Contact_ID = @ContactId;

END

GO

