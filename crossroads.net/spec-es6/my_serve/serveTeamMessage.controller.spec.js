import CONSTANTS from 'crds-constants';
import ServeTeamMessageController from '../../app/my_serve/serve_team_message/serveTeamMessage.controller';

describe('GroupMessageController', () => {
	let fixture,
		rootScope,
		log,
		state,
		qApi,
		serveTeamService;

	beforeEach(angular.mock.module(CONSTANTS.MODULES.MY_SERVE));

	beforeEach(inject(function ($injector) {
		rootScope = $injector.get('$rootScope');
		log = $injector.get('$log');
		state = $injector.get('$state');
		serveTeamService = $injector.get('ServeTeamService');
		qApi = $injector.get('$q');
		fixture = new ServeTeamMessageController(serveTeamService, log, rootScope, state);
	}));

	it('$onInit should properly initialize tinymce html editor', () => {
		spyOn(serveTeamService, 'getTeamDetailsByLeader').and.callFake(function () {
			var deferred = qApi.defer();
			deferred.resolve('Remote call result');
			return deferred.promise;
		});

		fixture.$onInit();

		expect(fixture.tinymceOptions.resize).toEqual(false);
		expect(fixture.tinymceOptions.height).toEqual(300);
		expect(fixture.tinymceOptions.menubar).toEqual(false);
		expect(fixture.tinymceOptions.statusbar).toEqual(false);
		expect(fixture.tinymceOptions.plugins).toEqual('paste link legacyoutput textcolor');
		expect(fixture.tinymceOptions.valid_elements).toEqual('ol,ul,li,p,br,strong/b,i,em,a[href|target=_blank],p,br');
		expect(fixture.tinymceOptions.toolbar).toEqual('undo redo | fontselect fontsizeselect forecolor backcolor | bold italic underline | alignleft aligncenter alignright | numlist bullist | link');

	});
});