import constants from 'crds-constants';
import GroupDetailRequestsController from '../../../../app/group_tool/group_detail/requests/groupDetail.requests.controller'
import GroupInquiry from '../../../../app/group_tool/model/groupInquiry';
import GroupInvitation from '../../../../app/group_tool/model/groupInvitation';

describe('GroupDetailRequestsController', () => {
    let fixture,
        groupService,
        state,
        rootScope,
        log,
        qApi;

    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

    beforeEach(inject(function($injector) {
        groupService = $injector.get('GroupService'); 
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

        fixture = new GroupDetailRequestsController(groupService, state, rootScope, log);
    }));

    describe('the constructor', () => {
        it('should initialize properties', () => {
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

            expect(fixture.invited.length).toEqual(0);
            expect(fixture.inquired.length).toEqual(0);
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
        it('should get invited and inquiries', () => {
          let mockInvities = [
            {
              "sourceId": 123,
              "groupRoleId": 16,
              "emailAddress": "for@me.com",
              "recipientName": "Knowledge Man",
              "requestDate": "2016-07-14T11:00:00",
              "invitationType": 1,
              "invitationId": 0,
              "invitationGuid": null
            },
            {
              "sourceId": 123,
              "groupRoleId": 16,
              "emailAddress": "really@fast.com",
              "recipientName": "Buffer Dude",
              "requestDate": "2016-07-14T11:00:00",
              "invitationType": 1,
              "invitationId": 0,
              "invitationGuid": null
            }
          ];

          let mockInquires = [
            {
              "groupId": 123,
              "emailAddress": "jim.kriz@ingagepartners.com",
              "phoneNumber": "513-432-1973",
              "firstName": "Dustin",
              "lastName": "Kocher",
              "requestDate": "2016-07-14T10:00:00",
              "placed": false,
              "inquiryId": 19
            },
            {
              "groupId": 123,
              "emailAddress": "jkerstanoff@callibrity.com",
              "phoneNumber": "513-987-1983",
              "firstName": "Joe",
              "lastName": "Kerstanoff",
              "requestDate": "2016-07-14T10:00:00",
              "placed": false,
              "inquiryId": 20
            },
            {
              "groupId": 123,
              "emailAddress": "kim.farrow@thrivecincinnati.com",
              "phoneNumber": "513-874-6947",
              "firstName": "Kim",
              "lastName": "Farrow",
              "requestDate": "2016-07-14T10:00:00",
              "placed": true,
              "inquiryId": 21
            }
          ];

          //Invities setup
          let invities = mockInvities.map((invitation) => {
            return new GroupInvitation(invitation);
          });

          let deferredInvities = qApi.defer();
          deferredInvities.resolve(mockInvities);
          deferredInvities.promise.then(() => {
            return invities;
          });

          //Inquiries setup
          let inquires = mockInquires.map((inquiry) => {
            return new GroupInquiry(inquiry);
          });

          let deferredInquiries = qApi.defer();
          deferredInquiries.resolve(mockInquires);
          deferredInquiries.promise.then(() => {
            return inquires;
          });

          spyOn(groupService, 'getInvities').and.callFake(function(groupId) {
            return(deferredInvities.promise);
          });

          spyOn(groupService, 'getInquiries').and.callFake(function(groupId) {
            return(deferredInquiries.promise);
          });

          fixture.$onInit();
          rootScope.$digest();
          expect(groupService.getInvities).toHaveBeenCalledWith(state.params.groupId);
          expect(groupService.getInquiries).toHaveBeenCalledWith(state.params.groupId);
          expect(fixture.invited).toBeDefined();
          expect(fixture.inquired).toBeDefined()
          expect(fixture.groupId).toBeDefined();
          expect(fixture.groupId).toEqual(state.params.groupId);
          expect(fixture.invited.length).toEqual(invities.length);
          expect(fixture.invited[0].emailAddress).toEqual(invities[0].emailAddress);
          expect(fixture.inquired.length).toEqual(inquires.length);
          expect(fixture.inquired[0].emailAddress).toEqual(inquires[0].emailAddress);
          expect(fixture.ready).toBeTruthy();
          expect(fixture.error).toBeFalsy();
        });

        it('should set error state if trouble getting inquiries', () => {
          let deferred = qApi.defer();
          let error = {
            status: 500,
            statusText: 'oops'
          };
          deferred.reject(error);

          spyOn(groupService, 'getInquiries').and.callFake(function(groupId) {
            return(deferred.promise);
          });

          fixture.$onInit();
          rootScope.$digest();

          expect(groupService.getInquiries).toHaveBeenCalledWith(state.params.groupId);
          expect(fixture.ready).toBeTruthy();
          expect(fixture.error).toBeTruthy();
        });
    });


    describe('getInquiring() function', () => {
        it('should get only placed inquired', () => {
          let mockInquires = [
            {
              "groupId": 123,
              "emailAddress": "jim.kriz@ingagepartners.com",
              "phoneNumber": "513-432-1973",
              "firstName": "Dustin",
              "lastName": "Kocher",
              "requestDate": "2016-07-14T10:00:00",
              "placed": null,
              "inquiryId": 19
            },
            {
              "groupId": 123,
              "emailAddress": "jkerstanoff@callibrity.com",
              "phoneNumber": "513-987-1983",
              "firstName": "Joe",
              "lastName": "Kerstanoff",
              "requestDate": "2016-07-14T10:00:00",
              "placed": false,
              "inquiryId": 20
            },
            {
              "groupId": 123,
              "emailAddress": "kim.farrow@thrivecincinnati.com",
              "phoneNumber": "513-874-6947",
              "firstName": "Kim",
              "lastName": "Farrow",
              "requestDate": "2016-07-14T10:00:00",
              "placed": true,
              "inquiryId": 21
            }
          ];

          let inquiries = mockInquires.map((inquiry) => {
            return new GroupInquiry(inquiry);
          });

          fixture.inquired = inquiries;
          expect(fixture.getInquiring().length).toEqual(1);
          expect(fixture.getInquiring()[0].emailAddress).toEqual('jim.kriz@ingagepartners.com');
        });
    });
});