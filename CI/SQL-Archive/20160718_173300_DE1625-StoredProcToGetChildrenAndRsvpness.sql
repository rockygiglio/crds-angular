USE [MinistryPlatform]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_ChildRsvpd]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[api_crds_ChildRsvpd]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[api_crds_ChildRsvpd]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[api_crds_ChildRsvpd] AS' 
END
GO


ALTER PROCEDURE [dbo].[api_crds_ChildRsvpd]
	-- Add the parameters for the stored procedure here
	@ContactID int,
	@EventGroupID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @ChildcareGroupType int = 27;
	DECLARE @ParticipantID int;
	SELECT @ParticipantID = Participant_ID FROM Participants where Contact_ID = @ContactID;

	SELECT 1 as 'Rsvpd' FROM dbo.Group_Participants g
	JOIN dbo.Groups groups on g.Group_ID = groups.Group_ID
	WHERE g.Participant_ID = @ParticipantID 
	AND groups.Group_Type_ID = @ChildcareGroupType
	AND (g.End_Date is null OR g.End_Date > getDate())
	AND (groups.End_Date is null OR groups.End_Date > getDate())
	AND groups.Group_ID = @EventGroupID
END
GO


