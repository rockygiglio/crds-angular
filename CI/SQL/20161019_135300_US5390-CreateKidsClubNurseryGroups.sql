Use [MinistryPlatform];

DECLARE @Group_Type_ID int = 4; -- Age or Grade Group
DECLARE @Ministry_ID int = 2; -- Kids Club
DECLARE @Congregation_ID int = 5; -- Not Site Specific
DECLARE @Contact_ID int = 7494108; -- Deanne Crooms
DECLARE @Group_Start_Date DATE = GETDATE();
DECLARE @Domain_ID int = 1;
DECLARE @Secure_Check_In int = 1;
DECLARE @Suppress_Nametag int = 1;
DECLARE @Promote_Weekly int = 1;

DECLARE @Age_N_Attribute_ID int = 9014;

DECLARE @Nursery_Month_1_Attribute_ID int = 9020;
DECLARE @Nursery_Month_2_Attribute_ID int = 9021;
DECLARE @Nursery_Month_3_Attribute_ID int = 9022;
DECLARE @Nursery_Month_4_Attribute_ID int = 9023;
DECLARE @Nursery_Month_5_Attribute_ID int = 9024;
DECLARE @Nursery_Month_6_Attribute_ID int = 9025;
DECLARE @Nursery_Month_7_Attribute_ID int = 9026;
DECLARE @Nursery_Month_8_Attribute_ID int = 9027;
DECLARE @Nursery_Month_9_Attribute_ID int = 9028;
DECLARE @Nursery_Month_10_Attribute_ID int = 9029;
DECLARE @Nursery_Month_11_Attribute_ID int = 9030;
DECLARE @Nursery_Month_12_Attribute_ID int = 9031;

DECLARE @Month_January_Attribute_ID int = 9002;
DECLARE @Month_February_Attribute_ID int = 9003;
DECLARE @Month_March_Attribute_ID int = 9004;
DECLARE @Month_April_Attribute_ID int = 9005;
DECLARE @Month_May_Attribute_ID int = 9006;
DECLARE @Month_June_Attribute_ID int = 9007;
DECLARE @Month_July_Attribute_ID int = 9008;
DECLARE @Month_August_Attribute_ID int = 9009;
DECLARE @Month_September_Attribute_ID int = 9010;
DECLARE @Month_October_Attribute_ID int = 9011;
DECLARE @Month_November_Attribute_ID int = 9012;
DECLARE @Month_December_Attribute_ID int = 9013;

DECLARE @KidsClubNurseryGroups TABLE (
  Group_Name nvarchar(75),
  Group_Description nvarchar(2000),
  Age_In_Months_To_Promote int,
  Nursery_Month_Attribute_ID int,
  Month_Attribute_ID int
);

-- ====================================================================
-- January
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (January)', 'Kids Club Age Group 11-12 Months, January birthday. This promotes automatically to a 1 Year Old January group.', 12, @Nursery_Month_12_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 10-11 Month Old (January)', 'Kids Club Age Group 10-11 Months, January birthday. This promotes automatically to a 11-12 Month Old January group.', 11, @Nursery_Month_11_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 9-10 Month Old (January)', 'Kids Club Age Group 9-10 Months, January birthday. This promotes automatically to a 10-11 Month Old January group.', 10, @Nursery_Month_10_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 8-9 Month Old (January)', 'Kids Club Age Group 8-9 Months, January birthday. This promotes automatically to a 9-10 Month Old January group.', 9, @Nursery_Month_9_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 7-8 Month Old (January)', 'Kids Club Age Group 7-8 Months, January birthday. This promotes automatically to a 8-9 Month Old January group.', 8, @Nursery_Month_8_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 6-7 Month Old (January)', 'Kids Club Age Group 6-7 Months, January birthday. This promotes automatically to a 7-8 Month Old January group.', 7, @Nursery_Month_7_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 5-6 Month Old (January)', 'Kids Club Age Group 5-6 Months, January birthday. This promotes automatically to a 6-7 Month Old January group.', 6, @Nursery_Month_6_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 4-5 Month Old (January)', 'Kids Club Age Group 4-5 Months, January birthday. This promotes automatically to a 5-6 Month Old January group.', 5, @Nursery_Month_5_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 3-4 Month Old (January)', 'Kids Club Age Group 3-4 Months, January birthday. This promotes automatically to a 4-5 Month Old January group.', 4, @Nursery_Month_4_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 2-3 Month Old (January)', 'Kids Club Age Group 2-3 Months, January birthday. This promotes automatically to a 3-4 Month Old January group.', 3, @Nursery_Month_3_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 1-2 Month Old (January)', 'Kids Club Age Group 1-2 Months, January birthday. This promotes automatically to a 2-3 Month Old January group.', 2, @Nursery_Month_2_Attribute_ID, @Month_January_Attribute_ID),
  ('Kids Club 0-1 Month Old (January)', 'Kids Club Age Group 0-1 Months, January birthday. This promotes automatically to a 1-2 Month Old January group.', 1, @Nursery_Month_1_Attribute_ID, @Month_January_Attribute_ID);

