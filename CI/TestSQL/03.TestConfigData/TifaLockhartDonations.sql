USE [MinistryPlatform]
GO

--Tifa Lockhart Donations
--Note Donations need to be added  in order by Donation_Date
--Add a donation from last year for Tifa
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @lastYear as VARCHAR(4)
set @lastYear = CONVERT(VARCHAR(4), YEAR(DATEADD(YEAR, -1, GETDATE())));
--250 cash 9/03 12:01 AM last year
INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date                                  ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                           ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,250.0000       ,CAST(@lastYear+'-09-03 00:01' as smalldatetime),2              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,2                 ,CAST(@lastYear+'-09-03 00:01' as smalldatetime),null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                        ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc),250.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a soft credit donation from last year for tifa lockhart
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @fidelityDonor as int
set @fidelityDonor = (select donor_record from Contacts where company_name like 'Fidelity%');

DECLARE @lastYear as VARCHAR(4)
set @lastYear = CONVERT(VARCHAR(4), YEAR(DATEADD(YEAR, -1, GETDATE())));

--1500 soft credit donation for last year
INSERT INTO [dbo].Donations 
(Donor_ID       ,Donation_Amount ,Donation_Date                                  ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                           ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@fidelityDonor ,1500.0000       ,CAST(@lastYear+'-09-03 01:00' as smalldatetime),5              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,2                 ,CAST(@lastYear+'-09-03 01:00' as smalldatetime),null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution for the Soft Credit Donation. The sub query here is a little ridiculous but it works...
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                                                                           ,Amount   ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = (select donor_record from contacts where company_name like 'Fidelity%') and donation_date = CAST(@lastYear+'-09-03 01:00' as smalldatetime)),1500.0000,3         ,null     ,null        ,@tifaDonorId     ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation to Cloud's trip pledge by Tifa - not anonymous.
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @cloudDonorId as int
set @cloudDonorId = (select donor_record from contacts where Email_Address = 'mpcrds+CloudStrife@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--150 donation CASH for this year 01/01 1AM
INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,150.0000       ,CAST(@thisYear+'-01-01 01:00:00' as smalldatetime),2              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,2                 ,CAST(@thisYear+'-01-01 01:00:00' as smalldatetime),null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                        ,Amount  ,Program_ID                                                                 ,Pledge_ID                                                                                        ,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc),150.0000,(select program_id from programs where program_name like '(t) GO Midgar%') ,(select pledge_id from pledges where donor_id = @cloudDonorId and Pledge_Campaign_ID = 10000000) ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation to Cloud's trip pledge by Tifa - Anonymous.
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @cloudDonorId as int
set @cloudDonorId = (select donor_record from contacts where Email_Address = 'mpcrds+CloudStrife@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--100 donation CASH 01/02 1AM
INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,100.0000       ,CAST(@thisYear+'-01-02 01:00:00' as smalldatetime),2              ,null                  ,null       ,null    ,null ,null            ,1          ,null               ,null                       ,2                 ,CAST(@thisYear+'-01-02 01:00:00' as smalldatetime),null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. 
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                        ,Amount  ,Program_ID                                                                 ,Pledge_ID                                                                                        ,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc),100.0000,(select program_id from programs where program_name like '(t) GO Midgar%') ,(select pledge_id from pledges where donor_id = @cloudDonorId and Pledge_Campaign_ID = 10000000) ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation by Tifa, with credit card
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--150 donation CC 01/03 Mastercard
INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code              ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,150.0000       ,CAST(@thisYear+'-01-03 06:00:00' as smalldatetime),4              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,2                 ,CAST(@thisYear+'-01-03 06:00:00' as smalldatetime),null                 ,null                       ,'ch_16odS5Eldv5NE53si5NDZzoN' ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. 
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                        ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc),150.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation by Tifa, with bank ACH but no stripe information. This mirrors bank transactions migrated from legacy.
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date      ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,200.0000       ,CAST(@thisYear+'-01-03 08:00:00' as smalldatetime),5              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,2                 ,{ts '2015-09-01 00:00:00'},null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                        ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc),200.0000,54        ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a soft credit donation from this year for tifa lockhart
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @fidelityDonor as int
set @fidelityDonor = (select donor_record from Contacts where company_name like 'Fidelity%');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

