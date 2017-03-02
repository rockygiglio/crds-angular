USE MinistryPlatform
GO

-- =====================================
-- Author		Jon Horner
-- Date			2/27/2017
-- Description	Adds image urls to Andover, Georgetown, and Richmond sites
-- =====================================

DECLARE @andoverId INT = 19;
DECLARE @andoverUrl nvarchar(max) = '//crds-cms-uploads.imgix.net/content/images/andover-sq.jpg';

DECLARE @georgetownId INT = 16;
DECLARE @georgetownUrl nvarchar(max) = '//crds-cms-uploads.imgix.net/content/images/georgetown-sq.jpg';

DECLARE @richmondId INT = 17;
DECLARE @richmondUrl nvarchar(max) = '//crds-cms-uploads.imgix.net/content/images/richmond-sq.jpg';

UPDATE Locations
SET Image_URL = @andoverUrl
WHERE Location_ID = @andoverId;

UPDATE Locations
SET Image_URL = @georgetownUrl
WHERE Location_ID = @georgetownId;

UPDATE Locations
SET Image_URL = @richmondUrl
WHERE Location_ID = @richmondId;