-- ====================================================================
-- February
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (February)', 'Kids Club Age Group 11-12 Months, February birthday. This promotes automatically to a 1 Year Old February group.', 12, @Nursery_Month_12_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 10-11 Month Old (February)', 'Kids Club Age Group 10-11 Months, February birthday. This promotes automatically to a 11-12 Month Old February group.', 11, @Nursery_Month_11_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 9-10 Month Old (February)', 'Kids Club Age Group 9-10 Months, February birthday. This promotes automatically to a 10-11 Month Old February group.', 10, @Nursery_Month_10_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 8-9 Month Old (February)', 'Kids Club Age Group 8-9 Months, February birthday. This promotes automatically to a 9-10 Month Old February group.', 9, @Nursery_Month_9_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 7-8 Month Old (February)', 'Kids Club Age Group 7-8 Months, February birthday. This promotes automatically to a 8-9 Month Old February group.', 8, @Nursery_Month_8_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 6-7 Month Old (February)', 'Kids Club Age Group 6-7 Months, February birthday. This promotes automatically to a 7-8 Month Old February group.', 7, @Nursery_Month_7_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 5-6 Month Old (February)', 'Kids Club Age Group 5-6 Months, February birthday. This promotes automatically to a 6-7 Month Old February group.', 6, @Nursery_Month_6_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 4-5 Month Old (February)', 'Kids Club Age Group 4-5 Months, February birthday. This promotes automatically to a 5-6 Month Old February group.', 5, @Nursery_Month_5_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 3-4 Month Old (February)', 'Kids Club Age Group 3-4 Months, February birthday. This promotes automatically to a 4-5 Month Old February group.', 4, @Nursery_Month_4_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 2-3 Month Old (February)', 'Kids Club Age Group 2-3 Months, February birthday. This promotes automatically to a 3-4 Month Old February group.', 3, @Nursery_Month_3_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 1-2 Month Old (February)', 'Kids Club Age Group 1-2 Months, February birthday. This promotes automatically to a 2-3 Month Old February group.', 2, @Nursery_Month_2_Attribute_ID, @Month_February_Attribute_ID),
  ('Kids Club 0-1 Month Old (February)', 'Kids Club Age Group 0-1 Months, February birthday. This promotes automatically to a 1-2 Month Old February group.', 1, @Nursery_Month_1_Attribute_ID, @Month_February_Attribute_ID);

-- ====================================================================
-- March
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (March)', 'Kids Club Age Group 11-12 Months, March birthday. This promotes automatically to a 1 Year Old March group.', 12, @Nursery_Month_12_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 10-11 Month Old (March)', 'Kids Club Age Group 10-11 Months, March birthday. This promotes automatically to a 11-12 Month Old March group.', 11, @Nursery_Month_11_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 9-10 Month Old (March)', 'Kids Club Age Group 9-10 Months, March birthday. This promotes automatically to a 10-11 Month Old March group.', 10, @Nursery_Month_10_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 8-9 Month Old (March)', 'Kids Club Age Group 8-9 Months, March birthday. This promotes automatically to a 9-10 Month Old March group.', 9, @Nursery_Month_9_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 7-8 Month Old (March)', 'Kids Club Age Group 7-8 Months, March birthday. This promotes automatically to a 8-9 Month Old March group.', 8, @Nursery_Month_8_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 6-7 Month Old (March)', 'Kids Club Age Group 6-7 Months, March birthday. This promotes automatically to a 7-8 Month Old March group.', 7, @Nursery_Month_7_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 5-6 Month Old (March)', 'Kids Club Age Group 5-6 Months, March birthday. This promotes automatically to a 6-7 Month Old March group.', 6, @Nursery_Month_6_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 4-5 Month Old (March)', 'Kids Club Age Group 4-5 Months, March birthday. This promotes automatically to a 5-6 Month Old March group.', 5, @Nursery_Month_5_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 3-4 Month Old (March)', 'Kids Club Age Group 3-4 Months, March birthday. This promotes automatically to a 4-5 Month Old March group.', 4, @Nursery_Month_4_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 2-3 Month Old (March)', 'Kids Club Age Group 2-3 Months, March birthday. This promotes automatically to a 3-4 Month Old March group.', 3, @Nursery_Month_3_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 1-2 Month Old (March)', 'Kids Club Age Group 1-2 Months, March birthday. This promotes automatically to a 2-3 Month Old March group.', 2, @Nursery_Month_2_Attribute_ID, @Month_March_Attribute_ID),
  ('Kids Club 0-1 Month Old (March)', 'Kids Club Age Group 0-1 Months, March birthday. This promotes automatically to a 1-2 Month Old March group.', 1, @Nursery_Month_1_Attribute_ID, @Month_March_Attribute_ID);

