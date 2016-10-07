USE MinistryPlatform
GO

DECLARE @SectionName NVARCHAR(64) ='Medical & Waivers'
DECLARE @SectionID INT = 22

IF NOT EXISTS (SELECT * FROM dbo.dp_Page_Sections WHERE Page_Section_ID = @SectionID)
BEGIN
	SET IDENTITY_INSERT dbo.dp_Page_Sections ON
	INSERT INTO dp_Page_Sections(Page_Section_ID,Page_Section,View_Order) VALUES(@SectionID, @SectionName, 87)
	SET IDENTITY_INSERT dbo.dp_Page_Sections OFF
END 
GO
