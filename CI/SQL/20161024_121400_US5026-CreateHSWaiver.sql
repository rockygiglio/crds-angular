USE MinistryPlatform
GO

IF NOT EXISTS(SELECT * FROM cr_waivers WHERE Waiver_Name = 'HS Summer Camp')
BEGIN

INSERT INTO cr_Waivers(Waiver_Name,Waiver_Text,Domain_ID)
VALUES('HS Summer Camp', '<h2>HS Summer Camp </h2>
<p>
LIABILITY RELEASE: RELEASE OF ALL CLAIMS:
</p>
<p>
I, the undersigned parent or legal guardian for the camp attendee, do hereby
release, forever discharge, and agree to indemnify and hold harmless Crossroads Church, its staff, board,
volunteers and all representatives thereof from any liability, claims, or demands of any kind, including, but
not limited to, personal injury, sickness, or death, as well as property damage and expenses of any nature
whatsoever which may result from my child’s participation at HS Summer Camp.
</p>
<p>
Furthermore, I agree to assume all responsibility for any of the previously mentioned occurrences. I
authorize the church to provide such food, transportation, and lodging (if applicable) as the church may
determine appropriate. I give my permission for my child to participate in the aforementioned activity, and
for any representative of the church to obtain medical treatment for my child, in the event this becomes
necessary, in the determination of that representative. I assume responsibility for any and all medical or
other costs incurred on behalf of my child. Should my child have to return home before the camp ends
for medical or disciplinary reasons, I hereby assume any costs incurred in accomplishing that.
</p>
<p>
AUTHORIZATION TO CREATE/USE VISUAL AND AUDIO RECORDINGS
</p>
<p>
I hereby grant to Crossroads Community Church (“Crossroads") the right and authority to take such
photographs and other visual recordings of my child, and to make such audio recordings of my child''s
voice, as is deemed appropriate by Crossroads for educational, recreational, or promotional purposes
relating Solely to Crossroads. Any visual and audio recordings Created pursuant to the foregoing grant of
authority may be published or displayed in mass media publications, in Crossroads'' newsletter, in television
or movie presentations, or on Crossroads'' internet website. My child''s full name may also be used in
Connection with any such use.
</p>
<p>
I further grant to Crossroads all right, title, and/or interest of any kind that my child or I may have in the
visual and audio recordings Created pursuant to the foregoing grant of authority. Upon creation, such
recordings shall become the sole property of Crossroads, subject to a right to revoke the authority to use
such recordings described below.
</p>
<p>
I hereby acknowledge that this authorization is made without promise of Compensation from Crossroads.
The grant of authority to use is effective until revoked by a writing signed by me (or by any person
Succeeding me as my student''s legal guardian, or by my student upon his or her attaining age 18) and
delivered to any appropriate Crossroads official. Any such revocation shall be effective to prevent use
arising only after receipt by such Crossroads official of the required written notice.
</p>',1)

END

GO