-- ====================================================================
-- April
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (April)', 'Kids Club Age Group 11-12 Months, April birthday. This promotes automatically to a 1 Year Old April group.', 12, @Nursery_Month_12_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 10-11 Month Old (April)', 'Kids Club Age Group 10-11 Months, April birthday. This promotes automatically to a 11-12 Month Old April group.', 11, @Nursery_Month_11_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 9-10 Month Old (April)', 'Kids Club Age Group 9-10 Months, April birthday. This promotes automatically to a 10-11 Month Old April group.', 10, @Nursery_Month_10_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 8-9 Month Old (April)', 'Kids Club Age Group 8-9 Months, April birthday. This promotes automatically to a 9-10 Month Old April group.', 9, @Nursery_Month_9_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 7-8 Month Old (April)', 'Kids Club Age Group 7-8 Months, April birthday. This promotes automatically to a 8-9 Month Old April group.', 8, @Nursery_Month_8_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 6-7 Month Old (April)', 'Kids Club Age Group 6-7 Months, April birthday. This promotes automatically to a 7-8 Month Old April group.', 7, @Nursery_Month_7_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 5-6 Month Old (April)', 'Kids Club Age Group 5-6 Months, April birthday. This promotes automatically to a 6-7 Month Old April group.', 6, @Nursery_Month_6_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 4-5 Month Old (April)', 'Kids Club Age Group 4-5 Months, April birthday. This promotes automatically to a 5-6 Month Old April group.', 5, @Nursery_Month_5_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 3-4 Month Old (April)', 'Kids Club Age Group 3-4 Months, April birthday. This promotes automatically to a 4-5 Month Old April group.', 4, @Nursery_Month_4_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 2-3 Month Old (April)', 'Kids Club Age Group 2-3 Months, April birthday. This promotes automatically to a 3-4 Month Old April group.', 3, @Nursery_Month_3_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 1-2 Month Old (April)', 'Kids Club Age Group 1-2 Months, April birthday. This promotes automatically to a 2-3 Month Old April group.', 2, @Nursery_Month_2_Attribute_ID, @Month_April_Attribute_ID),
  ('Kids Club 0-1 Month Old (April)', 'Kids Club Age Group 0-1 Months, April birthday. This promotes automatically to a 1-2 Month Old April group.', 1, @Nursery_Month_1_Attribute_ID, @Month_April_Attribute_ID);

-- ====================================================================
-- May
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (May)', 'Kids Club Age Group 11-12 Months, May birthday. This promotes automatically to a 1 Year Old May group.', 12, @Nursery_Month_12_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 10-11 Month Old (May)', 'Kids Club Age Group 10-11 Months, May birthday. This promotes automatically to a 11-12 Month Old May group.', 11, @Nursery_Month_11_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 9-10 Month Old (May)', 'Kids Club Age Group 9-10 Months, May birthday. This promotes automatically to a 10-11 Month Old May group.', 10, @Nursery_Month_10_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 8-9 Month Old (May)', 'Kids Club Age Group 8-9 Months, May birthday. This promotes automatically to a 9-10 Month Old May group.', 9, @Nursery_Month_9_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 7-8 Month Old (May)', 'Kids Club Age Group 7-8 Months, May birthday. This promotes automatically to a 8-9 Month Old May group.', 8, @Nursery_Month_8_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 6-7 Month Old (May)', 'Kids Club Age Group 6-7 Months, May birthday. This promotes automatically to a 7-8 Month Old May group.', 7, @Nursery_Month_7_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 5-6 Month Old (May)', 'Kids Club Age Group 5-6 Months, May birthday. This promotes automatically to a 6-7 Month Old May group.', 6, @Nursery_Month_6_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 4-5 Month Old (May)', 'Kids Club Age Group 4-5 Months, May birthday. This promotes automatically to a 5-6 Month Old May group.', 5, @Nursery_Month_5_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 3-4 Month Old (May)', 'Kids Club Age Group 3-4 Months, May birthday. This promotes automatically to a 4-5 Month Old May group.', 4, @Nursery_Month_4_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 2-3 Month Old (May)', 'Kids Club Age Group 2-3 Months, May birthday. This promotes automatically to a 3-4 Month Old May group.', 3, @Nursery_Month_3_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 1-2 Month Old (May)', 'Kids Club Age Group 1-2 Months, May birthday. This promotes automatically to a 2-3 Month Old May group.', 2, @Nursery_Month_2_Attribute_ID, @Month_May_Attribute_ID),
  ('Kids Club 0-1 Month Old (May)', 'Kids Club Age Group 0-1 Months, May birthday. This promotes automatically to a 1-2 Month Old May group.', 1, @Nursery_Month_1_Attribute_ID, @Month_May_Attribute_ID);

