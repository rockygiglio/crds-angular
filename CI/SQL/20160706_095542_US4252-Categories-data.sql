Use [MinistryPlatform]
GO

IF NOT EXISTS(SELECT * FROM cr_Categories WHERE Category = 'Healing')
BEGIN
INSERT INTO [dbo].[cr_Categories] 
(Category ,Description,Domain_ID) VALUES
('Healing',NULL       ,1        );
END
GO

IF NOT EXISTS(SELECT * FROM cr_Categories WHERE Category = 'Interest')
BEGIN
INSERT INTO [dbo].[cr_Categories] 
(Category  ,Description,Domain_ID) VALUES
('Interest',NULL       ,1         );
END
GO

IF NOT EXISTS(SELECT * FROM cr_Categories WHERE Category = 'Life Stages')
BEGIN
INSERT INTO [dbo].[cr_Categories]
(Category     ,Description,Domain_ID) VALUES
('Life Stages',NULL       ,1        );
END
GO

IF NOT EXISTS(SELECT * FROM cr_Categories WHERE Category = 'Neighborhoods')
BEGIN
INSERT INTO [dbo].[cr_Categories] 
(Category       ,Description,Domain_ID) VALUES
('Neighborhoods',NULL       ,1        );
END
GO

IF NOT EXISTS(SELECT * FROM cr_Categories WHERE Category = 'Spritual Growth')
BEGIN
INSERT INTO [dbo].[cr_Categories] 
(Category          ,Description,Domain_ID) VALUES
('Spiritual Growth',NULL       ,1        );
END
GO

SET IDENTITY_INSERT [dbo].[cr_Categories] OFF;