USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Pages]
   SET [Default_Field_List] = N'Contact_ID_Table.Display_Name, cr_Medical_Information.InsuranceCompany AS Insurance_Company,cr_Medical_Information.PolicyHolderName AS Policy_Holder,cr_Medical_Information.PhysicianName as Physician_Name,cr_Medical_Information.PhysicianPhone AS Physician_Phone'
      ,[Selected_Record_Expression] = N'cr_Medical_Information.InsuranceCompany + ''; '' + cr_Medical_Information.PolicyHolderName'
      
 WHERE [Page_ID] = 607
GO