-- ====================================================================
-- June
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (June)', 'Kids Club Age Group 11-12 Months, June birthday. This promotes automatically to a 1 Year Old June group.', 12, @Nursery_Month_12_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 10-11 Month Old (June)', 'Kids Club Age Group 10-11 Months, June birthday. This promotes automatically to a 11-12 Month Old June group.', 11, @Nursery_Month_11_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 9-10 Month Old (June)', 'Kids Club Age Group 9-10 Months, June birthday. This promotes automatically to a 10-11 Month Old June group.', 10, @Nursery_Month_10_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 8-9 Month Old (June)', 'Kids Club Age Group 8-9 Months, June birthday. This promotes automatically to a 9-10 Month Old June group.', 9, @Nursery_Month_9_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 7-8 Month Old (June)', 'Kids Club Age Group 7-8 Months, June birthday. This promotes automatically to a 8-9 Month Old June group.', 8, @Nursery_Month_8_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 6-7 Month Old (June)', 'Kids Club Age Group 6-7 Months, June birthday. This promotes automatically to a 7-8 Month Old June group.', 7, @Nursery_Month_7_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 5-6 Month Old (June)', 'Kids Club Age Group 5-6 Months, June birthday. This promotes automatically to a 6-7 Month Old June group.', 6, @Nursery_Month_6_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 4-5 Month Old (June)', 'Kids Club Age Group 4-5 Months, June birthday. This promotes automatically to a 5-6 Month Old June group.', 5, @Nursery_Month_5_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 3-4 Month Old (June)', 'Kids Club Age Group 3-4 Months, June birthday. This promotes automatically to a 4-5 Month Old June group.', 4, @Nursery_Month_4_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 2-3 Month Old (June)', 'Kids Club Age Group 2-3 Months, June birthday. This promotes automatically to a 3-4 Month Old June group.', 3, @Nursery_Month_3_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 1-2 Month Old (June)', 'Kids Club Age Group 1-2 Months, June birthday. This promotes automatically to a 2-3 Month Old June group.', 2, @Nursery_Month_2_Attribute_ID, @Month_June_Attribute_ID),
  ('Kids Club 0-1 Month Old (June)', 'Kids Club Age Group 0-1 Months, June birthday. This promotes automatically to a 1-2 Month Old June group.', 1, @Nursery_Month_1_Attribute_ID, @Month_June_Attribute_ID);

-- ====================================================================
-- July
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (July)', 'Kids Club Age Group 11-12 Months, July birthday. This promotes automatically to a 1 Year Old July group.', 12, @Nursery_Month_12_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 10-11 Month Old (July)', 'Kids Club Age Group 10-11 Months, July birthday. This promotes automatically to a 11-12 Month Old July group.', 11, @Nursery_Month_11_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 9-10 Month Old (July)', 'Kids Club Age Group 9-10 Months, July birthday. This promotes automatically to a 10-11 Month Old July group.', 10, @Nursery_Month_10_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 8-9 Month Old (July)', 'Kids Club Age Group 8-9 Months, July birthday. This promotes automatically to a 9-10 Month Old July group.', 9, @Nursery_Month_9_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 7-8 Month Old (July)', 'Kids Club Age Group 7-8 Months, July birthday. This promotes automatically to a 8-9 Month Old July group.', 8, @Nursery_Month_8_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 6-7 Month Old (July)', 'Kids Club Age Group 6-7 Months, July birthday. This promotes automatically to a 7-8 Month Old July group.', 7, @Nursery_Month_7_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 5-6 Month Old (July)', 'Kids Club Age Group 5-6 Months, July birthday. This promotes automatically to a 6-7 Month Old July group.', 6, @Nursery_Month_6_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 4-5 Month Old (July)', 'Kids Club Age Group 4-5 Months, July birthday. This promotes automatically to a 5-6 Month Old July group.', 5, @Nursery_Month_5_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 3-4 Month Old (July)', 'Kids Club Age Group 3-4 Months, July birthday. This promotes automatically to a 4-5 Month Old July group.', 4, @Nursery_Month_4_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 2-3 Month Old (July)', 'Kids Club Age Group 2-3 Months, July birthday. This promotes automatically to a 3-4 Month Old July group.', 3, @Nursery_Month_3_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 1-2 Month Old (July)', 'Kids Club Age Group 1-2 Months, July birthday. This promotes automatically to a 2-3 Month Old July group.', 2, @Nursery_Month_2_Attribute_ID, @Month_July_Attribute_ID),
  ('Kids Club 0-1 Month Old (July)', 'Kids Club Age Group 0-1 Months, July birthday. This promotes automatically to a 1-2 Month Old July group.', 1, @Nursery_Month_1_Attribute_ID, @Month_July_Attribute_ID);

