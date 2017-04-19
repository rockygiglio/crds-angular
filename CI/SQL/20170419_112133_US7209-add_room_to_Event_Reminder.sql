USE [MinistryPlatform]
GO

UPDATE [dbo].[dp_Communications]
   SET 
      [Body] = '<p class="rally-rte-class-002849d41"><font face="arial, sans-serif"><span style="font-size: 12px;"></span></font></p> <p class="rally-rte-class-002849d41" style="font-family: arial, sans-serif; font-size: 12px;"><font face="arial, sans-serif">Your event<b> [Event_Title]</b> scheduled is approaching:</font><span class="Apple-tab-span" style="color: rgb(51, 51, 51); white-space: pre;">	</span><span style="color: rgb(51, 51, 51); font-family: arial, sans, sans-serif; white-space: pre-wrap;"> 	</span></p><ul style="color: rgb(51, 51, 51); font-family: arial, sans, sans-serif; font-size: 12px; line-height: 17.5px; white-space: pre-wrap;"><li><b><font face="arial, sans-serif">Date:  [Event_Start_Date] </font>	</b></li><li><b><font face="arial, sans-serif">Time:  [Event_Start_Time] </font>		</b></li><li><b><font face="arial, sans-serif">Room:  [Room_Name] </font> </b>   </li></ul> <p class="rally-rte-class-002849d41"><font face="arial, sans-serif"><span style="font-size: 12px;"></span></font></p><p class="rally-rte-class-002849d41" style="font-family: arial, sans-serif; font-size: 12px;"><font face="arial, sans-serif">If you would like to change or cancel a room reservation, please contact one of our reservation specialists below</font><span class="Apple-tab-span" style="white-space: pre;">	</span></p> <ul class="rally-rte-class-002849d41" style="font-family: arial, sans-serif; font-size: 12px;"> 	<li><font face="arial, sans-serif">Florence - Brianna Quinn</font></li> 	<li><font face="arial, sans-serif">Mason - Eboni Perkins</font></li> 	<li><font face="arial, sans-serif">Oakley - Monica Faison    <span class="Apple-tab-span" style="white-space:pre">	</span></font></li><li><font face="arial, sans-serif">West Side - Michelle Rueve </font></li> 	<li><font face="arial, sans-serif">Uptown - Meghan Pope</font></li> </ul> <div> 	<p class="rally-rte-class-002849d41"><font face="arial, sans-serif"><span style="font-size: 12px;"><a href="https://admin.crossroads.net/ministryplatform/#/308/[Event_ID]">View event details</a></span></font></p> 	<p class="rally-rte-class-002849d41"><font face="arial, sans-serif"><span style="font-size: 12px;"></span></font></p> </div>'
 WHERE [Communication_ID] = 14909
GO


