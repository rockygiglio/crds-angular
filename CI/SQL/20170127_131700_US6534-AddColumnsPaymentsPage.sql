USE MinistryPlatform
GO
IF EXISTS (SELECT 1 FROM dp_Pages WHERE Page_ID = 359)
BEGIN
UPDATE dp_Pages
SET Default_Field_List = 'Payment_Date ,Contact_ID_Table.Display_Name ,Contact_ID_Table.Nickname ,Contact_ID_Table.First_Name ,Payment_Total ,Payment_Type_ID_Table.Payment_Type ,Payments.Item_Number ,Payments.Invoice_Number ,Payments.Transaction_Code ,Payments.Processed ,Payments.Merchant_Batch ,Payment_Status_ID_Table.Donation_Status as [Payment Status] ,Payments.Processor_Fee_Amount'
WHERE Page_ID = 359
END