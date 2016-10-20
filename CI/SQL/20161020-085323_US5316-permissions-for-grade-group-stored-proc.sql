USE [MinistryPlatform]
GO

DECLARE @PROCNAME NVARCHAR(128) = N'api_crds_Grade_Group_For_Camps'; 

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_API_Procedures] WHERE [Procedure_Name] = @PROCNAME)
BEGIN
	INSERT INTO [dbo].[dp_API_Procedures]
			   ([Procedure_Name]
			   ,[Description])
		 VALUES
			   (@PROCNAME
			   ,N'Gets grade groups for camps open for registration')
END

DECLARE @APIROLE int = 62;
DECLARE @PROCID int;
SELECT @PROCID = p.API_Procedure_ID FROM [dbo].[dp_API_Procedures] p WHERE p.Procedure_Name = @PROCNAME;

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Role_API_Procedures] ap WHERE ap.[API_Procedure_ID] = @PROCID AND ap.Role_ID = @APIROLE)
BEGIN
	INSERT INTO [dbo].[dp_Role_API_Procedures]
				([Role_ID]
				,[API_Procedure_ID]
				,[Domain_ID])
		VALUES
				(@APIROLE
				,@PROCID
				,1)
END