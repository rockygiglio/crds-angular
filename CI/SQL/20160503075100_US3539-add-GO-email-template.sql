USE [MinistryPlatform]
GO

INSERT INTO [dbo].[dp_Communications]
           (
			[Author_User_ID],
			[Subject],
			[Body],
			[Domain_ID],
			[Start_Date],
			[Communication_Status_ID],
			[From_Contact],
			[Reply_to_Contact],
			[Template],
			[Active]
		  )
     VALUES
           (
			5,
			'Your Project for GO Cincinnati 2016',
			'<div style="font-family: Verdana; font-size: 12px;"><div>Hello [Volunteer First Name]!</div><div><br /></div><div>We''re so excited to serve our partner organizations with you! We''re going to have <b>8000+ people</b> from churches all over the city serving at <b>over 500 different project</b> sites. It will be a day to remember!!</div><div><br /></div><div>The info below will help paint a picture of the day (did we mention we love to paint?). </div><div><br /></div><div>To honor the volunteers who are coordinating project assignments, we will not be able to make any changes after May 14th at 5pm. Thanks for understanding!</div><div><br /></div><div><br /></div><div><b>What are the details of the day?</b></div><div>First, set your alarm clock nice and early and please <b>eat a big lumberjack breakfast before you arrive</b> and pack any water you''ll need during the day. Water is not provided.</div><div><br /></div><div>Because we are a team and we want to look like a team, <b>arrive wearing a plain white t-shirt </b>or sweatshirt. It''s a tradition so don''t be the weenie in green!</div><div><br /></div><div><b>Doors open at 7am! You''ll arrive and check in no later than 7:30am at:</b></div><div>[Launch Site]</div><div>[Launch Site Address]</div><div><br /></div><div><b>Check in at:</b></div><div>[Check-In Floor #]  [Check-In Area]  [Check-In Room Number] to let your Team Captain know you are here and ready to GO!</div><div><br /></div><div>After we check in, we''ll all head into the auditorium for a rockin'' launch service and then head out to our work sites. We''ll all work from 9 am to 1 pm.</div><div><br /></div><div><b>Who am I serving with?</b></div><div>Your Team Captain is [TC Name]. In case of emergency (only, please), you can contact them at [TC Contact].</div><div><br /></div><div>You told us your Group Connector is: <b><i>[Group Connector Name]</i></b> so we''ve signed you up accordingly.</div><div><br /></div><div>If you''ve signed up with your spouse and kids on the same form: No one else will receive this email except you so PLEASE let them know the details!</div><div><br /></div><div><b>What''s my project?</b></div><div>Your project is [Project Name] and you''ll be doing a(n) [Project Type] project. [Note To Volunteers 1]  [Note To Volunteers 2]</div><div><br /></div><div>A great way to prepare for the day is to learn more about what this organization does by Googling them and then praying for them.</div><div><br /></div><div><b>Where''s my project?</b></div><div>Your project site is located at [Project Street Address], [Project City], [Project State], [Project Zip]. If you need a map from Crossroads to your site, please print one before the big day and bring it with you. Maps are not provided. When you arrive at the site, please park [Parking Location].</div><div><br /></div><div><b>Parking at Crossroads</b></div><div>If your project is close to Crossroads, please park close to the main building to allow others with projects farther to park in the outer areas of the parking lots.</div><div><br /></div><div><b>Is that it?</b></div><div>We think that''s everything you need to know for GO Cincinnati 2016. Can''t wait for you to join us as we blitz our city with compassion! </div><div><br /></div><div>Excited,</div><div>The GO Cincinnati 2016 Lead Team</div></div>',
			1,
			'2016-4-1',
			1,
			7542465,
			7542465,
			1,
			1
		   )
GO