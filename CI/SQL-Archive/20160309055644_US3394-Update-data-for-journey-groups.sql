USE MinistryPlatform;
GO 

UPDATE Groups
SET Meeting_Day_ID = NULL
WHERE Group_Type_ID = 19
AND (End_Date > GETDATE() OR End_Date IS NULL)
AND Meeting_Day_ID = 1
AND Offsite_Meeting_Address IS NULL
AND Meeting_Time IS NULL
