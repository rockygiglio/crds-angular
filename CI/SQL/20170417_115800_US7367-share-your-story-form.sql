USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Jon Horner
-- Create date: 4/17/2017
-- Description:	Scripts the form fields for the
--		Share Your Story page in the Group
--		Leader Application
-- =============================================

DECLARE @glaFormID INT = 29;
DECLARE @lgTextBoxID INT = 2;
DECLARE @domainID INT = 1;
DECLARE @required INT = 1;

DECLARE @storyFieldId INT = 1522;
DECLARE @taughtFieldId INT = 1523;

IF NOT EXISTS (SELECT 1 FROM Form_Fields WHERE Form_Field_ID = @storyFieldId)
BEGIN
	SET IDENTITY_INSERT Form_Fields ON;

	INSERT INTO Form_Fields
	(
		Form_Field_ID
		, Field_Order
		, Field_Label
		, Field_Type_ID
		, [Required]
		, Form_ID
		, Domain_ID
		, Placement_Required
	)
	VALUES
	(
		@storyFieldId
		, 40
		, 'Spiritual Growth as a Disciple'
		, @lgTextBoxID
		, @required
		, @glaFormID
		, @domainID
		, 0
	);

	SET IDENTITY_INSERT Form_Fields OFF;
END

IF NOT EXISTS (SELECT 1 FROM Form_Fields WHERE Form_Field_ID = @taughtFieldId)
BEGIN
	SET IDENTITY_INSERT Form_Fields ON;

	INSERT INTO Form_Fields
	(
		Form_Field_ID
		, Field_Order
		, Field_Label
		, Field_Type_ID
		, [Required]
		, Form_ID
		, Domain_ID
		, Placement_Required
	)
	VALUES
	(
		@taughtFieldId
		, 50
		, 'What has God Taught You'
		, @lgTextBoxID
		, @required
		, @glaFormID
		, @domainID
		, 0
	);

	SET IDENTITY_INSERT Form_Fields OFF;
END