-- ====================================================================
-- August
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (August)', 'Kids Club Age Group 11-12 Months, August birthday. This promotes automatically to a 1 Year Old August group.', 12, @Nursery_Month_12_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 10-11 Month Old (August)', 'Kids Club Age Group 10-11 Months, August birthday. This promotes automatically to a 11-12 Month Old August group.', 11, @Nursery_Month_11_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 9-10 Month Old (August)', 'Kids Club Age Group 9-10 Months, August birthday. This promotes automatically to a 10-11 Month Old August group.', 10, @Nursery_Month_10_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 8-9 Month Old (August)', 'Kids Club Age Group 8-9 Months, August birthday. This promotes automatically to a 9-10 Month Old August group.', 9, @Nursery_Month_9_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 7-8 Month Old (August)', 'Kids Club Age Group 7-8 Months, August birthday. This promotes automatically to a 8-9 Month Old August group.', 8, @Nursery_Month_8_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 6-7 Month Old (August)', 'Kids Club Age Group 6-7 Months, August birthday. This promotes automatically to a 7-8 Month Old August group.', 7, @Nursery_Month_7_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 5-6 Month Old (August)', 'Kids Club Age Group 5-6 Months, August birthday. This promotes automatically to a 6-7 Month Old August group.', 6, @Nursery_Month_6_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 4-5 Month Old (August)', 'Kids Club Age Group 4-5 Months, August birthday. This promotes automatically to a 5-6 Month Old August group.', 5, @Nursery_Month_5_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 3-4 Month Old (August)', 'Kids Club Age Group 3-4 Months, August birthday. This promotes automatically to a 4-5 Month Old August group.', 4, @Nursery_Month_4_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 2-3 Month Old (August)', 'Kids Club Age Group 2-3 Months, August birthday. This promotes automatically to a 3-4 Month Old August group.', 3, @Nursery_Month_3_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 1-2 Month Old (August)', 'Kids Club Age Group 1-2 Months, August birthday. This promotes automatically to a 2-3 Month Old August group.', 2, @Nursery_Month_2_Attribute_ID, @Month_August_Attribute_ID),
  ('Kids Club 0-1 Month Old (August)', 'Kids Club Age Group 0-1 Months, August birthday. This promotes automatically to a 1-2 Month Old August group.', 1, @Nursery_Month_1_Attribute_ID, @Month_August_Attribute_ID);

-- ====================================================================
-- September
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (September)', 'Kids Club Age Group 11-12 Months, September birthday. This promotes automatically to a 1 Year Old September group.', 12, @Nursery_Month_12_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 10-11 Month Old (September)', 'Kids Club Age Group 10-11 Months, September birthday. This promotes automatically to a 11-12 Month Old September group.', 11, @Nursery_Month_11_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 9-10 Month Old (September)', 'Kids Club Age Group 9-10 Months, September birthday. This promotes automatically to a 10-11 Month Old September group.', 10, @Nursery_Month_10_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 8-9 Month Old (September)', 'Kids Club Age Group 8-9 Months, September birthday. This promotes automatically to a 9-10 Month Old September group.', 9, @Nursery_Month_9_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 7-8 Month Old (September)', 'Kids Club Age Group 7-8 Months, September birthday. This promotes automatically to a 8-9 Month Old September group.', 8, @Nursery_Month_8_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 6-7 Month Old (September)', 'Kids Club Age Group 6-7 Months, September birthday. This promotes automatically to a 7-8 Month Old September group.', 7, @Nursery_Month_7_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 5-6 Month Old (September)', 'Kids Club Age Group 5-6 Months, September birthday. This promotes automatically to a 6-7 Month Old September group.', 6, @Nursery_Month_6_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 4-5 Month Old (September)', 'Kids Club Age Group 4-5 Months, September birthday. This promotes automatically to a 5-6 Month Old September group.', 5, @Nursery_Month_5_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 3-4 Month Old (September)', 'Kids Club Age Group 3-4 Months, September birthday. This promotes automatically to a 4-5 Month Old September group.', 4, @Nursery_Month_4_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 2-3 Month Old (September)', 'Kids Club Age Group 2-3 Months, September birthday. This promotes automatically to a 3-4 Month Old September group.', 3, @Nursery_Month_3_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 1-2 Month Old (September)', 'Kids Club Age Group 1-2 Months, September birthday. This promotes automatically to a 2-3 Month Old September group.', 2, @Nursery_Month_2_Attribute_ID, @Month_September_Attribute_ID),
  ('Kids Club 0-1 Month Old (September)', 'Kids Club Age Group 0-1 Months, September birthday. This promotes automatically to a 1-2 Month Old September group.', 1, @Nursery_Month_1_Attribute_ID, @Month_September_Attribute_ID);

