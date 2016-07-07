import constants from 'crds-constants';
import CreateGroupController from '../../../app/group_tool/create_group/createGroup.controller'

describe('CreateGroupController', () => {
    let fixture,
        participantService,
        location,
        log,
        deferred,
        scope;

    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

    beforeEach(inject(function($injector) {
        participantService = $injector.get('Participant'); 
        location = $injector.get('$location');
        log = $injector.get('$log');
        deferred = $injector.get('$q');
        scope = $injector.get('$rootScope');

        fixture = new CreateGroupController(participantService, location, log);
    }));

    describe('the constructor', () => {
        it('should initialize properties', () => {
            expect(fixture.ready).toBeFalsy();
            expect(fixture.approvedLeader).toBeFalsy();
        });
    });

    describe('$onInit() function', () => {
        it('should set properties if leader is approved', () => {
            var promised = deferred.defer();
            promised.resolve({'ApprovedSmallGroupLeader': true});
            spyOn(participantService, 'get').and.returnValue(promised.promise);

            fixture.$onInit();
            scope.$digest();
            expect(fixture.ready).toBeTruthy();
            expect(fixture.approvedLeader).toBeTruthy();
        });

        it('should redirect to leader application if leader is not approved', () => {
            var promised = deferred.defer();
            promised.resolve({'ApprovedSmallGroupLeader': false});
            spyOn(participantService, 'get').and.returnValue(promised.promise);
            spyOn(location, 'path').and.callFake(function() { });

            fixture.$onInit();
            scope.$digest();

            expect(fixture.ready).toBeFalsy();
            expect(fixture.approvedLeader).toBeFalsy();
            expect(location.path).toHaveBeenCalledWith('/grouptool/leader');
        });

        it('should redirect to leader application if error getting leader participant', () => {
            var promised = deferred.defer();
            promised.reject({'status': 500, 'statusText': 'abandon hope all ye who enter here'});
            spyOn(participantService, 'get').and.returnValue(promised.promise);
            spyOn(location, 'path').and.callFake(function() { });

            fixture.$onInit();
            scope.$digest();

            expect(fixture.ready).toBeFalsy();
            expect(fixture.approvedLeader).toBeFalsy();
            expect(location.path).toHaveBeenCalledWith('/grouptool/leader');
        });
    });
});