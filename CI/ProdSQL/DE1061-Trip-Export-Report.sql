--*******************************************************************************
-- 63 Attribute_Type_ID for Frequent Flyers
-- Airlines Attribute_ID in (3958, 3959, 3960, 3980)
--********************************************************************************

DECLARE @ContactAttributesToUpdate TABLE (Contact_ID INT, Attribute_ID INT, TotalRows INT);
INSERT INTO @ContactAttributesToUpdate
       SELECT ca.Contact_ID, a.Attribute_ID, COUNT(*) AS Count
       FROM [MinistryPlatform].[dbo].[Contact_Attributes] ca
            INNER JOIN attributes a ON ca.attribute_ID = a.attribute_id
       WHERE ca.End_Date IS NULL
	   AND ca.Attribute_ID in (3958, 3959, 3960, 3980, 4623)
       GROUP BY ca.Contact_ID, a.Attribute_ID
       HAVING COUNT(*) > 1
	   ORDER BY ca.Contact_ID;

DECLARE @AllAttributesForContact TABLE (Contact_Attribute_ID INT, Contact_ID INT, Attribute_ID INT);
INSERT INTO @AllAttributesForContact
       SELECT ca.Contact_Attribute_ID, ca.Contact_ID, ca.Attribute_ID
       FROM Contact_Attributes ca
            INNER JOIN Attributes a ON ca.Attribute_ID = a.Attribute_ID
            INNER JOIN @ContactAttributesToUpdate x ON ca.Contact_ID = x.Contact_ID
                                                       AND ca.Attribute_ID = x.Attribute_ID
													   AND ca.End_Date IS NULL;

DECLARE @ContactAttributesToNotUpdate TABLE (Contact_Attribute_ID INT, Contact_ID INT, Attribute_ID INT);
INSERT INTO @ContactAttributesToNotUpdate
       SELECT MAX(ca.contact_attribute_id) Contact_Attribute_ID, ca.Contact_ID, ca.Attribute_ID
       FROM Contact_Attributes ca
            INNER JOIN attributes a ON ca.attribute_ID = a.Attribute_id
            INNER JOIN @ContactAttributesToUpdate tmp ON ca.contact_id = tmp.contact_id
                                                         AND a.Attribute_ID = tmp.Attribute_ID
       GROUP BY ca.contact_id, ca.attribute_id;

DECLARE @FinalCountDown TABLE (Contact_Attribute_ID INT, Contact_ID INT, Attribute_ID INT);
INSERT INTO @FinalCountDown
       SELECT *
       FROM @AllAttributesForContact
       EXCEPT
       SELECT *
       FROM @ContactAttributesToNotUpdate;

--make this update to update END_DATE to TODAY
SELECT *
FROM @FinalCountDown
ORDER BY Contact_ID, Attribute_ID;


-- uncomment this when you are ready to update everything
--UPDATE [dbo].[Contact_Attributes]
  -- SET [End_Date] = GETDATE()
   --WHERE Contact_Attribute_ID IN (SELECT Contact_Attribute_ID FROM @FinalCountDown)
