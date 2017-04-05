USE [MinistryPlatform]
GO

CREATE FUNCTION StripNonNumeric(  @String VARCHAR(500) ) 
RETURNS VARCHAR(40)
AS
BEGIN
    DECLARE
          @n INT = 1,
          @Return VARCHAR(100) = ''

    WHILE @n <= LEN(@String)
       BEGIN
          SET @Return = @Return + CASE
                             WHEN ASCII(SUBSTRING(@String, @n, 1)) BETWEEN ASCII('0') AND ASCII('9')
                                THEN SUBSTRING(@String, @n, 1)
                                ELSE ''
                             END
          SET @n = @n + 1
       END

    RETURN CASE
         WHEN @Return = ''
            THEN NULL
            ELSE @Return
         END
END
GO

DECLARE 
 @AuditItemID INT      --SETS to 0
,@UserName Varchar(50) --SETS to 'Svc Mngr'
,@UserID INT			--SETS to 0
,@TableName Varchar(50) --SETS to 'Group_Participants'
,@Update_ID  INT
,@CurrentPhoneNumber NVARCHAR(25)
,@RepairedPhoneNumber NVARCHAR(25)

SET @AuditItemID = 0
SET @UserName = 'Svc Mngr'
SET @UserID = 0


--Update Contacts
CREATE TABLE #AllNumbersToChange (contact_id int,  mobile_phone varchar(40), wip_phone varchar(40), new_phone varchar(40) )
 
INSERT into #AllNumbersToChange (contact_id, mobile_phone, wip_phone)
select contact_id, Mobile_Phone, dbo.StripNonNumeric(Mobile_phone) FROM Contacts
WHERE 
  Mobile_Phone IS NOT NULL AND NOT Mobile_Phone LIKE '[2-9][0-9][0-9]-[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]'

--Create an index
CREATE INDEX wip ON #AllNumbersToChange (wip_phone)

--if it's less than 10 digits or greater than 14 digits or 123456789 or starts with 999, we will delete it.
--new_phone is already set to null so we are good

  
--if it's exactly 11 digits and starts with 0 or 1 pull that off so that now it's 10 and will get formatted
UPDATE #AllNumbersToChange
SET wip_phone = SUBSTRING(wip_phone,2,10)
WHERE LEN(wip_phone) = 11
AND wip_phone like '[01]%'


--if it's great than 10 digits and starts with list of known area codes then trim the end to make it 10
UPDATE #AllNumbersToChange
SET wip_phone = 
    CASE
	   WHEN SUBSTRING(wip_phone,1,3) IN (
	        '215',
			'254',
			'260',
			'262',
			'283',
			'317',
			'325',
			'336',
			'346',
			'386',
			'419',
			'440',
			'502',
			'512',
			'513',
			'515',
			'551',
			'561',
			'606',
			'614',
			'740',
			'760',
			'770',
			'810',
			'812',
			'813',
			'858',
			'859',
			'865',
			'909',
			'928',
			'937',
			'978') 
	     THEN SUBSTRING(wip_phone,1,10)
       ELSE NULL
	END
WHERE LEN(wip_phone) > 10 AND LEN(wip_phone) <= 14

--if it's 10 digits format it unless it's 123465789 or 999, which case delete it
UPDATE #AllNumbersToChange
SET new_phone = 
	CASE 
		WHEN wip_phone LIKE '%123456789%' THEN NULL
		WHEN wip_phone = '0000000000' THEN NULL
		WHEN wip_phone LIKE '999%' THEN NULL
		WHEN wip_phone like '[01]%' THEN NULL
		ELSE SUBSTRING(wip_phone,1,3)+'-'+SUBSTRING(wip_phone,4,3)+'-'+SUBSTRING(wip_phone,7,4)
	END
WHERE LEN(wip_phone) = 10


--For testing/validation only
/*
select contact_id, mobile_phone as original, new_phone as to_update 
from #AllNumbersToChange
*/

-- The real thing

SET @TableName = 'Contacts'

	DECLARE CursorPUTT CURSOR FAST_FORWARD FOR
	SELECT Contact_ID,mobile_phone, new_phone FROM #AllNumbersToChange

	OPEN CursorPUTT
	FETCH NEXT FROM CursorPUTT INTO @Update_ID, @CurrentPhoneNumber, @RepairedPhoneNumber
		WHILE @@FETCH_STATUS = 0
		BEGIN

		
			UPDATE Contacts 
			SET [Mobile_Phone] = @RepairedPhoneNumber 
			WHERE Contact_ID = @Update_ID 		
			
			--Audit Log the Change
            EXEC [dbo].[crds_Add_Audit] 
				 @TableName 
				,@Update_ID
				,'Mass Updated'
				,@UserName
				,@UserID
				,'Mobile_Phone'
				,'Mobile Phone'
				,@CurrentPhoneNumber
				,@RepairedPhoneNumber 
	
			FETCH NEXT FROM CursorPUTT INTO @Update_ID, @CurrentPhoneNumber, @RepairedPhoneNumber
			
		END
	CLOSE CursorPUTT
	DEALLOCATE CursorPUTT



