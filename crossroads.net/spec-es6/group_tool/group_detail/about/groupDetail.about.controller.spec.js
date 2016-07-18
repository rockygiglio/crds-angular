
import constants from 'crds-constants';
import GroupDetailAboutController from '../../../../app/group_tool/group_detail/about/groupDetail.about.controller';

describe('GroupDetailAboutController', () => {
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

        state.params = {
          groupId: 123
        };

        fixture = new GroupDetailAboutController(groupService, imageService, state, log);
    }));

    describe('the constructor', () => {
        it('should initialize properties', () => {
            expect(fixture.defaultProfileImageUrl).toEqual(imageService.DefaultProfileImage);
            expect(fixture.groupId).toEqual(state.params.groupId);
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

          expect(groupService.getGroup).toHaveBeenCalledWith(state.params.groupId);
          expect(fixture.data).toBeDefined();
          expect(fixture.data.primaryContact).toBeDefined();
          expect(fixture.data.primaryContact.contactId).toBeDefined();
          expect(fixture.data.primaryContact.contactId).toEqual(groupData.contactId);
          expect(fixture.data.primaryContact.imageUrl).toBeDefined();
          expect(fixture.data.primaryContact.imageUrl).toEqual(`${imageService.ProfileImageBaseURL}${groupData.contactId}`);
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