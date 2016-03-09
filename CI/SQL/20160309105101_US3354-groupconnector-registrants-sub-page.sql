USE MinistryPlatform;
GO

DECLARE @SUB_PAGE_ID int = 4;
DECLARE @DISPLAY_NAME NVARCHAR(50) = 'Registrations';
DECLARE @SINGULAR_NAME NVARCHAR(50) = 'Registration';
DECLARE @PAGE_ID INT = 13;
DECLARE @VIEW_ORDER SMALLINT = 10;
DECLARE @PRIMARY_TABLE NVARCHAR(50) = 'cr_GroupConnectorRegistrations';
DECLARE @PRIMARY_KEY NVARCHAR(50) = 'GroupConnectorRegistration_ID';
DECLARE @SELECTED_RECORD_EXPRESSION NVARCHAR(500) = 'Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[Display_Name]';
DECLARE @FILTER_KEY NVARCHAR(50) = 'GroupConnector_ID';
DECLARE @Relation_Type_ManyToMany INT = '2';
DECLARE @DISPLAY_COPY BIT = 0;
DECLARE @ACCESS_LEVEL_FULL INT = 3;
DECLARE @ROLE_ID INT = 107; --System Administrator - CRDS

DECLARE @DEFAULT_FIELD_LIST NVARCHAR(4000) = N'Registration_ID_Table_Participant_ID_Table_Contact_ID_Table.[Display_Name] AS [Registrant]';

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON;

INSERT INTO [dbo].[dp_Sub_Pages]
    ([Sub_Page_ID]
    ,[Display_Name]
    ,[Singular_Name]
    ,[Page_ID]
    ,[View_Order]
    ,[Primary_Table]
    ,[Default_Field_List]
    ,[Selected_Record_Expression]
    ,[Filter_Key]
    ,[Relation_Type_ID]
    ,[Display_Copy]
)
VALUES
    (@SUB_PAGE_ID
    ,@DISPLAY_NAME
    ,@SINGULAR_NAME
    ,@PAGE_ID
    ,@VIEW_ORDER
    ,@PRIMARY_TABLE
    ,@DEFAULT_FIELD_LIST
    ,@SELECTED_RECORD_EXPRESSION
    ,@FILTER_KEY
    ,@Relation_Type_ManyToMany
    ,@DISPLAY_COPY
);

SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF;
