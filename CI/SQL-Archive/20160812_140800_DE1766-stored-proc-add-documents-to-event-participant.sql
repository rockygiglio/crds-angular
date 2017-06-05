USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[crds_AddDocumentsToParticipant]    Script Date: 8/15/2016 11:02:09 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_AddDocumentsToParticipant]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[crds_AddDocumentsToParticipant]
GO

/****** Object:  StoredProcedure [dbo].[crds_AddDocumentsToParticipant]    Script Date: 8/15/2016 11:02:09 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crds_AddDocumentsToParticipant]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[crds_AddDocumentsToParticipant] AS' 
END
GO




ALTER PROCEDURE [dbo].[crds_AddDocumentsToParticipant]
	-- Add the parameters for the stored procedure here
	@Destination_ID int,
	@EventParticipant_ID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CURRENT_DOCUMENT_ID int;
	DECLARE @DOCUMENTS TABLE (
		DocumentID int
	);

	INSERT INTO @DOCUMENTS SELECT Document_ID FROM cr_Document_Destinations WHERE Destination_ID = @Destination_ID

	DECLARE MC CURSOR LOCAL STATIC READ_ONLY FORWARD_ONLY
	FOR 
	SELECT DISTINCT DocumentID from @DOCUMENTS
	
	OPEN MC;
	FETCH NEXT FROM MC INTO @CURRENT_DOCUMENT_ID;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM [dbo].[cr_EventParticipant_Documents] 
					   WHERE Event_Participant_ID = @EventParticipant_ID
					   AND Document_ID = @CURRENT_DOCUMENT_ID)
		BEGIN
			INSERT INTO cr_EventParticipant_Documents (
				Event_Participant_ID,
				Document_ID,
				Received,
				Domain_ID
			) VALUES (
				@EventParticipant_ID,
				@CURRENT_DOCUMENT_ID,
				0,
				1
			)
		END
		FETCH NEXT FROM MC INTO @CURRENT_DOCUMENT_ID;
	END
	CLOSE MC;
	DEALLOCATE MC;
END


GO


