
import constants from 'crds-constants';
import ConfirmRequestController from '../../../app/group_tool/confirm_request/confirmRequest.controller';
import GroupMessage from '../../../app/group_tool/model/groupMessage';


describe('ConfirmRequestController', () => {
    let fixture,
        mockProfile,
        rootScope,
        messageService,
        groupService,
        qApi;

    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

    beforeEach(angular.mock.module(($provide)=> {
        mockProfile = jasmine.createSpyObj('Profile', ['Personal']);
        $provide.value('Profile', mockProfile);
    }));

    beforeEach(inject(function($injector) {
        messageService = $injector.get('MessageService');
        groupService = $injector.get('GroupService');
        rootScope = $injector.get('$rootScope');
        qApi = $injector.get('$q');

        fixture = new ConfirmRequestController(rootScope, messageService, groupService);
        fixture.group = {groupId: 1}
    }));

    describe('$onInit() function', () => {
        it('when emailLeader should create a groupMessage', () =>{
            fixture.emailLeader = true;
            fixture.$onInit();

            expect(fixture.groupMessage).toBeDefined();
            expect(fixture.groupMessage.groupId).toEqual(1);
        });

        it('should create a groupMessage', () =>{
            fixture.emailLeader = false;
            fixture.$onInit();

            expect(fixture.groupMessage).not.toBeDefined();
        });
    });

    describe('sendEmail() function', () => {
        beforeEach(() => {
            fixture.processing = false;
            fixture.emailLeader = true;
        });

        it('should invoke sendLeaderMessage', () => {
            fixture.groupMessage = {groupId: 123, subject: 'aaa', body: 'bbb'};
            let deferred = qApi.defer();
            let success = {
                status: 200,
            };
            deferred.resolve(success);

            spyOn(messageService, 'sendLeaderMessage').and.callFake(function(groupMessage) {
                return(deferred.promise);
            });

            fixture.sendEmail();
            expect(messageService.sendLeaderMessage).toHaveBeenCalledWith(fixture.groupMessage);
        });
    });

    describe('submitRequest() function', () => {
        beforeEach(() => {
            fixture.processing = false;
            fixture.emailLeader = false;
        });

        it('should invoke submitJoinRequest', () => {
            fixture.group.groupId = 123;
            let deferred = qApi.defer();
            let success = {
                status: 200,
            };
            deferred.resolve(success);

            spyOn(groupService, 'submitJoinRequest').and.callFake(function(submitJoinRequest) {
                return(deferred.promise);
            });

            fixture.sendJoinRequest();
            expect(groupService.submitJoinRequest).toHaveBeenCalledWith(fixture.group.groupId);
        });
    });
});