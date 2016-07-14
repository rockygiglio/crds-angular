USE MinistryPlatform
GO

ALTER TABLE dbo.Groups 
ADD Kids_Welcome bit NOT NULL
CONSTRAINT DF_Groups_Kids_Welcome DEFAULT 0
