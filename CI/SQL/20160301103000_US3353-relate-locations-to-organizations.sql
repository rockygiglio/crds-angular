USE MinistryPlatform;
GO

Update [dbo].[dp_Pages] 
SET [Default_Field_List] = N'Locations.Location_Name,Location_Type_ID_Table.Location_Type,Organization_ID_Table.[Name] as [Organization],Locations.Move_In_Date,Locations.Move_Out_Date'
WHERE Display_Name = 'Locations'