-- ====================================================================
-- October
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (October)', 'Kids Club Age Group 11-12 Months, October birthday. This promotes automatically to a 1 Year Old October group.', 12, @Nursery_Month_12_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 10-11 Month Old (October)', 'Kids Club Age Group 10-11 Months, October birthday. This promotes automatically to a 11-12 Month Old October group.', 11, @Nursery_Month_11_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 9-10 Month Old (October)', 'Kids Club Age Group 9-10 Months, October birthday. This promotes automatically to a 10-11 Month Old October group.', 10, @Nursery_Month_10_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 8-9 Month Old (October)', 'Kids Club Age Group 8-9 Months, October birthday. This promotes automatically to a 9-10 Month Old October group.', 9, @Nursery_Month_9_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 7-8 Month Old (October)', 'Kids Club Age Group 7-8 Months, October birthday. This promotes automatically to a 8-9 Month Old October group.', 8, @Nursery_Month_8_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 6-7 Month Old (October)', 'Kids Club Age Group 6-7 Months, October birthday. This promotes automatically to a 7-8 Month Old October group.', 7, @Nursery_Month_7_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 5-6 Month Old (October)', 'Kids Club Age Group 5-6 Months, October birthday. This promotes automatically to a 6-7 Month Old October group.', 6, @Nursery_Month_6_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 4-5 Month Old (October)', 'Kids Club Age Group 4-5 Months, October birthday. This promotes automatically to a 5-6 Month Old October group.', 5, @Nursery_Month_5_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 3-4 Month Old (October)', 'Kids Club Age Group 3-4 Months, October birthday. This promotes automatically to a 4-5 Month Old October group.', 4, @Nursery_Month_4_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 2-3 Month Old (October)', 'Kids Club Age Group 2-3 Months, October birthday. This promotes automatically to a 3-4 Month Old October group.', 3, @Nursery_Month_3_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 1-2 Month Old (October)', 'Kids Club Age Group 1-2 Months, October birthday. This promotes automatically to a 2-3 Month Old October group.', 2, @Nursery_Month_2_Attribute_ID, @Month_October_Attribute_ID),
  ('Kids Club 0-1 Month Old (October)', 'Kids Club Age Group 0-1 Months, October birthday. This promotes automatically to a 1-2 Month Old October group.', 1, @Nursery_Month_1_Attribute_ID, @Month_October_Attribute_ID);

-- ====================================================================
-- November
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (November)', 'Kids Club Age Group 11-12 Months, November birthday. This promotes automatically to a 1 Year Old November group.', 12, @Nursery_Month_12_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 10-11 Month Old (November)', 'Kids Club Age Group 10-11 Months, November birthday. This promotes automatically to a 11-12 Month Old November group.', 11, @Nursery_Month_11_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 9-10 Month Old (November)', 'Kids Club Age Group 9-10 Months, November birthday. This promotes automatically to a 10-11 Month Old November group.', 10, @Nursery_Month_10_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 8-9 Month Old (November)', 'Kids Club Age Group 8-9 Months, November birthday. This promotes automatically to a 9-10 Month Old November group.', 9, @Nursery_Month_9_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 7-8 Month Old (November)', 'Kids Club Age Group 7-8 Months, November birthday. This promotes automatically to a 8-9 Month Old November group.', 8, @Nursery_Month_8_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 6-7 Month Old (November)', 'Kids Club Age Group 6-7 Months, November birthday. This promotes automatically to a 7-8 Month Old November group.', 7, @Nursery_Month_7_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 5-6 Month Old (November)', 'Kids Club Age Group 5-6 Months, November birthday. This promotes automatically to a 6-7 Month Old November group.', 6, @Nursery_Month_6_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 4-5 Month Old (November)', 'Kids Club Age Group 4-5 Months, November birthday. This promotes automatically to a 5-6 Month Old November group.', 5, @Nursery_Month_5_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 3-4 Month Old (November)', 'Kids Club Age Group 3-4 Months, November birthday. This promotes automatically to a 4-5 Month Old November group.', 4, @Nursery_Month_4_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 2-3 Month Old (November)', 'Kids Club Age Group 2-3 Months, November birthday. This promotes automatically to a 3-4 Month Old November group.', 3, @Nursery_Month_3_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 1-2 Month Old (November)', 'Kids Club Age Group 1-2 Months, November birthday. This promotes automatically to a 2-3 Month Old November group.', 2, @Nursery_Month_2_Attribute_ID, @Month_November_Attribute_ID),
  ('Kids Club 0-1 Month Old (November)', 'Kids Club Age Group 0-1 Months, November birthday. This promotes automatically to a 1-2 Month Old November group.', 1, @Nursery_Month_1_Attribute_ID, @Month_November_Attribute_ID);

