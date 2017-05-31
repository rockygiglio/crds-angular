USE [MinistryPlatform]
GO

ALTER TABLE cr_Projects ADD [Check_In_Floor]  nvarchar(50);

ALTER TABLE cr_Projects ADD [Check_In_Area]  nvarchar(50);

ALTER TABLE cr_Projects ADD [Check_In_Room_Number]  nvarchar(50);

ALTER TABLE cr_Projects ADD [Note_To_Volunteers_1]  nvarchar(500);

ALTER TABLE cr_Projects ADD [Note_To_Volunteers_2]  nvarchar(500);

ALTER TABLE cr_Projects ADD [Project_Parking_Location]  nvarchar(50);

ALTER TABLE cr_Projects ADD [Address_ID] int;

ALTER TABLE cr_Projects
ADD CONSTRAINT FK_Projects_Addresses FOREIGN KEY(Address_ID)REFERENCES dbo.Addresses(Address_ID)


GO
