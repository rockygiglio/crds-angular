import CONSTANTS from 'crds-constants';
import ServeTeamContainerController from '../../app/my_serve/serve_team_container/serveTeamContainer.controller';

describe('ServeTeamContainerController', () => {
	let fixture,
		qApi,
		serveTeamService;

	beforeEach(angular.mock.module(CONSTANTS.MODULES.MY_SERVE));

	beforeEach(inject(function ($injector) {
		serveTeamService = $injector.get('ServeTeamService');
		qApi = $injector.get('$q');
		fixture = new ServeTeamContainerController(serveTeamService, qApi);
	}));



	it('$onInit should call getIsLeader and getTeamRsvps', () => {
		spyOn(serveTeamService, 'getIsLeader').and.callFake(function () {
			var deferred = qApi.defer();
			deferred.resolve(true);
			return deferred.promise;
		});

		spyOn(serveTeamService, 'getTeamRsvps').and.callFake(function () {
			var deferred = qApi.defer();
			deferred.resolve({Opportunity_ID: 1234});
			return deferred.promise;
		});

		fixture.team = {groupId:'123'};

		fixture.$onInit();

		expect(serveTeamService.getIsLeader).toHaveBeenCalled();
		expect(serveTeamService.getTeamRsvps).toHaveBeenCalled();

	});
});