USE MinistryPlatform
GO

DECLARE @User_Name NVARCHAR(50) = 'Sandi Ritter';
DECLARE @User_Id INT = 1529662;
DECLARE @DonationIdTbl TABLE (ID INT, New_Status INT, Prev_Status INT)

--Checks and Bank Payments that have a status of succeeded and a batch id
Update Donations 
SET donation_status_id = 2 --deposited
OUTPUT INSERTED.donation_id, INSERTED.donation_status_id, DELETED.donation_status_id INTO @DonationIdTbl
WHERE donation_date between '2016-01-25' and (DATEADD(DAY,  -7, GETDATE()))
AND donation_status_id = 4  --succeeded
AND payment_type_Id IN (1,5) --check and bank
AND Batch_ID IS NOT NULL
 
DECLARE @AuditLogTbl TABLE (AID INT, Record_ID INT)
    INSERT INTO dbo.dp_Audit_Log
       (Table_Name, Record_ID, Audit_Description, User_Name, User_ID, Date_Time)
        OUTPUT INSERTED.Audit_Item_ID, INSERTED.Record_ID INTO @AuditLogTbl    
       SELECT 'Donations', ID,'Updated',@User_Name, @User_Id, GETDATE() FROM @DonationIdTbl
      
  INSERT INTO dbo.dp_Audit_Detail
    (Audit_Item_ID, Field_Name, Field_Label, Previous_ID, New_ID)
  SELECT AId, 'Donation_Status_ID','Donation_Status', Prev_Status, New_Status 
  FROM @AuditLogTbl A 
    INNER JOIN @DonationIdTbl T on T.ID = A.Record_ID 