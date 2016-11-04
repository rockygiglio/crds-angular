USE MinistryPlatform
GO

IF NOT EXISTS(SELECT * FROM cr_waivers WHERE Waiver_Name = 'Camp Chautaqua')
BEGIN

INSERT INTO cr_Waivers(Waiver_Name,Waiver_Text,Domain_ID)
VALUES('Camp Chautaqua', '<h2>PARTICIPANT RELEASE FORM</h2>
<p>
Participation Release
</p>
<p>
I, as the participant and/or the legal parent of a participant, do hereby indemnify and hold harmless Camp Chautauqua, and Buckeye
Blitz Paintball, and their officers, directors, agents, employees, volunteers and representatives (the “Indemnified Parties”) from and
against any and all liability, damages, actions, cause of action, claims, losses and/or expenses, including but not limited to attorney’s
fees, court costs and expenses, arising in connections with or based on injury to or death of any persons or property, including the
loss of use thereof, caused in whole or in part by any member of the Group or the Summer Camp Directorship, regardless of whether
or not caused in whole or in part by the negligence of the indemnified parties, or any one or more of them. However, this
indemnification shall not apply to willful misconduct committed by the Indemnified Parties. I understand that part of the camping
experience involves activities and group living arrangements and interactions that may be new to myself or my child, and that they
come with certain risks and uncertainties beyond what I and/or my child may be used to dealing with at home. I am aware of these
risks, and I am assuming them (on behalf of my child). I realize that no environment is risk free, and so I acknowledge and/or have
instructed my child on the importance of abiding by the camp’s rules, and I and my child both agree that I and he or she is familiar
with these rules and will obey them.
</p>
<p>
Recreation
</p>
<p>
The recreation programs at event venues strive to offer fun, safe, and challenging activities that engage the whole person – body,
mind and soul. Program staff are trained and as a team committed to your rewarding experience with safety as their highest priority.
They have done everything possible to mitigate any risks involved in their recreation programs. However there are inherent risks to
participation in recreation activities, including but not limited to, initiative games, zip line, climbing or descending unpredictable and
possibly slick or uneven terrain, running, jumping, and unforeseen forces of nature or weather, outdoor activities, and aquatics, any of
which could result in elevated heart and respiratory rates, an injury/illness that could result in loss of life, limb, and/or property.
</p>
<p>
Rules and Regulations
</p>
<p>
I (and my child) will obey the rules and regulations of Camp Chautauqua and will cooperate with the leaders and campers.
</p>
<p>
Medical Waiver
</p>
<p>
Should a parent/guardian not be present, I hereby authorize The Chautauqua staff, Camp Health Officer or Summer Camp
Directorship to make emergency medical decisions for me and/or my child and I understand that my insurance coverage will be
primary coverage. If the church or group I and/or my child is attending with has insurance the respective church or group is second
coverage and Chautauqua will be third and for accidents only­no illness coverage.
</p>
<p>
Registration
</p>
<p>
All information gained is for use for the event scheduled at Camp Chautauqua and is not shared or sold to other entities.
</p>
<p>
Understanding
</p>
<p>
I represent and acknowledge that I have completely read and understand this document and all its terms and all matters referred to
herein, and I signed voluntarily as my free act and deed, that I have had an ample opportunity to obtain the advice of counsel and
that, by signing this document, I understand that I am relinquishing legal rights and remedies that may have otherwise been available
to me. I understand that this Waiver and Release shall be construed as broadly and inclusively as is permitted by applicable law and
agree that if any portion of this document is held invalid, the remaining shall continue in full force and effect. To the extent the
restriction on filing lawsuits is deemed unlawful, I agree to submit any Claims to a Christian conciliation/mediation organization for
binding resolution.
</p>
',1)

END

GO
