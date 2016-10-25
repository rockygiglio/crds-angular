USE [AttendanceTracker]
GO
SET IDENTITY_INSERT [dbo].[entry_type] ON 

INSERT [dbo].[entry_type] ([entry_type_id], [entry_type_name], [display_order]) VALUES (1, N'Attendees', 0)
INSERT [dbo].[entry_type] ([entry_type_id], [entry_type_name], [display_order]) VALUES (2, N'Volunteers', 1)
INSERT [dbo].[entry_type] ([entry_type_id], [entry_type_name], [display_order]) VALUES (3, N'Paid', 2)
SET IDENTITY_INSERT [dbo].[entry_type] OFF
SET IDENTITY_INSERT [dbo].[ministry] ON 

INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (1, N'Adults', 0)
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (2, N'Kids Club', 1)
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (3, N'Middle School', 2)
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (4, N'High School', 3)
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (5, N'BIG', 4)
INSERT [dbo].[ministry] ([ministry_id], [ministry_name], [display_order]) VALUES (6, N'Streaming', 5)
SET IDENTITY_INSERT [dbo].[ministry] OFF
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
SET IDENTITY_INSERT [dbo].[site] ON 

INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (1, N'Oakley', 0)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (2, N'Mason', 1)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (3, N'Florence', 2)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (4, N'West Side', 3)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (5, N'Uptown', 4)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (6, N'Oxford', 5)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (7, N'East Side', 6)
INSERT [dbo].[site] ([site_id], [site_name], [display_order]) VALUES (8, N'Anywhere', 7)
SET IDENTITY_INSERT [dbo].[site] OFF

GO