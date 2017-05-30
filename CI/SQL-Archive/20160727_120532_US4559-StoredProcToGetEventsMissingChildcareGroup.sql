USE [MinistryPlatform]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Andy Canterbury <andrew.canterbury@ingagepartners.com>
-- Create date: 7/27/2016
-- Description:	Get data needed to add the childcare group to childcare events that don't have one
-- =============================================
CREATE PROCEDURE api_crds_MissingChildcareGroup 
	-- Add the parameters for the stored procedure here
	@Group_Type int,
	@Event_type int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @GoodEvents Table ( Event_ID int, Group_ID int, Sequence_ID int );

	INSERT INTO @GoodEvents
	SELECT e.Event_ID, eg.Group_ID , sr.Sequence_ID FROM dbo.Events e
	JOIN dbo.Event_Groups eg ON e.Event_ID = eg.Event_ID
	JOIN dbo.Groups g ON g.Group_ID = eg.Group_ID
	JOIN dbo.dp_Sequence_Records sr ON sr.Record_ID = e.Event_ID AND sr.Table_Name = 'Events'
	WHERE e.Event_Type_ID = @Event_Type AND g.Group_Type_ID = @Group_Type
	AND e.Event_Start_Date >= GETDATE()

	SELECT e.Event_ID, ge.Group_ID  FROM dbo.Events e
	JOIN dbo.dp_Sequence_Records sr ON sr.Record_ID = e.Event_ID AND sr.Table_Name = 'Events'
	JOIN @GoodEvents ge ON ge.Sequence_ID = sr.Sequence_ID
	WHERE e.Event_Type_ID = @Event_Type AND e.Event_ID NOT IN (SELECT Event_ID FROM @GoodEvents)
	AND e.Event_Start_Date >= GETDATE()
END
GO