import constants from 'crds-constants';
import GroupDetailRequestsController from '../../../../app/group_tool/group_detail/requests/groupDetail.requests.controller'

describe('GroupDetailRequestsController', () => {
    let fixture,
        groupService,
        imageService,
        state,
        rootScope,
        log,
        qApi;

    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

    beforeEach(inject(function($injector) {
        groupService = $injector.get('GroupService'); 
        imageService = $injector.get('ImageService');
        state = $injector.get('$state');
        rootScope = $injector.get('$rootScope');
        log = $injector.get('$log');
        qApi = $injector.get('$q');

        rootScope.MESSAGES = {
          generalError: 'general error',
          emailSent: 'email sent',
          emailSendingError: 'email sending error'
        };

        state.params = {
          groupId: 123
        };

        fixture = new GroupDetailRequestsController(groupService, imageService, state, rootScope, log);
    }));

    describe('the constructor', () => {
        it('should initialize properties', () => {
            expect(fixture.defaultProfileImageUrl).toEqual(imageService.DefaultProfileImage);
            expect(fixture.groupId).toEqual(state.params.groupId);
            expect(fixture.ready).toBeFalsy();
            expect(fixture.error).toBeFalsy();
            expect(fixture.currentView).toEqual('List');
            expect(fixture.currentRequest).toBe(null);
            expect(fixture.invite).toBe(null);
            expect(fixture.groupParticipantRoles).toEqual([
              { 'id': constants.GROUP.ROLES.MEMBER, 'label': 'Participant' },
              { 'id': constants.GROUP.ROLES.LEADER, 'label': 'Co-Leader' },
              { 'id': constants.GROUP.ROLES.APPRENTICE, 'label': 'Apprentice' }
            ]);

            expect(fixture.processing).toBeFalsy();
        });
    });

    describe('setView() function', () => {
      it('should set current view', () => {
        fixture.setView('Invite');
        expect(fixture.currentView).toEqual('Invite');
        fixture.setView('List');
        expect(fixture.currentView).toEqual('List');
      });
    });

    describe('beginInvitation() function', () => {
      it('should setup invitation appropriately', () => {
        fixture.beginInvitation();
        expect(fixture.processing).toBeFalsy();
        expect(fixture.invite).toBeDefined();
        expect(fixture.invite.invitationType).toEqual(constants.INVITATION.TYPES.GROUP);
        expect(fixture.invite.sourceId).toEqual(state.params.groupId);
        expect(fixture.currentView).toEqual('Invite');
      });
    });

    describe('sendInvitation() function', () => {
      let form = {
        $valid: true
      };
      let invitation = {
        'sourceId': 11,
        'groupRoleId': 22,
        'emailAddress': 'email',
        'recipientName': 'recipient',
        'requestDate': '2016-07-14T04:03:11.223Z',
        'invitationType': 33,
      };

      it('should emit a message if the form is invalid', () => {
        fixture.setView('Invite');
        spyOn(groupService, 'sendGroupInvitation').and.callFake(() => { });
        spyOn(rootScope, '$emit').and.callFake(() => { });
        form.$valid = false;

        fixture.sendInvitation(form, invitation);
        expect(groupService.sendGroupInvitation).not.toHaveBeenCalled();
        expect(fixture.processing).toBeFalsy();
        expect(fixture.currentView).toEqual('Invite');
        expect(rootScope.$emit).toHaveBeenCalledWith('notify', rootScope.MESSAGES.generalError);
      });

      it('should emit a message if there is an error sending the invite', () => {
        fixture.setView('Invite');
        var deferred = qApi.defer();
        deferred.reject({ status: 500, statusText: 'doh!'});

        spyOn(groupService, 'sendGroupInvitation').and.callFake(() => { return deferred.promise;});
        spyOn(rootScope, '$emit').and.callFake(() => { });
        form.$valid = true;

        fixture.sendInvitation(form, invitation);
        rootScope.$digest();
        expect(groupService.sendGroupInvitation).toHaveBeenCalledWith(invitation);
        expect(fixture.processing).toBeFalsy();
        expect(fixture.currentView).toEqual('Invite');
        expect(rootScope.$emit).toHaveBeenCalledWith('notify', rootScope.MESSAGES.emailSendingError);
      });

      it('should send the invite and return to the list', () => {
        fixture.setView('Invite');
        var deferred = qApi.defer();
        deferred.resolve({});

        spyOn(groupService, 'sendGroupInvitation').and.callFake(() => { return deferred.promise;});
        spyOn(rootScope, '$emit').and.callFake(() => { });
        spyOn(fixture, '$onInit').and.callFake(() => { });
        form.$valid = true;

        fixture.sendInvitation(form, invitation);
        rootScope.$digest();
        expect(groupService.sendGroupInvitation).toHaveBeenCalledWith(invitation);
        expect(fixture.processing).toBeFalsy();
        expect(fixture.currentView).toEqual('List');
        expect(fixture.invite).toBe(null);
        expect(rootScope.$emit).toHaveBeenCalledWith('notify', rootScope.MESSAGES.emailSent);
        expect(fixture.$onInit).toHaveBeenCalled();
      });
      
    });

    describe('$onInit() function', () => {
        it('should get requests and set image url', () => {
          let requests = [
            {
              'contactId': 1670863,
              'participantId': 456,
              'name': 'Chris Jackson',
              'requestType': 'requested',
              'emailAddress': 'cj101@gmail.com',
              'dateRequested': new Date(2016, 5, 20)
            },
            {
              'contactId': 123,
              'participantId': 456,
              'name': 'Sally Jackson',
              'requestType': 'requested',
              'emailAddress': 'sallyj@yahoo.com',
              'dateRequested': new Date(2016, 5, 15)
            },
            {
              'contactId': 123,
              'participantId': 456,
              'name': 'Donny Franks',
              'requestType': 'invited',
              'emailAddress': 'donnyf@gmail.com',
              'dateRequested': new Date(2016, 4, 15)
            },
          ];

          let deferred = qApi.defer();
          deferred.resolve({
            'groupId': state.params.groupId,
            'requests': requests
          });

          spyOn(groupService, 'getGroupRequests').and.callFake(function(groupId) {
            return(deferred.promise);
          });

          fixture.$onInit();
          rootScope.$digest();

          expect(groupService.getGroupRequests).toHaveBeenCalledWith(state.params.groupId);
          expect(fixture.data).toBeDefined();
          expect(fixture.data.groupId).toBeDefined();
          expect(fixture.data.groupId).toEqual(state.params.groupId);
          expect(fixture.data.requests).toBeDefined();
          expect(fixture.data.requests.length).toEqual(requests.length);
          fixture.data.requests.forEach(function(r) {
            expect(r.imageUrl).toBeDefined();
            expect(r.imageUrl).toEqual(`${imageService.ProfileImageBaseURL}${r.contactId}`);
          }, this);
          expect(fixture.ready).toBeTruthy();
          expect(fixture.error).toBeFalsy();
        });

        it('should set error state if trouble getting requests', () => {
          let deferred = qApi.defer();
          let error = {
            status: 500,
            statusText: 'oops'
          };
          deferred.reject(error);

          spyOn(groupService, 'getGroupRequests').and.callFake(function(groupId) {
            return(deferred.promise);
          });

          fixture.$onInit();
          rootScope.$digest();

          expect(groupService.getGroupRequests).toHaveBeenCalledWith(state.params.groupId);
          expect(fixture.ready).toBeTruthy();
          expect(fixture.error).toBeTruthy();
        });
    });
});