-- ====================================================================
-- December
INSERT INTO @KidsClubNurseryGroups VALUES
  ('Kids Club 11-12 Month Old (December)', 'Kids Club Age Group 11-12 Months, December birthday. This promotes automatically to a 1 Year Old December group.', 12, @Nursery_Month_12_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 10-11 Month Old (December)', 'Kids Club Age Group 10-11 Months, December birthday. This promotes automatically to a 11-12 Month Old December group.', 11, @Nursery_Month_11_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 9-10 Month Old (December)', 'Kids Club Age Group 9-10 Months, December birthday. This promotes automatically to a 10-11 Month Old December group.', 10, @Nursery_Month_10_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 8-9 Month Old (December)', 'Kids Club Age Group 8-9 Months, December birthday. This promotes automatically to a 9-10 Month Old December group.', 9, @Nursery_Month_9_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 7-8 Month Old (December)', 'Kids Club Age Group 7-8 Months, December birthday. This promotes automatically to a 8-9 Month Old December group.', 8, @Nursery_Month_8_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 6-7 Month Old (December)', 'Kids Club Age Group 6-7 Months, December birthday. This promotes automatically to a 7-8 Month Old December group.', 7, @Nursery_Month_7_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 5-6 Month Old (December)', 'Kids Club Age Group 5-6 Months, December birthday. This promotes automatically to a 6-7 Month Old December group.', 6, @Nursery_Month_6_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 4-5 Month Old (December)', 'Kids Club Age Group 4-5 Months, December birthday. This promotes automatically to a 5-6 Month Old December group.', 5, @Nursery_Month_5_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 3-4 Month Old (December)', 'Kids Club Age Group 3-4 Months, December birthday. This promotes automatically to a 4-5 Month Old December group.', 4, @Nursery_Month_4_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 2-3 Month Old (December)', 'Kids Club Age Group 2-3 Months, December birthday. This promotes automatically to a 3-4 Month Old December group.', 3, @Nursery_Month_3_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 1-2 Month Old (December)', 'Kids Club Age Group 1-2 Months, December birthday. This promotes automatically to a 2-3 Month Old December group.', 2, @Nursery_Month_2_Attribute_ID, @Month_December_Attribute_ID),
  ('Kids Club 0-1 Month Old (December)', 'Kids Club Age Group 0-1 Months, December birthday. This promotes automatically to a 1-2 Month Old December group.', 1, @Nursery_Month_1_Attribute_ID, @Month_December_Attribute_ID);

BEGIN TRANSACTION;

