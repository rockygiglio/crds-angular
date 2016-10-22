import CONSTANTS from 'crds-constants';
import ServeTeamMembersController from '../../app/my_serve/serve_team_members/serveTeamMembers.controller';

describe('ServeTeamMembersController', () => {
	let fixture,
		log,
		qApi,
		serveTeamService;

	beforeEach(angular.mock.module(CONSTANTS.MODULES.MY_SERVE));

	beforeEach(inject(function ($injector) {
		fixture = new ServeTeamMembersController();
	}));



	it('should build out allMembers data object', () => {
		fixture.opportunities = opportunitiesData;
		expect(fixture.opportunities.length).toBe(3, 'because this is before Not Available opp has been added');

		fixture.loadTeamMembers();

		expect(fixture.allMembers.length).toBe(4);
		expect(fixture.allMembers[3].members.length).toBe(3);
		expect(_.uniq(fixture.allMembers[3].members, 'Participant_ID').length).toBe(2);
		expect(fixture.opportunities.length).toBe(4, 'because a Not available opportunity is added');
	});

	var opportunitiesData = [{
		"Opportunity_ID": 2218723,
		"Opportunity_Title": "(t) Coffee Setup Sat 2:00",
		"Group_Role_ID": 16,
		"rsvpMembers": [{
			"Event_ID": 4510383,
			"Participant_ID": 7554224,
			"Opportunity_ID": 2218723,
			"Group_Role_ID": 16,
			"NickName": "Heather",
			"Last_Name": "Kerstanoff",
			"Response_Result_ID": 2,
			"age": 18
		}, {
			"Event_ID": 4510383,
			"Participant_ID": 7537153,
			"Opportunity_ID": 2218723,
			"Group_Role_ID": 16,
			"NickName": "Joe",
			"Last_Name": "Kerstanoff",
			"Response_Result_ID": 1,
			"age": 31
		}
		]
	}, {
		"Opportunity_ID": 2218724,
		"Opportunity_Title": "(t) Coffee Team Sat 4:00",
		"Group_Role_ID": 16,
		"rsvpMembers": [{
			"Event_ID": 4510383,
			"Participant_ID": 7554224,
			"Opportunity_ID": 2218724,
			"Group_Role_ID": 16,
			"NickName": "Heather",
			"Last_Name": "Kerstanoff",
			"Response_Result_ID": 2,
			"age": 18
		}
		]
	}, {
		"Opportunity_ID": 2218725,
		"Opportunity_Title": "(t) Coffee Leader Sat 4:00",
		"Group_Role_ID": 22,
		"rsvpMembers": [{
			"Event_ID": 4510383,
			"Participant_ID": 7554225,
			"Opportunity_ID": 2218723,
			"Group_Role_ID": 22,
			"NickName": "Paige",
			"Last_Name": "Kerstanoff",
			"Response_Result_ID": 2,
			"age": 13
		}, {
			"Event_ID": 4510383,
			"Participant_ID": 7537155,
			"Opportunity_ID": 2218725,
			"Group_Role_ID": 22,
			"NickName": "James",
			"Last_Name": "Kerstanoff",
			"Response_Result_ID": 1,
			"age": 15
		}
		]
	}];
});