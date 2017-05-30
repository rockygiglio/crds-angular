USE MinistryPlatform
GO

DECLARE @ID int
DECLARE IDs CURSOR LOCAL FOR Select MedicalInformation_ID from [dbo].[cr_Medical_Information] WHERE [Allowed_To_Administer_Medications] is not null

OPEN IDs
FETCH NEXT FROM IDs into @ID
WHILE @@FETCH_STATUS = 0
BEGIN	
	
	DECLARE @LIST NVARCHAR(400)
    SELECT @LIST = Allowed_To_Administer_Medications FROM [dbo].[cr_Medical_Information] WHERE MedicalInformation_ID = @ID
	
	select * INTO #Temp from dbo.fnSplitString(@LIST,',')
	DECLARE @UPDATED nvarchar(400) = N''
	DECLARE @MED nvarchar(50)
	DECLARE MEDs CURSOR LOCAL FOR select * from #Temp
	OPEN MEDs
	FETCH NEXT FROM MEDs into @MED
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @UPDATED += UPPER(LEFT(@MED,1))+LOWER(SUBSTRING(@MED,2,LEN(@MED))) + ','
		FETCH NEXT FROM MEDs into @MED
	END
	UPDATE [dbo].[cr_Medical_Information] SET [Allowed_To_Administer_Medications] = @UPDATED WHERE [MedicalInformation_ID] = @ID
	CLOSE MEDs
	DEAllOCATE MEDs
	DROP TABLE #Temp
    FETCH NEXT FROM IDs into @ID
END

CLOSE IDs
DEALLOCATE IDs





