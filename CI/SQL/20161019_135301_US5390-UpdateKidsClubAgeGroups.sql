Use [MinistryPlatform];

DECLARE @Contact_ID int = 7494108; -- Deanne Crooms

DECLARE @Month_Jan_Attribute_ID int = 9002;
DECLARE @Month_Feb_Attribute_ID int = 9003;
DECLARE @Month_March_Attribute_ID int = 9004;
DECLARE @Month_April_Attribute_ID int = 9005;
DECLARE @Month_May_Attribute_ID int = 9006;
DECLARE @Month_June_Attribute_ID int = 9007;
DECLARE @Month_July_Attribute_ID int = 9008;
DECLARE @Month_Aug_Attribute_ID int = 9009;
DECLARE @Month_Sep_Attribute_ID int = 9010;
DECLARE @Month_Oct_Attribute_ID int = 9011;
DECLARE @Month_Nov_Attribute_ID int = 9012;
DECLARE @Month_Dec_Attribute_ID int = 9013;

DECLARE @Age_1_Attribute_ID int = 9015;
DECLARE @Age_2_Attribute_ID int = 9016;
DECLARE @Age_3_Attribute_ID int = 9017;
DECLARE @Age_4_Attribute_ID int = 9018;
DECLARE @Age_5_Attribute_ID int = 9019;

DECLARE @Domain_ID int = 1;
DECLARE @Attribute_Date DATE = GETDATE();

BEGIN TRANSACTION;