INSERT INTO [dbo].Donations 
(Donor_ID       ,Donation_Amount ,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@fidelityDonor ,2500.0000       ,CAST(@thisYear+'-06-07 00:00:00' as smalldatetime),5              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,2                 ,CAST(@thisYear+'-06-07 00:00:00' as smalldatetime),null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution for the Soft Credit Donation
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                                                                     ,Amount   ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @fidelityDonor and donation_date = CAST(@thisYear+'-06-07 00:00:00' as smalldatetime)),2500.0000,3         ,null     ,null        ,@tifaDonorId     ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation by Tifa, with credit card that we will do a partial refund on
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--555 donation with CC American Express
INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code             ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,555.0000       ,CAST(@thisYear+'-01-04 00:00:00' as smalldatetime),4              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,2                 ,CAST(@thisYear+'-01-04 00:00:00' as smalldatetime),null                 ,null                       ,'ch_16ngVCEldv5NE53s9ZK1bq1Z',null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                        ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc),555.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Donation refund by Tifa, with credit card
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes                     ,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code             ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId  ,-55.0000       ,CAST(@thisYear+'-01-04 05:00:00' as smalldatetime),4              ,null                  ,null       ,null    ,'Refund on last donation' ,null            ,0          ,null               ,null                       ,2                 ,CAST(@thisYear+'-01-04 05:00:00' as smalldatetime),null                 ,null                       ,'re_16ngWoEldv5NE53sFQdhoGr8',null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution. This has a lot of sub-selects to get the right data. Sorry :(
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                        ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc),-55.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a declined donation for Tifa Lockhart
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--300 declined donation by ACH
INSERT INTO [dbo].donations 
(Donor_ID     ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes                                                          ,Online_Donation_Information,Transaction_Code             ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId ,300.0000       ,CAST(@thisYear+'-01-05 11:27:27' as smalldatetime),5              ,null                  ,null       ,null    ,null ,null            ,null       ,null               ,null                       ,3                 ,CAST(@thisYear+'-01-05 11:27:27' as smalldatetime),'insufficient_funds: Your account has insufficient funds to cover the payment.',null                       ,'py_16obd5Eldv5NE53s36krdjBt',null             ,null            ,null     ,1        ,null    ,0        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,1               ,null        ,0.2500              ,null                   ,null                     );

INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                         ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc) ,300.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a pending donation for Tifa Lockhart
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--Pending ACH bank donation
INSERT INTO [dbo].donations 
(Donor_ID     ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code             ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId ,180.0000       ,CAST(@thisYear+'-01-05 12:27:27' as smalldatetime),5              ,null                  ,null       ,null    ,null ,null            ,null       ,null               ,null                       ,1                 ,CAST(@thisYear+'-01-05 12:27:27' as smalldatetime),null                 ,null                       ,'py_16obd5Eldv5NE53s36krdjBt',null             ,null            ,null     ,1        ,null    ,0        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,1               ,null        ,0.2500              ,null                   ,null                     );

INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                         ,Amount  ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc) ,180.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a noncash asset donation with 2 distributions
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--2000 non/cash asset donation of type stock
INSERT INTO [dbo].donations 
(Donor_ID     ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId ,2000.0000      ,CAST(@thisYear+'-01-06 12:27:27' as smalldatetime),6              ,1                     ,null       ,null    ,null ,null            ,null       ,null               ,null                       ,2                 ,CAST(@thisYear+'-01-06 12:27:27' as smalldatetime),null                 ,null                       ,null            ,null             ,null            ,null     ,1        ,null    ,0        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,1               ,null        ,null                ,null                   ,null                     );

--1000 distribution to General fund. 
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                         ,Amount   ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc) ,1000.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );

--1000 distribution to (t) Test Pledge Program1 fund.
INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                         ,Amount   ,Program_ID                                                                       ,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc) ,1000.0000,(Select program_id from programs where program_name = '(t) Test Pledge Program1'),null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO


