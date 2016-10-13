import CONSTANTS from 'crds-constants';
import ServeTeamMembersController from '../../app/my_serve/serve_team_members/serveTeamMembers.controller';

describe('ServeTeamMembersController', () => {
    let fixture,
    log,
    qApi,
    serveTeamService;

    beforeEach(angular.mock.module(CONSTANTS.MODULES.MY_SERVE));

    beforeEach(inject(function ($injector) {
       // log = $injector.get('$log');
        serveTeamService = $injector.get('ServeTeamService');
        //qApi = $injector.get('$q');

        fixture = new ServeTeamMembersController(serveTeamService);
    }));



    fit('should build out allMembers data object', () => {
         fixture.team = teamData;
         fixture.loadTeamMembers(fixture.team);
		 
         expect(fixture.allMembers.length).toBe(4);
         expect(fixture.rsvpYesLeaders.length).toBe(1);
         expect(fixture.rsvpNoMembers.length).toBe(3);
		 expect(_.uniq(fixture.rsvpNoMembers, 'Participant_ID').length).toBe(2);
    });

    var teamData = {
	"groupId" : 176822,
    "eventId" : 4510383,
	"eventType" : "Service - Oakley Saturday at 4:30pm",
	"eventTypeId" : 94,
	"index" : 1,
	"name" : "(t) FI Oakley Coffee Team",
	"primaryContact" : "lizett.trujillo@ingagepartners.com",
	"pastDeadline" : true,
	"pastDeadlineMessage" : 53,
	"serveOppertunities" : [{
			"Opportunity_ID" : 2218723,
			"Opportunity_Title" : "(t) Coffee Setup Sat 2:00",
			"Group_Role_ID" : 16,
			"rsvpMembers" : [{
					"Event_ID" : 4510383,
					"Participant_ID" : 7554224,
					"Opportunity_ID" : 2218723,
					"Group_Role_ID" : 16,
					"NickName" : "Heather",
					"Last_Name" : "Kerstanoff",
					"Response_Result_ID" : 2
				}, {
					"Event_ID" : 4510383,
					"Participant_ID" : 7537153,
					"Opportunity_ID" : 2218723,
					"Group_Role_ID" : 16,
					"NickName" : "Joe",
					"Last_Name" : "Kerstanoff",
					"Response_Result_ID" : 1
				}
			]
		}, {
			"Opportunity_ID" : 2218724,
			"Opportunity_Title" : "(t) Coffee Team Sat 4:00",
			"Group_Role_ID" : 16,
			"rsvpMembers" : [{
					"Event_ID" : 4510383,
					"Participant_ID" : 7554224,
					"Opportunity_ID" : 2218724,
					"Group_Role_ID" : 16,
					"NickName" : "Heather",
					"Last_Name" : "Kerstanoff",
					"Response_Result_ID" : 2
				}
			]
		},{
			"Opportunity_ID" : 2218725,
			"Opportunity_Title" : "(t) Coffee Leader Sat 4:00",
			"Group_Role_ID" : 22,
			"rsvpMembers" : [{
					"Event_ID" : 4510383,
					"Participant_ID" : 7554225,
					"Opportunity_ID" : 2218723,
					"Group_Role_ID" : 22,
					"NickName" : "Paige",
					"Last_Name" : "Kerstanoff",
					"Response_Result_ID" : 2
				}, {
					"Event_ID" : 4510383,
					"Participant_ID" : 7537155,
					"Opportunity_ID" : 2218725,
					"Group_Role_ID" : 22,
					"NickName" : "James",
					"Last_Name" : "Kerstanoff",
					"Response_Result_ID" : 1
				}
			]
		}
	],
	"rsvpYesCount" : 2
};
});