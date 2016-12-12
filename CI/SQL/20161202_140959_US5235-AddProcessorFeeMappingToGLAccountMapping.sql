use MinistryPlatform;
Go


IF NOT EXISTS ( SELECT * FROM sys.columns WHERE Name = N'Processor_Fee_Mapping_ID' AND Object_ID = Object_ID(N'dbo.GL_Account_Mapping'))
BEGIN
	ALTER TABLE [dbo].[GL_Account_Mapping] 
		ADD Processor_Fee_Mapping_ID int null
END

IF NOT EXISTS (SELECT * FROM sys.objects o WHERE o.object_id = object_id(N'[dbo].[FK_GL_Account_Mapping_GL_Account_Mapping]') AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
BEGIN
    ALTER TABLE [dbo].[GL_Account_Mapping] WITH CHECK ADD CONSTRAINT [FK_GL_Account_Mapping_GL_Account_Mapping] FOREIGN KEY([Processor_Fee_Mapping_ID]) REFERENCES [dbo].[GL_Account_Mapping] ([GL_Account_Mapping_ID])
END