--Add a Check Donation that has a check number and a stripe transaction ID
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--42 dollar Check donation with a check number of 1234
INSERT INTO [dbo].donations 
(Donor_ID     ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code             ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID        ,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId ,42.0000        ,CAST(@thisYear+'-01-07 12:27:27' as smalldatetime),1              ,null                  ,'1234'     ,null    ,null ,null            ,null       ,null               ,null                       ,2                 ,CAST(@thisYear+'-01-06 12:27:27' as smalldatetime),null                 ,null                       ,'py_17q50XEldv5NE53sDWzSG9s6',null             ,null            ,null     ,1        ,null    ,0        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,1               ,'cus_85UsQReBytr2dn',0.25                ,null                   ,null                     );

INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                         ,Amount ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc) ,42.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Check Donation that has a check number but no stripe transaction ID. This mirrors migrated check data.
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--43 dollar Check donation with a check number of 1337
INSERT INTO [dbo].donations 
(Donor_ID     ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID        ,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId ,43.0000        ,CAST(@thisYear+'-01-07 13:27:27' as smalldatetime),1              ,null                  ,'1337'     ,null    ,null ,null            ,null       ,null               ,null                       ,2                 ,CAST(@thisYear+'-01-06 12:27:27' as smalldatetime),null                 ,null                       ,null            ,null             ,null            ,null     ,1        ,null    ,0        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,1               ,null                ,0.25                ,null                   ,null                     );

INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                         ,Amount ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc) ,43.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Check Donation has stripe data but no check number for some reason.
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--44 dollar check with no check number but has stripe data - Should not HAPPEN
INSERT INTO [dbo].donations 
(Donor_ID     ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code             ,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID        ,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId ,44.0000        ,CAST(@thisYear+'-01-07 14:27:27' as smalldatetime),1              ,null                  ,null       ,null    ,null ,null            ,null       ,null               ,null                       ,2                 ,CAST(@thisYear+'-01-06 12:27:27' as smalldatetime),null                 ,null                       ,'py_17q5pZEldv5NE53s2tTutxOz',null             ,null            ,null     ,1        ,null    ,0        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,1               ,'cus_85UsQReBytr2dn',0.25                ,null                   ,null                     );

INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                         ,Amount ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc) ,44.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Check with no check number or stripe data (DBT check case)
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--45 dollar check with no check number and no stripe data
INSERT INTO [dbo].donations 
(Donor_ID     ,Donation_Amount,Donation_Date                                     ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date                              ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId ,45.0000        ,CAST(@thisYear+'-01-07 15:27:27' as smalldatetime),1              ,null                  ,null       ,null    ,null ,null            ,null       ,null               ,null                       ,2                 ,CAST(@thisYear+'-01-06 12:27:27' as smalldatetime),null                 ,null                       ,null            ,null             ,null            ,null     ,1        ,null    ,0        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,1               ,null        ,null                ,null                   ,null                     );

INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                         ,Amount ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc) ,45.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

--Add a Credit card donation on today's date that has a donation_status of SUCCESSFUL but is not in a deposit. 
DECLARE @tifaDonorId as int
set @tifaDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+TifaLockhart@gmail.com');

DECLARE @thisYear as VARCHAR(4)
set @thisYear = CONVERT(VARCHAR(4), YEAR(GETDATE()));

--45 dollar check with no check number and no stripe data
INSERT INTO [dbo].donations 
(Donor_ID     ,Donation_Amount,Donation_Date ,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@tifaDonorId ,72.0000        ,getDate()     ,4              ,null                  ,null       ,null    ,null ,null            ,null       ,null               ,null                       ,4                 ,getDate()           ,null                 ,null                       ,null            ,null             ,null            ,null     ,1        ,null    ,0        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,1               ,null        ,null                ,null                   ,null                     );

INSERT INTO [dbo].donation_distributions 
(Donation_ID                                                                                         ,Amount ,Program_ID,Pledge_ID,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
((select top 1 Donation_ID from donations where donor_id = @tifaDonorId order by Donation_date desc) ,72.0000,3         ,null     ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO