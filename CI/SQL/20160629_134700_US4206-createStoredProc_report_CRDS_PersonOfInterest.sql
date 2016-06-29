USE [MinistryPlatform]
GO

/****** Object:  StoredProcedure [dbo].[report_CRDS_PersonOfInterest]    Script Date: 6/29/2016 1:17:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[report_CRDS_PersonOfInterest]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[report_CRDS_PersonOfInterest]

	@DomainID varchar(40)
	,@UserID varchar(40)
	,@PageID Int
	,@SelectionID Int
	,@Greet Int --(0=Household_Name, 1=Individual Informal, 2=Individual formal, 3=The Last Name Family, FUTURE 4=Combined Informal, 5=Combined Formal
	,@NoEmail Bit = 0
	,@Sort Int = NULL
	,@OmitBulkMailOptOut BIT = 0

AS
BEGIN

SET NOCOUNT ON
SET FMTONLY OFF

DECLARE @EmailString Varchar(1)
SET @EmailString = CASE WHEN @NoEmail = 1 THEN ''/'' ELSE ''@'' END

DECLARE @ContactsPageID Int
SET @ContactsPageID = (SELECT top 1 Page_ID FROM dp_Pages WHERE Table_Name = ''Contacts'')

DECLARE @BaseURL Varchar(100)
SET @BaseURL = (SELECT Top 1 CS.Value FROM dp_Configuration_Settings  CS INNER JOIN dp_Domains Dom on Dom.Domain_ID = CS.Domain_ID AND Dom.Domain_GUID = @DomainID WHERE CS.Application_Code = ''COMMON'' AND CS.Key_Name = ''ImageURL'') 

DECLARE @ImageNotSupported Varchar(254)
SET @ImageNotSupported = (SELECT Top 1 CS.Value + ''images/mpUnknownImage.png'' FROM dp_Configuration_Settings  CS INNER JOIN dp_Domains Dom on Dom.Domain_ID = CS.Domain_ID AND Dom.Domain_GUID = @DomainID WHERE CS.Application_Code = ''COMMON'' AND CS.Key_Name = ''ThinkMinistryRepository'') 


CREATE TABLE #SelectedContacts (Contact_ID int)
INSERT INTO #SelectedContacts 
/*if UserID is Varchar from User_GUID in report SP use...*/
EXEC dp_DeriveSelectedContacts @SelectionID, @PageID, NULL, @UserID
/*if UserID is INT from API sp use...*/
--EXEC dp_DeriveSelectedContacts @SelectionID, @PageID, @UserID, NULL
CREATE INDEX IX_TempSelectedContacts_ContactID ON #SelectedContacts(Contact_ID)

