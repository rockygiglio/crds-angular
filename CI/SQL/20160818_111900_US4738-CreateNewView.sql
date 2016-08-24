USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Page_Views]
           ([View_Title]
           ,[Page_ID]
           ,[Description]
           ,[Field_List]
           ,[View_Clause]
           ,[Order_By]
           ,[User_ID]
           ,[User_Group_ID])
     VALUES
           (
		   'Outstanding Donations (10 days)'
           ,297
           ,'Donations that have had a status of Pending or Succeeded for over 10 days'
           ,'Donation_Date,Donor_ID_Table_Contact_ID_Table.Display_Name,
			 Donor_ID_Table_Contact_ID_Table.Nickname,
			 Donor_ID_Table_Contact_ID_Table.Email_Address,
			 Donation_Amount,
			 Payment_Type_ID_Table.Payment_Type,
		     Donation_Status_ID_Table.Donation_Status,
			 Donations.Donation_Status_Date,Donations.Check_Scanner_Batch,Is_Recurring_Gift'
           ,'Donation_Status_ID_Table.Donation_Status_ID IN (1,4,null) and Donation_Date < Getdate()-10'
           , null
           , null
           , null
		   )
GO

