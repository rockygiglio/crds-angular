USE [master]
GO

CREATE DATABASE AttendanceTracker
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

USE [AttendanceTracker]
GO
CREATE TABLE [dbo].[entry_type](
	[entry_type_id] [int] IDENTITY(1,1) NOT NULL,
	[entry_type_name] [nvarchar](255) NULL,
	[display_order] [int] NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[ministry](
	[ministry_id] [int] IDENTITY(1,1) NOT NULL,
	[ministry_name] [nvarchar](255) NULL,
	[display_order] [int] NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[service](
	[service_id] [int] IDENTITY(1,1) NOT NULL,
	[service_name] [nvarchar](255) NULL,
	[service_group] [nvarchar](255) NULL,
	[display_order] [int] NULL
) ON [PRIMARY]

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

CREATE TABLE [dbo].[Sessions](
	[sid] [nvarchar](32) NOT NULL,
	[expires] [datetime2](7) NULL,
	[data] [nvarchar](max) NULL,
	[createdAt] [datetime2](7) NOT NULL,
	[updatedAt] [datetime2](7) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

CREATE TABLE [dbo].[site](
	[site_id] [int] IDENTITY(1,1) NOT NULL,
	[site_name] [nvarchar](255) NULL,
	[display_order] [int] NULL
) ON [PRIMARY]

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

GO