DROP TABLE #AllNumbersToChange

--Households

CREATE TABLE #AllHouseholdNumbersToChange (household_id int, home_phone varchar(40), wip_phone varchar(40), new_phone varchar(40) )
 
INSERT into #AllHouseholdNumbersToChange (household_id, home_phone, wip_phone)
select household_id, home_phone, dbo.StripNonNumeric(home_phone) FROM Households
WHERE 
  home_phone IS NOT NULL AND NOT home_phone LIKE '[2-9][0-9][0-9]-[0-9][0-9][0-9]-[0-9][0-9][0-9][0-9]'

 --Create index
CREATE INDEX wip ON #AllHouseholdNumbersToChange (wip_phone)


--if it's less than 10 digits or greater than 14 digits or 123456789 or starts with 999, we will delete it.
--new_phone is already set to null so we are good

  
--if it's exactly 11 digits and starts with 0 or 1 pull that off so that now it's 10 and will get formatted
UPDATE #AllHouseholdNumbersToChange
SET wip_phone = SUBSTRING(wip_phone,2,10)
WHERE LEN(wip_phone) = 11
AND wip_phone like '[01]%'


--if it's great than 10 digits and starts with list of known area codes then trim the end to make it 10
UPDATE #AllHouseholdNumbersToChange
SET wip_phone = 
    CASE
	   WHEN SUBSTRING(wip_phone,1,3) IN (
	        '215',
			'254',
			'260',
			'262',
			'283',
			'317',
			'325',
			'336',
			'346',
			'386',
			'419',
			'440',
			'502',
			'512',
			'513',
			'515',
			'551',
			'561',
			'606',
			'614',
			'740',
			'760',
			'770',
			'810',
			'812',
			'813',
			'858',
			'859',
			'865',
			'909',
			'928',
			'937',
			'978') 
	     THEN SUBSTRING(wip_phone,1,10)
       ELSE NULL
	END
WHERE LEN(wip_phone) > 10 AND LEN(wip_phone) <= 14

--if it's 10 digits format it unless it's 123465789 or 999, which case delete it
UPDATE #AllHouseholdNumbersToChange
SET new_phone = 
	CASE 
		WHEN wip_phone LIKE '%123456789%' THEN NULL
		WHEN wip_phone = '0000000000' THEN NULL
		WHEN wip_phone LIKE '999%' THEN NULL
		WHEN wip_phone like '[01]%' THEN NULL
		ELSE SUBSTRING(wip_phone,1,3)+'-'+SUBSTRING(wip_phone,4,3)+'-'+SUBSTRING(wip_phone,7,4)
	END
WHERE LEN(wip_phone) = 10

--For testing/validation only
/*
select household_id, home_phone as original, new_phone as to_update 
from #AllHouseholdNumbersToChange
*/

--The real thing

SET @TableName = 'Households'

	DECLARE CursorPUTT CURSOR FAST_FORWARD FOR
	SELECT household_ID,home_phone, new_phone FROM #AllHouseholdNumbersToChange

	OPEN CursorPUTT
	FETCH NEXT FROM CursorPUTT INTO @Update_ID, @CurrentPhoneNumber, @RepairedPhoneNumber
		WHILE @@FETCH_STATUS = 0
		BEGIN

		
			UPDATE Households 
			SET [Home_Phone] = @RepairedPhoneNumber 
			WHERE Household_id = @Update_ID 		
			
			--Audit Log the Change
            EXEC [dbo].[crds_Add_Audit] 
				 @TableName 
				,@Update_ID
				,'Mass Updated'
				,@UserName
				,@UserID
				,'Home_Phone'
				,'Home Phone'
				,@CurrentPhoneNumber
				,@RepairedPhoneNumber 
	
			FETCH NEXT FROM CursorPUTT INTO @Update_ID, @CurrentPhoneNumber, @RepairedPhoneNumber
			
		END
	CLOSE CursorPUTT
	DEALLOCATE CursorPUTT


DROP TABLE #AllHouseholdNumbersToChange

DROP FUNCTION StripNonNumeric
