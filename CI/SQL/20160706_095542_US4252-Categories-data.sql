Use [MinistryPlatform]
GO

IF NOT EXISTS(Select * from cr_Categories where Category = 'Healing')
BEGIN
INSERT INTO [dbo].[cr_Categories] 
(Category ,Description,Domain_ID) VALUES
('Healing',null       ,1        );
END
GO

IF NOT EXISTS(Select * from cr_Categories where Category = 'Interest')
BEGIN
INSERT INTO [dbo].[cr_Categories] 
(Category  ,Description,Domain_ID) VALUES
('Interest',null       ,1         );
END
GO

IF NOT EXISTS(Select * from cr_Categories where Category = 'Life Stages')
BEGIN
INSERT INTO [dbo].[cr_Categories]
(Category     ,Description,Domain_ID) VALUES
('Life Stages',null       ,1        );
END
GO

IF NOT EXISTS(Select * from cr_Categories where Category = 'Neighborhoods')
BEGIN
INSERT INTO [dbo].[cr_Categories] 
(Category       ,Description,Domain_ID) VALUES
('Neighborhoods',null       ,1        );
END
GO

IF NOT EXISTS(Select * from cr_Categories where Category = 'Spritual Growth')
BEGIN
INSERT INTO [dbo].[cr_Categories] 
(Category          ,Description,Domain_ID) VALUES
('Spiritual Growth',null       ,1        );
END
GO

SET IDENTITY_INSERT [dbo].[cr_Categories] OFF;