USE MinistryPlatform
GO

INSERT INTO cr_Childcare_Preferred_Times (Congregation_ID, Childcare_Day_ID, Childcare_Start_Time, Childcare_End_Time, Domain_ID)
VALUES  (1, 3, '09:30', '11:30', 1)
      , (1, 3, '18:30', '20:30', 1)
	  , (1, 5, '09:30', '12:00', 1)
	  , (1, 5, '18:30', '20:30', 1)
	  , (6, 3, '09:30', '11:30', 1)
	  , (6, 3, '18:30', '20:30', 1)
	  , (6, 6, '09:30', '12:00', 1)
	  , (8, 3, '09:30', '11:30', 1)
	  , (8, 5, '09:30', '12:00', 1)
	  , (8, 5, '18:30', '20:30', 1)
	  , (7, 3, '09:30', '11:30', 1)
	  , (7, 3, '18:30', '20:30', 1)
	  , (7, 5, '09:30', '12:00', 1)
	  , (7, 5, '18:30', '20:30', 1);

GO
