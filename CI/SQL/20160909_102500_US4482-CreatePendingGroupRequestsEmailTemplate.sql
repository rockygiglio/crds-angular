USE [MinistryPlatform]
GO

DECLARE @AuthorUserID int = 5; -- Administrator, Church
DECLARE @FromContactID int = 7675411; -- groups@crossroads.net
DECLARE @ReplyToContactID int = 7675411; -- groups@crossroads.net
DECLARE @TemplateID int = 2004; -- Assigned by MPIdentityMaintenance.dbo.Get_Next_Available_ID
DECLARE @Body VARCHAR(max) = '<div>Hi [Nick_Name] [Last_Name],</div><div><br /></div><div>You have [Pending_Requests_Count] pending request(s) from people who want to join your &quot;[Group_Name]&quot; group. Log into your crossroads.net account, and go to your groups dashboard to view them, and approve or deny the group participants.</div><div><br /></div><div>https://[BaseUrl]/groups/mygroups/detail/[Group_ID]/requests</div><div><br /></div><div>All Pending Requests:</div><div>[Pending_Requests]</div><div><br /></div><div>Thanks!</div><div>Crossroads Groups Team</div><div><br /></div><div><i>This email was sent to: [All_Leaders]</i></div>';
DECLARE @Subject VARCHAR(max) = 'Pending Request(s) to Join Group';

IF NOT EXISTS (SELECT 1 FROM [dbo].[dp_Communications] WHERE Communication_ID = @TemplateID)
BEGIN
  SET IDENTITY_INSERT [dbo].[dp_Communications] ON
  INSERT INTO [dp_Communications]
  (
     [Communication_ID]
    ,[Author_User_ID]
    ,[Subject]
    ,[Body]
    ,[Domain_ID]
    ,[Start_Date]
    ,[From_Contact]
    ,[Reply_to_Contact]
    ,[Template]
    ,[Active]
  )
  VALUES
  (
     @TemplateID
    ,@AuthorUserID
    ,@Subject
    ,@Body
    ,1
    ,GetDate()
    ,@FromContactID
    ,@ReplyToContactID
    ,1
    ,1
  )

  SET IDENTITY_INSERT [dbo].[dp_Communications] OFF
END
ELSE
BEGIN
   UPDATE [dbo].[dp_Communications]
   SET 
      [Author_User_ID] = @AuthorUserID
      ,[Body] = @Body
      ,[Subject] = @Subject
      ,[From_Contact] = @FromContactID
      ,[Reply_to_Contact] = @ReplyToContactID
      ,[Template] = 1
      ,[Active] = 1
   WHERE Communication_ID = @TemplateID
END