BEGIN TRY
  -- Update Contact on existing groups
  UPDATE Groups SET Primary_Contact = @Contact_ID WHERE Group_ID IN (
    173940, 173941, 173942, 173943, 173944, 173945, 173946, 173947, 173948, 173949, 173950, 173951, 173952, 173953, 173954,
    173955, 173956, 173957, 173958, 173959, 173960, 173961, 173962, 173963, 173964, 173965, 173966, 173967, 173968, 173969,
    173970, 173971, 173972, 173973, 173974, 173975, 173976, 173977, 173978, 173979, 173980, 173981, 173982, 173983, 173984,
    173985, 173986, 173987, 173988, 173989, 173990, 173991, 173992, 173993, 173994, 173995, 173996, 173997, 173998, 173999   
  );

  -- Make sure they auto-promote
  UPDATE Groups SET Promote_Weekly = 1 WHERE Group_ID IN (
    173940, 173941, 173942, 173943, 173944, 173945, 173946, 173947, 173948, 173949, 173950, 173951, 173952, 173953, 173954,
    173955, 173956, 173957, 173958, 173959, 173960, 173961, 173962, 173963, 173964, 173965, 173966, 173967, 173968, 173969,
    173970, 173971, 173972, 173973, 173974, 173975, 173976, 173977, 173978, 173979, 173980, 173981, 173982, 173983, 173984,
    173985, 173986, 173987, 173988, 173989, 173990, 173991, 173992, 173993, 173994, 173995, 173996, 173997, 173998, 173999   
  );

  -- Remove any existing age or month attributes
  DELETE FROM Group_Attributes WHERE Group_ID IN (
    173940, 173941, 173942, 173943, 173944, 173945, 173946, 173947, 173948, 173949, 173950, 173951, 173952, 173953, 173954,
    173955, 173956, 173957, 173958, 173959, 173960, 173961, 173962, 173963, 173964, 173965, 173966, 173967, 173968, 173969,
    173970, 173971, 173972, 173973, 173974, 173975, 173976, 173977, 173978, 173979, 173980, 173981, 173982, 173983, 173984,
    173985, 173986, 173987, 173988, 173989, 173990, 173991, 173992, 173993, 173994, 173995, 173996, 173997, 173998, 173999   
  )
  AND Attribute_ID IN (
    @Month_Jan_Attribute_ID, @Month_Feb_Attribute_ID, @Month_March_Attribute_ID, @Month_April_Attribute_ID, @Month_May_Attribute_ID,
    @Month_June_Attribute_ID, @Month_July_Attribute_ID, @Month_Aug_Attribute_ID, @Month_Sep_Attribute_ID, @Month_Oct_Attribute_ID,
    @Month_Nov_Attribute_ID, @Month_Dec_Attribute_ID, @Age_1_Attribute_ID, @Age_2_Attribute_ID, @Age_3_Attribute_ID,
    @Age_4_Attribute_ID, @Age_5_Attribute_ID
  );

  -- Insert Group Birth Month attributes
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Jan_Attribute_ID, 173940, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Jan
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Feb_Attribute_ID, 173941, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Feb
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_March_Attribute_ID, 173942, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old March
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_April_Attribute_ID, 173943, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old April
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_May_Attribute_ID, 173944, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old May
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_June_Attribute_ID, 173945, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old June
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_July_Attribute_ID, 173946, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old July
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Aug_Attribute_ID, 173947, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Aug
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Sep_Attribute_ID, 173948, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Sep
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Oct_Attribute_ID, 173949, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Oct
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Nov_Attribute_ID, 173950, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Nov
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Dec_Attribute_ID, 173951, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Dec
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Jan_Attribute_ID, 173952, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Jan
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Feb_Attribute_ID, 173953, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Feb
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_March_Attribute_ID, 173954, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old March
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_April_Attribute_ID, 173955, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old April
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_May_Attribute_ID, 173956, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old May
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_June_Attribute_ID, 173957, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old June
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_July_Attribute_ID, 173958, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old July
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Aug_Attribute_ID, 173959, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Aug
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Sep_Attribute_ID, 173960, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Sep
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Oct_Attribute_ID, 173961, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Oct
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Nov_Attribute_ID, 173962, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Nov
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Dec_Attribute_ID, 173963, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Dec
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Jan_Attribute_ID, 173964, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Jan
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Feb_Attribute_ID, 173965, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Feb
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_March_Attribute_ID, 173966, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old March
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_April_Attribute_ID, 173967, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old April
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_May_Attribute_ID, 173968, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old May
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_June_Attribute_ID, 173969, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old June
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_July_Attribute_ID, 173970, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old July
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Aug_Attribute_ID, 173971, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Aug
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Sep_Attribute_ID, 173972, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Sep
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Oct_Attribute_ID, 173973, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Oct
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Nov_Attribute_ID, 173974, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Nov
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Dec_Attribute_ID, 173975, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Dec
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Jan_Attribute_ID, 173976, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Jan
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Feb_Attribute_ID, 173977, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Feb
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_March_Attribute_ID, 173978, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old March
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_April_Attribute_ID, 173979, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old April
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_May_Attribute_ID, 173980, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old May
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_June_Attribute_ID, 173981, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old June
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_July_Attribute_ID, 173982, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old July
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Aug_Attribute_ID, 173983, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Aug
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Sep_Attribute_ID, 173984, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Sep
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Oct_Attribute_ID, 173985, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Oct
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Nov_Attribute_ID, 173986, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Nov
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Dec_Attribute_ID, 173987, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Dec
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Jan_Attribute_ID, 173988, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Jan
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Feb_Attribute_ID, 173989, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Feb
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_March_Attribute_ID, 173990, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old March
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_April_Attribute_ID, 173991, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old April
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_May_Attribute_ID, 173992, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old May
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_June_Attribute_ID, 173993, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old June
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_July_Attribute_ID, 173994, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old July
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Aug_Attribute_ID, 173995, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Aug
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Sep_Attribute_ID, 173996, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Sep
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Oct_Attribute_ID, 173997, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Oct
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Nov_Attribute_ID, 173998, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Nov
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Dec_Attribute_ID, 173999, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Dec

  -- Insert Group Age Attributes
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173940, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Jan
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173941, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Feb
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173942, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old March
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173943, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old April
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173944, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old May
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173945, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old June
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173946, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old July
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173947, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Aug
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173948, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Sep
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173949, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Oct
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173950, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Nov
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_5_Attribute_ID, 173951, @Domain_ID, @Attribute_Date); -- Kids Club 5 Year Old Dec
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173952, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Jan
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173953, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Feb
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173954, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old March
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173955, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old April
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173956, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old May
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173957, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old June
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173958, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old July
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173959, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Aug
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173960, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Sep
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173961, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Oct
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173962, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Nov
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_4_Attribute_ID, 173963, @Domain_ID, @Attribute_Date); -- Kids Club 4 Year Old Dec
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173964, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Jan
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173965, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Feb
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173966, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old March
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173967, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old April
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173968, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old May
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173969, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old June
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173970, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old July
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173971, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Aug
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173972, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Sep
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173973, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Oct
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173974, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Nov
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_3_Attribute_ID, 173975, @Domain_ID, @Attribute_Date); -- Kids Club 3 Year Old Dec
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173976, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Jan
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173977, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Feb
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173978, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old March
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173979, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old April
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173980, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old May
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173981, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old June
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173982, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old July
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173983, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Aug
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173984, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Sep
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173985, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Oct
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173986, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Nov
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_2_Attribute_ID, 173987, @Domain_ID, @Attribute_Date); -- Kids Club 2 Year Old Dec
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173988, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Jan
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173989, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Feb
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173990, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old March
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173991, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old April
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173992, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old May
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173993, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old June
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173994, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old July
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173995, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Aug
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173996, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Sep
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173997, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Oct
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173998, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Nov
  INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_1_Attribute_ID, 173999, @Domain_ID, @Attribute_Date); -- Kids Club 1 Year Old Dec
END TRY
BEGIN CATCH
  PRINT 'Rolling back transaction due to error ' + ERROR_MESSAGE();
  IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
  COMMIT TRANSACTION;
