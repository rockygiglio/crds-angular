--Create test data for Brave Journey Group Finder
--Creating Groups and Group_Attributes for the inserted groups
USE MinistryPlatform;
GO

DECLARE @Host_Contacts AS TABLE (Contact_ID INT, Last_Name VARCHAR(50), Congregation_ID INT, Email_Address VARCHAR(255), Address_ID INT)

DECLARE @cnt INT = 0

WHILE @cnt < 5
BEGIN
  INSERT INTO @Host_Contacts (Contact_ID, Last_Name, Congregation_ID, Email_Address, Address_ID)
	(SELECT c.Contact_ID, c.Last_Name, h.Congregation_ID, c.Email_Address, h.Address_ID
     FROM Contacts c
     INNER JOIN Households h ON c.Household_ID = h.Household_ID
     WHERE (c.Email_Address like '%ingagepartners%' OR c.Email_Address like '%crossroads.net%')
     AND h.Address_ID IS NOT NULL
     AND h.Congregation_ID IS NOT NULL)
	 
	 SET @cnt = @cnt + 1
END

DECLARE @IdentityOutput AS TABLE ( Group_ID int )

INSERT INTO Groups([Group_Name]
	,[Group_Type_ID]
	,[Ministry_ID]
	,[Congregation_ID]
	,[Primary_Contact]
	,[Description]
	,[Start_Date]
	,[End_Date]
	,[Offsite_Meeting_Address]
	,[Available_Online]
	,[Meeting_Time]
	,[Meeting_Day_ID]
	,[Domain_ID]
	,[Remaining_Capacity]) 
	OUTPUT INSERTED.Group_ID INTO @IdentityOutput
	SELECT CONCAT('(dev) 2016 Brave ', hc.Last_Name)
		,19
		,8
		,hc.Congregation_ID
		,hc.Contact_ID
		,'Testing Group Mass Create'
		,GETDATE()
		,CAST('20160701 00:00:00.000' as DATETIME)
		,hc.Address_ID
		,1
		,(SELECT CAST(CONCAT(FLOOR(RAND()*(24-1)+1), ':00:00.000') as TIME))
		,(SELECT FLOOR(RAND()*(7-1)+1))
		,1
		,5
	FROM  @Host_Contacts hc

	DECLARE @Group_ID int

	WHILE EXISTS (SELECT * FROM @IdentityOutput)
	BEGIN
		SELECT TOP 1 @Group_ID = Group_ID
		FROM @IdentityOutput
		ORDER BY Group_ID

		--Add Group_Attributes to groups
		INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, [Start_Date])
		  VALUES( (SELECT TOP 1 Attribute_ID FROM Attributes
					WHERE Attribute_Type_ID = 71
					ORDER BY NEWID())
				,@Group_ID
				,1
				,GETDATE())

		INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, [Start_Date])
		  VALUES( (SELECT TOP 1 Attribute_ID FROM Attributes
					WHERE Attribute_Type_ID = 73
					ORDER BY NEWID())
				,@Group_ID
				,1
				,GETDATE())

		INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, [Start_Date])
		  VALUES( (SELECT TOP 1 Attribute_ID FROM Attributes
					WHERE Attribute_Type_ID = 75
					ORDER BY NEWID())
				,@Group_ID
				,1
				,GETDATE())

		DELETE @IdentityOutput
		WHERE Group_ID = @Group_ID
	END
