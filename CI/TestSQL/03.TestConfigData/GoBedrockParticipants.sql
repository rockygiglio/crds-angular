--(t+auto) GO Bedrock participant records
USE [MinistryPlatform]
GO

DECLARE @fredPartId as int
set @fredPartId = (select participant_record from Contacts where Email_address = 'mpcrds+auto+fredflintstone@gmail.com');

DECLARE @fredDonorId as int
set @fredDonorId = (select donor_record from Contacts where Email_Address = 'mpcrds+auto+fredflintstone@gmail.com');

DECLARE @thisyear as VARCHAR(4)
set @thisyear = CONVERT(VARCHAR(4), datepart(year, getdate()));

DECLARE @tripName AS VARCHAR(24)
set @tripName = '(t+auto) GO Bedrock '+@thisyear;

DECLARE @startYear as VARCHAR(19)
set @startYear = @thisyear+'0101';

--Add Fred Flintstone to the GO Bedrock child GROUP
DECLARE @subGroupID as int
SET @subGroupID = (select GROUP_ID from groups where group_name = @tripName + ' (Trip Participants)');

INSERT INTO [dbo].Group_Participants 
(Group_ID   ,Participant_ID,Group_Role_ID,Domain_ID,[Start_Date] ,End_Date,Employee_Role,Hours_Per_Week,Notes,__ExternalPersonGroupRoleID,__ExternalGroupRoleID,__CanManageEvents,__CanMANageMembers,__EmailOptOut,__ISAnonymous,__ServiceTimeID,_First_Attendance,_Second_Attendance,_Third_Attendance,_Last_Attendance) VALUES
(@subGroupID,@fredPartId  ,16           ,1        ,@startYear   ,null    ,0            ,null          ,null ,null                       ,null                 ,null             ,null              ,null         ,null         ,null           ,null             ,null              ,null             ,null            );

--Add Fred Flintstone to Event_Participant list
INSERT INTO [dbo].Event_Participants 
(Event_ID                                                             ,Participant_ID,Participation_Status_ID,Time_In   ,Time_Confirmed ,Time_Out,Notes,Domain_ID,Group_Participant_ID,[Check-in_Station],_Setup_Date               ,Group_ID,Room_ID,Call_Parents,Group_Role_ID,Response_ID,__ExternalCalendarServingtimePersonID,Opportunity_ID) VALUES
((select event_id from events where Event_Title like '(t+auto) GO Bedrock%'),@fredPartId  ,2                      ,@startYear,@startYear    ,null    ,null ,1        ,null                ,null              ,{ts '2015-09-09 19:03:37'},null    ,null   ,null        ,null         ,null       ,null                                 ,null          );

--Add a Pledge for Fred Flintstone. Pledge campaign ID is hard coded since it will not change.
--http://crossroads.knowledgeowl.com/help/createedit-go-trip-pledge
INSERT INTO [dbo].Pledges 
(Donor_ID      ,Pledge_Campaign_ID,Pledge_Status_ID,Total_Pledge,Installments_Planned,Installments_Per_Year,First_Installment_Date ,Notes,Domain_ID,Beneficiary,Trip_Leader,Currency,__ExternalPersonID1,__ExternalPersonID2,__ExternalCommitmentID,__ExternalApplicationID) VALUES
(@fredDonorId ,10000010          ,1               ,1000.0000   ,0                   ,0                    ,@startYear             ,null ,1        ,null       ,0          ,null    ,null               ,null               ,null                  ,null                   );

--Add a deposit Donation to the pledge by Fred.
--Would his deposit donation need to be greater than or equal to the expected registration deposit (300.00)? If so this should probably be changed below as well. 
INSERT INTO [dbo].Donations 
(Donor_ID      ,Donation_Amount,Donation_Date,Payment_Type_ID,Non_Cash_Asset_Type_ID,Item_Number,Batch_ID,Notes,Donor_Account_ID,[Anonymous],Check_Scanner_Batch,Donation_Status_Information,Donation_Status_ID,Donation_Status_Date ,Donation_Status_Notes,Online_Donation_Information,Transaction_Code,Subscription_Code,Gateway_Response,Processed,Domain_ID,Currency,Receipted,Invoice_Number,Receipt_Number,__ExternalContributionID,__ExternalPaymentID,__ExternalGiverID,__ExternalDonorID,__ExteralMasterID1,__ExternalMasterID2,Registered_Donor,Processor_ID,Processor_Fee_Amount,Reconcile_Change_Needed,Reconcile_Change_Complete) VALUES
(@fredDonorId ,100.0000       ,@startYear   ,2              ,null                  ,null       ,null    ,null ,null            ,0          ,null               ,null                       ,4                 ,@startYear           ,null                 ,null                       ,null            ,null             ,null            ,1        ,1        ,null    ,1        ,null          ,null          ,null                    ,null               ,null             ,null             ,null              ,null               ,null            ,null        ,null                ,null                   ,null                     );

--Insert the Donation_Distribution.
DECLARE @donationID as int
SET @donationID = (select donation_id from donations where donor_id = (select donor_record from contacts where email_address = 'mpcrds+auto+fredflintstone@gmail.com'));

DECLARE @programID as int
SET @programID = (select program_id from programs where program_name = @tripName);

DECLARE @pledgeID as int
SET @pledgeID = (select pledge_id from pledges where donor_id = (select donor_record from contacts where Email_Address = 'mpcrds+auto+fredflintstone@gmail.com'));

INSERT INTO [dbo].donation_distributions 
(Donation_ID ,Amount  ,Program_ID ,Pledge_ID ,Target_Event,Soft_Credit_Donor,Notes,Domain_ID,__ExternalContributionID,__ExternalCommitmentID,Congregation_ID) VALUES
(@donationID ,100.0000,@programID ,@pledgeID ,null        ,null             ,null ,1        ,null                    ,null                  ,5              );
GO

