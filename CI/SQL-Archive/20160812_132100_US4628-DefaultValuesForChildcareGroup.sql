USE MinistryPlatform
GO

DECLARE @AppCode NVARCHAR(32) = 'COMMON'

IF NOT EXISTS(SELECT * FROM dp_Configuration_Settings WHERE Application_Code = @AppCode AND Key_Name = 'ChildcareMaxAge')
BEGIN
 INSERT INTO dp_Configuration_Settings(Application_Code,Key_Name,Value,Description,Domain_ID)
	VALUES(@AppCode,'ChildcareMaxAge','10','Default Maximum Age to use when creating groups of type Childcare',1)
END

IF NOT EXISTS(SELECT * FROM dp_Configuration_Settings WHERE Application_Code = @AppCode AND Key_Name = 'ChildcareMinParticipants')
BEGIN
 INSERT INTO dp_Configuration_Settings(Application_Code,Key_Name,Value,Description,Domain_ID)
	VALUES(@AppCode,'ChildcareMinParticipants','25','Default Minimum Participants to use when creating groups of type Childcare',1)
END

IF NOT EXISTS(SELECT * FROM dp_Configuration_Settings WHERE Application_Code = @AppCode AND Key_Name = 'ChildcareTargetSize')
BEGIN
 INSERT INTO dp_Configuration_Settings(Application_Code,Key_Name,Value,Description,Domain_ID)
	VALUES(@AppCode,'ChildcareTargetSize','100','Default Target Size to use when creating groups of type Childcare',1)
END

GO
