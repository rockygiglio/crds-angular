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

      expect(fixture.invited.length).toEqual(0);
      expect(fixture.inquired.length).toEqual(0);
    });
  });

  describe('setView(newView, refresh) function', () => {
    it('should set current view', () => {
      fixture.setView('Invite', false);
      expect(fixture.currentView).toEqual('Invite');
      fixture.setView('List', false);
      expect(fixture.currentView).toEqual('List');
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
      expect(fixture.inquired.length).toEqual(1);
      expect(fixture.inquired[0].emailAddress).toEqual('jim.kriz@ingagepartners.com');
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
});