SELECT     C.Display_Name
, ISNULL(ISNULL(P.Prefix + '' '', '''') + C.First_Name + SPACE(1) + C.Last_Name + ISNULL(SPACE(1) + S.Suffix, ''''), C.Display_Name) AS Formal_Mail_Name
, ISNULL(ISNULL(C.Nickname, C.First_Name) + SPACE(1) + C.Last_Name, C.Display_Name) AS Informal_Mail_Name
, Print_Spouse_Informal = ''PSI''--, COALESCE (C.Nickname, C.First_Name) + ISNULL('' & '' + COALESCE (Sp.Nickname, Sp.First_Name), '''') + SPACE(1) + C.Last_Name
, Print_Spouse_Formal = ''PSF''-- LTRIM(CASE WHEN C.Prefix_ID IS NULL AND Sp.Prefix_ID IS NULL AND Sp.Contact_ID > 0 THEN ''Mr. & Mrs.'' ELSE ISNULL(P.Prefix, CASE WHEN C.Gender_ID = 1 THEN ''Mr.'' WHEN C.Gender_ID IS NULL AND SP.Gender_ID = 2 THEN ''Mr.'' WHEN C.Gender_ID IS NULL AND C.Marital_Status_ID IS NULL THEN '''' WHEN MS.Marital_Status = ''Single'' THEN ''Miss'' WHEN MS.Marital_Status = ''Married'' THEN ''Mrs.'' WHEN MS.Marital_Status IS NULL THEN '''' ELSE ''Ms.'' END) + CASE WHEN Sp.Contact_ID > 0 THEN ISNULL('' & '' + Spp.Prefix, '' & '' + CASE WHEN Sp.Gender_ID = 1 THEN ''Mr.'' WHEN SP.Gender_ID IS NULL AND C.Gender_ID = 1 THEN ''Mrs.'' WHEN Sp.Gender_ID IS NULL THEN '''' WHEN SPMS.Marital_Status = ''Single'' THEN ''Miss'' WHEN SPMS.Marital_Status = ''Married'' THEN ''Mrs.'' WHEN SPMS.Marital_Status IS NULL  THEN '''' ELSE ''Ms.'' END) ELSE '''' END END + ISNULL(SPACE(1) + CASE C.Gender_ID WHEN 1 THEN C.First_Name WHEN 2 THEN ISNULL(SP.First_Name, C.First_Name) ELSE C.First_Name END, '''') + SPACE(1) + C.Last_Name + ISNULL(SPACE(1) + CASE C.Gender_ID WHEN 1 THEN S.Suffix WHEN 2 THEN ISNULL(SPS.Suffix, S.Suffix) ELSE SPS.Suffix END, ''''))
, Greeting = CASE @Greet WHEN 0 THEN ISNULL(H.Household_Name,C.Display_Name) WHEN 1 THEN ISNULL(ISNULL(C.Nickname, C.First_Name) + SPACE(1) + C.Last_Name, C.Display_Name) WHEN 2 THEN ISNULL(ISNULL(P.Prefix + '' '', '''') + C.First_Name + SPACE(1) + C.Last_Name + ISNULL(SPACE(1) + S.Suffix, ''''), C.Display_Name) WHEN 3 THEN ''The '' + C.Last_Name + '' Family'' ELSE C.Display_Name END
, P.Prefix
, C.First_Name
, C.Middle_Name
, C.Last_Name
, S.Suffix
, C.Nickname
, C.Date_of_Birth
, CS.Contact_Status
, ISNULL(A.Address_Line_1,'''') AS Address_Line_1
, ISNULL(A.Address_Line_2,'''') AS Address_Line_2
, A.City
, A.[State/Region]
, A.Postal_Code
, CASE A.Foreign_Country WHEN ''USA'' THEN NULL WHEN ''US'' THEN NULL WHEN ''United States'' THEN NULL ELSE UPPER(A.Foreign_Country) END AS Foreign_Country
, ISNULL(A.City + '', '','''') + ISNULL(A.[State/Region] + SPACE(1),'''') + ISNULL(A.Postal_Code,'''')/* + ISNULL(Char(13) + Char(10) + SPACE(3) + CASE A.Foreign_Country WHEN ''USA'' THEN NULL WHEN ''US'' THEN NULL WHEN ''United States'' THEN NULL ELSE UPPER(A.Foreign_Country) END,'''') */ AS City_State_Zip
, C.Email_Address
, H.Home_Phone
, C.Mobile_Phone
, C.Company_Phone
, C.Pager_Phone
, C.Fax_Phone
, ISNULL(MS.Marital_Status,''*Unknown Marital'') AS Marital_Status
, C.Gender_ID
, C.Household_Position_ID
, C.Marital_Status_ID
, ISNULL(G.Gender,''*Unknown Gender'') AS Gender
, C.Contact_ID
, C.Participant_Record
, C.Household_ID
, H.Household_Name
, Image_File = CASE WHEN dp_Files.Extension LIKE ''tif%'' THEN @ImageNotSupported 
					ELSE  @BaseURL + ''?dn='' + Convert(Varchar(40),Dom.Domain_GUID) + ''&fn='' + Convert(Varchar(40), dp_Files.Unique_Name) + ''.'' + dp_Files.Extension  
					END
, C.Domain_ID
, CASE WHEN @Sort IS NULL THEN C.Display_Name WHEN @Sort = 1 THEN ISNULL(A.Postal_Code,''00000'') ELSE C.Display_Name END AS Postal_Sort
, C.__Age AS Age
, PT.Participant_Type
, ISNULL(Cong.Congregation_Name ,''*No Congregation'') AS Congregation_Name
, Age_Group = ISNULL(CASE WHEN __Age <= 120 THEN Convert(Varchar(3),ROUND(C.__Age,-1,1)) + ''''''s to '' + Convert(Varchar(3),ROUND(C.__Age,-1,1)+9) + ''''''s'' ELSE NULL END,''*Unknown Age'')
, HP.Household_Position
FROM dbo.Contacts AS C
 INNER JOIN dbo.dp_Domains Dom ON Dom.Domain_ID = C.Domain_ID
 INNER JOIN dbo.Contact_Statuses AS CS WITH(NoLock) ON CS.Contact_Status_ID = C.Contact_Status_ID
 LEFT OUTER JOIN dbo.Participants Part ON Part.Contact_ID = C.Contact_ID
 LEFT OUTER JOIN dbo.Participant_Types PT ON PT.Participant_Type_ID = Part.Participant_Type_ID
 LEFT OUTER JOIN dbo.Households AS H WITH(NoLock) ON H.Household_ID = C.Household_ID
 LEFT OUTER JOIN dbo.Congregations Cong ON Cong.Congregation_ID = H.Congregation_ID
 LEFT OUTER JOIN dbo.Suffixes AS S WITH(NoLock) ON S.Suffix_ID = C.Suffix_ID
 LEFT OUTER JOIN dbo.Prefixes AS P WITH(NoLock) ON P.Prefix_ID = C.Prefix_ID
 LEFT OUTER JOIN dbo.Genders AS G WITH(NoLock) ON G.Gender_ID = C.Gender_ID
 LEFT OUTER JOIN dbo.Marital_Statuses AS MS WITH(NoLock) ON MS.Marital_Status_ID = C.Marital_Status_ID
 LEFT OUTER JOIN dbo.Household_Positions HP ON HP.Household_Position_ID = C.Household_Position_ID
 LEFT OUTER JOIN dbo.Addresses AS A WITH(NoLock) ON A.Address_ID = H.Address_ID
 LEFT OUTER JOIN dbo.dp_Files WITH(NoLock) ON dbo.dp_Files.Record_ID = C.Participant_Record AND dbo.dp_Files.Table_Name = ''Participants'' AND dbo.dp_Files.Default_Image = 1

WHERE 
 EXISTS (SELECT 1 FROM #SelectedContacts SC WHERE SC.Contact_ID = C.Contact_ID) 
 AND (Email_Address NOT LIKE ''%@%'' OR ISNULL(C.Email_Address,@EmailString) LIKE ''%''+@EmailString+''%'')
 AND (@OmitBulkMailOptOut = 0 OR H.Bulk_Mail_Opt_Out = 0)
ORDER BY Postal_Sort

END



' 
END
GO


