USE [master]
GO

IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'AttendanceTracker')
BEGIN 

CREATE DATABASE AttendanceTracker;

END
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON


USE [AttendanceTracker]

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'entry_type' AND type = 'U')
BEGIN

CREATE TABLE [dbo].[entry_type](
	[entry_type_id] [int] IDENTITY(1,1) NOT NULL,
	[entry_type_name] [nvarchar](255) NULL,
	[display_order] [int] NULL
) ON [PRIMARY]

SET IDENTITY_INSERT [dbo].[entry_type] ON 
INSERT [dbo].[entry_type] ([entry_type_id], [entry_type_name], [display_order]) VALUES (1, N'Attendees', 0)
INSERT [dbo].[entry_type] ([entry_type_id], [entry_type_name], [display_order]) VALUES (2, N'Volunteers', 1)
INSERT [dbo].[entry_type] ([entry_type_id], [entry_type_name], [display_order]) VALUES (3, N'Paid', 2)
SET IDENTITY_INSERT [dbo].[entry_type] OFF


END 

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ministry' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[ministry](
	[ministry_id] [int] IDENTITY(1,1) NOT NULL,
	[ministry_name] [nvarchar](255) NULL,
	[display_order] [int] NULL
) ON [PRIMARY]

SET IDENTITY_INSERT [dbo].[ministry] ON 
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (1, N'Adults', 0)
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (2, N'Kids Club', 1)
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (3, N'Middle School', 2)
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (4, N'High School', 3)
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (5, N'BIG', 4)
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (6, N'Streaming', 5)
SET IDENTITY_INSERT [dbo].[ministry] OFF

END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'service' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[service](
	[service_id] [int] IDENTITY(1,1) NOT NULL,
	[service_name] [nvarchar](255) NULL,
	[service_group] [nvarchar](255) NULL,
	[display_order] [int] NULL
) ON [PRIMARY]
SET IDENTITY_INSERT [dbo].[service] ON 

INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (1, N'Saturday 1', NULL, 0)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (2, N'Saturday 2', NULL, 1)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (3, N'Saturday 3', NULL, 2)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (4, N'Sunday 1', NULL, 3)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (5, N'Sunday 2', NULL, 4)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (6, N'Sunday 3', NULL, 5)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (7, N'Sunday 4', NULL, 6)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (8, N'Special Event', NULL, 7)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (9, N'Mid-Week Service', NULL, 8)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (10, N'Student Ministry Camp', NULL, 9)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (11, N'Man Camp', NULL, 10)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (12, N'Woman Camp', NULL, 11)
INSERT [dbo].[service] ([service_id], [service_name], [service_group], [display_order]) VALUES (14, N'On Demand', NULL, 12)
SET IDENTITY_INSERT [dbo].[service] OFF
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'service_instance' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[service_instance](
	[service_instance_id] [int] IDENTITY(1,1) NOT NULL,
	[service_instance_map_id] [int] NULL,
	[created_user_id] [int] NULL,
	[created_date] [datetime2](7) NULL,
	[edited_user_id] [int] NULL,
	[edited_date] [datetime2](7) NULL,
	[site_id] [int] NULL,
	[ministry_id] [int] NULL,
	[service_id] [int] NULL,
	[date_of_service] [datetime2](7) NULL,
	[entry_type_id] [int] NULL,
	[entry_value] [nvarchar](255) NULL,
	[notes] [nvarchar](255) NULL
) ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Sessions' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[Sessions](
	[sid] [nvarchar](32) NOT NULL,
	[expires] [datetime2](7) NULL,
	[data] [nvarchar](max) NULL,
	[createdAt] [datetime2](7) NOT NULL,
	[updatedAt] [datetime2](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'site' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[site](
	[site_id] [int] IDENTITY(1,1) NOT NULL,
	[site_name] [nvarchar](255) NULL,
	[display_order] [int] NULL
) ON [PRIMARY]SET IDENTITY_INSERT [dbo].[site] ON 

INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (1, N'Oakley', 0)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (2, N'Mason', 1)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (3, N'Florence', 2)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (4, N'West Side', 3)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (5, N'Uptown', 4)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (6, N'Oxford', 5)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (7, N'East Side', 6)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (8, N'Anywhere', 7)
SET IDENTITY_INSERT [dbo].[site] OFF
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'user' AND type = 'U')
BEGIN
CREATE TABLE [dbo].[user](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[ministry_platform_id] [int] NULL,
	[first_name] [nvarchar](255) NULL,
	[last_name] [nvarchar](255) NULL,
	[email] [nvarchar](255) NULL,
	[first_active] [datetime2](7) NULL,
	[last_active] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END


GO






