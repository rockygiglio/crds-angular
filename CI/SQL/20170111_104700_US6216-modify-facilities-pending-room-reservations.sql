USE [MinistryPlatform]
GO

IF EXISTS(SELECT 1 FROM [dbo].[dp_Page_Views] WHERE View_Title LIKE 'Pending Room Reservations - %' AND View_Title != 'Pending Room Reservations - All')
BEGIN
  DELETE FROM [dbo].[dp_Page_Views]
  WHERE View_Title LIKE 'Pending Room Reservations - %' AND View_Title != 'Pending Room Reservations - All')
END
