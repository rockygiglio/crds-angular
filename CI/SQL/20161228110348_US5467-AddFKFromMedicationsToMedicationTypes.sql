USE MinistryPlatform
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Medications_Medication_Types]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]'))
ALTER TABLE [dbo].[cr_Medical_Information_Medications]  WITH CHECK ADD  CONSTRAINT [FK_cr_Medical_Information_Medications_Medication_Types] FOREIGN KEY([Medication_Type_ID])
REFERENCES [dbo].[cr_Medication_Types] ([Medication_Type_ID])
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_cr_Medical_Information_Medications_Medication_Types]') AND parent_object_id = OBJECT_ID(N'[dbo].[cr_Medical_Information_Medications]'))
ALTER TABLE [dbo].[cr_Medical_Information_Medications] CHECK CONSTRAINT [FK_cr_Medical_Information_Medications_Medication_Types]
GO