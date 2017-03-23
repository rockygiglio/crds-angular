USE [MinistryPlatform]
GO

--UPDATE FAILED TO PROCESS: Add batchName to Subject
UPDATE dp_Communications
SET Subject = 'Check Scanner Batch [batchName] Failed to Process'
WHERE
Template = 1
AND Communication_ID = 12260

--UPDATE SUCESSFULLY PROCESSED: Add batchName to Subject and remove Success
UPDATE dp_Communications
SET Subject = 'Check Scanner Batch - [batchName]',
    Body = 'Processed check scanner batch [batchName] with checks for program id [programId].<div><br /></div>Batch Result:<br /><pre>[batch]</pre>'
WHERE
Template = 1
AND Communication_ID = 12259