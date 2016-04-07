USE [MinistryPlatform];

--SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] ON;

INSERT INTO [dbo].[dp_Sub_Pages] (
     [Display_Name]
    ,[Singular_Name]
    ,[Page_ID]
    ,[View_Order]
    ,[Link_To_Page_ID]
    ,[Link_From_Field_Name]
    ,[Select_To_Page_ID]
    ,[Select_From_Field_Name]
    ,[Primary_Table]
    ,[Primary_Key]
    ,[Default_Field_List]
    ,[Selected_Record_Expression]
    ,[Filter_Key]
    ,[Relation_Type_ID]
    ,[On_Quick_Add]
    ,[Contact_ID_Field]
    ,[Default_View]
    ,[System_Name]
    ,[Date_Pivot_Field]
    ,[Start_Date_Field]
    ,[End_Date_Field]
    ,[Custom_Form_Name]
    ,[Display_Copy]
) VALUES (
    'Group Connectors'
    ,'Group Connector'
    ,16
    ,10
    ,13
    ,null
    ,null
    ,null
    ,'cr_Group_Connector_Registrations'
    ,'GroupConnectorRegistration_ID'
    ,'Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[First_Name] AS [First Name] , Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Last_Name] AS [Last Name] , Group_Connector_ID_Table_Primary_Registration_Table_Participant_ID_Table_Contact_ID_Table.[Email_Address] AS [Email Address]'
    ,'Group_Connector_Registration_ID'
    ,'Group_Connector_Registration_ID'
    ,2
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,NULL
    ,1
);

--SET IDENTITY_INSERT [dbo].[dp_Sub_Pages] OFF;