USE [MinistryPlatform]
GO

-- =====================================
-- Author:		Jon Horner
-- Create Date:	5/25/17
-- Description:	Add Reference name field
--		to the group leader form.
-- =====================================

DECLARE @groupLeaderForm INT = 29;
DECLARE @fieldOrder INT = 15; --After the reference contact id
DECLARE @fieldType INT = 1; --Text box type

INSERT INTO Form_Fields
(
	Field_Order
	, Field_Label
	, Field_Type_ID
	, [Required]
	, Form_ID
	, Domain_ID
	, Placement_Required
)
VALUES
(
	@fieldOrder
	, N'Staff Reference Name'
	, @fieldType
	, 1
	, @groupLeaderForm
	, 1
	, 0
);