USE MinistryPlatform 
GO

--First Pass, set congregation using one from household
UPDATE pd
SET pd.Congregation_ID = h.Congregation_ID
FROM dbo.Payment_Detail pd
JOIN dbo.Payments p on p.Payment_ID = pd.Payment_ID
JOIN dbo.Contacts c on c.Contact_ID = p.Contact_ID
JOIN dbo.Households h on c.Household_ID = h.Household_ID
WHERE pd.Congregation_ID IS NULL

--Second pass, for the ones that are still null, set them as Not Site Specific
UPDATE dbo.Payment_Detail
SET Congregation_ID = 5
WHERE Congregation_ID IS NULL