BEGIN TRY
  -- Delete groups created in Spring 2016 - not needed, never used, and will now be setup differently
  DELETE FROM Group_Rooms WHERE Group_ID IN (174000, 174001, 174002, 174003, 174004, 174005, 174006, 174007, 174008, 174009, 174010, 174011);
  DELETE FROM Event_Groups WHERE Group_ID IN (174000, 174001, 174002, 174003, 174004, 174005, 174006, 174007, 174008, 174009, 174010, 174011);
  DELETE FROM Group_Participants WHERE Group_ID IN (174000, 174001, 174002, 174003, 174004, 174005, 174006, 174007, 174008, 174009, 174010, 174011);
  DELETE FROM Groups WHERE Group_ID IN (174000, 174001, 174002, 174003, 174004, 174005, 174006, 174007, 174008, 174009, 174010, 174011);

  DECLARE KidsClubNurseryGroupsCursor CURSOR FOR
    SELECT * FROM @KidsClubNurseryGroups;

  DECLARE @Group_Name nvarchar(75), @Group_Description nvarchar(2000), @Age_In_Months_To_Promote int, @Nursery_Month_Attribute_ID int, @Month_Attribute_ID int, @Group_ID int, @Promote_Group_ID int;

  OPEN KidsClubNurseryGroupsCursor;
  FETCH NEXT FROM KidsClubNurseryGroupsCursor INTO
    @Group_Name, @Group_Description, @Age_In_Months_To_Promote, @Nursery_Month_Attribute_ID, @Month_Attribute_ID;

  WHILE @@FETCH_STATUS = 0
  BEGIN
    IF @Age_In_Months_To_Promote = 12
    BEGIN
      SET @Promote_Group_ID = CASE @Month_Attribute_ID
        WHEN @Month_January_Attribute_ID THEN 173988
        WHEN @Month_February_Attribute_ID THEN 173989
        WHEN @Month_March_Attribute_ID THEN 173990
        WHEN @Month_April_Attribute_ID THEN 173991
        WHEN @Month_May_Attribute_ID THEN 173992
        WHEN @Month_June_Attribute_ID THEN 173993
        WHEN @Month_July_Attribute_ID THEN 173994
        WHEN @Month_August_Attribute_ID THEN 173995
        WHEN @Month_September_Attribute_ID THEN 173996
        WHEN @Month_October_Attribute_ID THEN 173997
        WHEN @Month_November_Attribute_ID THEN 173998
        WHEN @Month_December_Attribute_ID THEN 173999
        ELSE null
      END;
    END
    ELSE
    BEGIN
      SET @Promote_Group_ID = @Group_ID;
    END

    SET @Group_ID = 0;
    SELECT @Group_ID=Group_ID FROM Groups WHERE Group_Name = @Group_Name;

    IF @Group_ID <= 0
    BEGIN
      PRINT 'Inserting new Group ' + @Group_Name;
      INSERT INTO Groups (
        group_name,
        group_type_id,
        Ministry_id,
        Congregation_id,
        Primary_contact,
        description,
        Start_date,
        domain_id,
        [Secure_Check-in],
        Suppress_Nametag,
        promotion_information,
        Promote_to_group,
        Age_in_Months_to_promote,
        Promote_Weekly
      ) VALUES (
        @Group_Name,
        @Group_Type_ID,
        @Ministry_ID,
        @Congregation_ID,
        @Contact_ID,
        @Group_Description,
        @Group_Start_Date,
        @Domain_ID,
        @Secure_Check_In,
        @Suppress_Nametag,
        NULL, -- Promotion Information
        @Promote_Group_ID,
        @Age_In_Months_To_Promote,
        @Promote_Weekly
      );
      SET @Group_ID = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
      PRINT 'Updating Existing Group ' + @Group_Name;
      UPDATE Groups
      SET group_name = @Group_Name,
        group_type_id = @Group_Type_ID,
        Ministry_id = @Ministry_ID,
        Congregation_id = @Congregation_ID,
        Primary_contact = @Contact_ID,
        description = @Group_Description,
        Start_date = @Group_Start_Date,
        domain_id = @Domain_ID,
        [Secure_Check-in] = @Secure_Check_In,
        Suppress_Nametag = @Suppress_Nametag,
        promotion_information = NULL,
        Promote_to_group = @Promote_Group_ID,
        Age_in_Months_to_promote = @Age_In_Months_To_Promote,
        Promote_Weekly = @Promote_Weekly
      WHERE Group_ID = @Group_ID;

      -- Delete month group attributes, if any
      DELETE FROM Group_Attributes WHERE Attribute_ID IN (
          @Nursery_Month_1_Attribute_ID,
          @Nursery_Month_2_Attribute_ID,
          @Nursery_Month_3_Attribute_ID,
          @Nursery_Month_4_Attribute_ID,
          @Nursery_Month_5_Attribute_ID,
          @Nursery_Month_6_Attribute_ID,
          @Nursery_Month_7_Attribute_ID,
          @Nursery_Month_8_Attribute_ID,
          @Nursery_Month_9_Attribute_ID,
          @Nursery_Month_10_Attribute_ID,
          @Nursery_Month_11_Attribute_ID,
          @Nursery_Month_12_Attribute_ID,

          @Month_January_Attribute_ID,
          @Month_February_Attribute_ID,
          @Month_March_Attribute_ID,
          @Month_April_Attribute_ID,
          @Month_May_Attribute_ID,
          @Month_June_Attribute_ID,
          @Month_July_Attribute_ID,
          @Month_August_Attribute_ID,
          @Month_September_Attribute_ID,
          @Month_October_Attribute_ID,
          @Month_November_Attribute_ID,
          @Month_December_Attribute_ID
        ) AND Group_ID = @Group_ID;
    END;

    -- Add appropriate nursery month attribute (0-1, 1-2, 2-3, etc)
    INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Nursery_Month_Attribute_ID, @Group_ID, @Domain_ID, @Group_Start_Date);
    -- Add appropriate birth month (January, February, March, etc)
    INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Month_Attribute_ID, @Group_ID, @Domain_ID, @Group_Start_Date);
    -- Add appropriate age attribute (nursery)
    INSERT INTO Group_Attributes (Attribute_ID, Group_ID, Domain_ID, Start_Date) VALUES (@Age_N_Attribute_ID, @Group_ID, @Domain_ID, @Group_Start_Date);

    FETCH NEXT FROM KidsClubNurseryGroupsCursor INTO
      @Group_Name, @Group_Description, @Age_In_Months_To_Promote, @Nursery_Month_Attribute_ID, @Month_Attribute_ID;
  END;

  CLOSE KidsClubNurseryGroupsCursor;
END TRY
BEGIN CATCH
  PRINT 'Rolling back transaction due to error ' + ERROR_MESSAGE();
  IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
END CATCH;

IF @@TRANCOUNT > 0
  COMMIT TRANSACTION;