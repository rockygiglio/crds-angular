
import constants from 'crds-constants';
import GroupDetailAboutController from '../../../../app/group_tool/group_detail/about/groupDetail.about.controller';

describe('GroupDetailAboutController', () => {
    let fixture,
        groupService,
        imageService,
        state,
        rootScope,
        log,
        qApi,
        mockProfile,
        cookies;


    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

        beforeEach(angular.mock.module(($provide)=> {
      mockProfile = jasmine.createSpyObj('Profile', ['Personal']);
      $provide.value('Profile', mockProfile);
    }));

    beforeEach(inject(function($injector) {
        groupService = $injector.get('GroupService');
        imageService = $injector.get('ImageService');
        state = $injector.get('$state');
        rootScope = $injector.get('$rootScope');
        log = $injector.get('$log');
        qApi = $injector.get('$q');
        cookies = $injector.get('$cookies');

        state.params = {
          groupId: 123
        };

        fixture = new GroupDetailAboutController(groupService, imageService, state, log, cookies);
    }));

    describe('groupExists() function', () => {
      it('should be true', () => {
        expect(fixture.groupExists()).toBeTruthy();
      });

      it('should be true', () => {
        state.params.groupId = null;
        expect(fixture.groupExists()).toBeFalsy();
      });
      
      it('should be true', () => {
        state.params.groupId = null;
        fixture.data = {groupId: 123};
        expect(fixture.groupExists()).toBeTruthy();
      });
    });

    describe('the constructor', () => {
        it('should initialize properties', () => {
            expect(fixture.defaultProfileImageUrl).toEqual(imageService.DefaultProfileImage);
            expect(fixture.ready).toBeFalsy();
            expect(fixture.error).toBeFalsy();
        });
    });

    describe('$onInit() function', () => {
        it('should get group and set image url', () => {
          let groupData = {
            contactId: 987
          };

          let deferred = qApi.defer();
          deferred.resolve(groupData);

          spyOn(groupService, 'getGroup').and.callFake(function(groupId) {
            return(deferred.promise);
          });

          fixture.$onInit();
          rootScope.$digest();


          expect(fixture.groupId).toEqual(state.params.groupId);
          expect(groupService.getGroup).toHaveBeenCalledWith(state.params.groupId);
          expect(fixture.data).toBeDefined();
          expect(fixture.data.primaryContact).toBeDefined();
          expect(fixture.data.primaryContact.contactId).toBeDefined();
          expect(fixture.data.primaryContact.contactId).toEqual(groupData.contactId);
          expect(fixture.data.primaryContact.imageUrl).toBeDefined();
          expect(fixture.data.primaryContact.imageUrl).toEqual(`${imageService.ProfileImageBaseURL}${groupData.contactId}`);
          expect(fixture.ready).toBeTruthy();
          expect(fixture.error).toBeFalsy();
          expect(fixture.forInvitation).toBeFalsy();
        });

        it('should set error state if trouble getting requests', () => {
          let deferred = qApi.defer();
          let error = {
            status: 500,
            statusText: 'oops'
          };
          deferred.reject(error);

          spyOn(groupService, 'getGroup').and.callFake(function(groupId) {
            return(deferred.promise);
          });

          fixture.$onInit();
          rootScope.$digest();

          expect(groupService.getGroup).toHaveBeenCalledWith(state.params.groupId);
          expect(fixture.ready).toBeTruthy();
          expect(fixture.error).toBeTruthy();
        });
    });
});