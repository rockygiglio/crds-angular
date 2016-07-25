import constants from 'crds-constants';
import CreateGroupPreviewController from '../../../../app/group_tool/create_group/preview/createGroup.preview.controller'
describe('CreateGroupPreviewController', () => {
    let fixture,
        groupService, 
        createGroupService, 
        group, 
        imageService, 
        state, 
        log, 
        rootScope,
        mockProfile,
        api;

    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

    beforeEach(angular.mock.module(($provide)=> {
      mockProfile = jasmine.createSpyObj('Profile', ['Personal']);
      $provide.value('Profile', mockProfile);
    }));

    beforeEach(inject(($injector)=> {
      groupService = $injector.get('GroupService');
      createGroupService = $injector.get('CreateGroupService');
      group = $injector.get('Group');
      imageService = $injector.get('ImageService');
      state = $injector.get('$state');
      location = $injector.get('$location');
      log = $injector.get('$log');
      rootScope = $injector.get('$rootScope');
      api = $injector.get('$q');

      fixture = new CreateGroupPreviewController(groupService, createGroupService, group, imageService, state, log, rootScope);

    }));

    describe('save', () => {
        it('should call save', () => {
            let promised = api.defer();

            spyOn(createGroupService, 'saveCreateGroupForm').and.callFake( function() {
                return promised.promise; 
            })
            fixture.groupData = "Hi Dustin";
            fixture.save();
            expect(createGroupService.saveCreateGroupForm).toHaveBeenCalledWith(fixture.groupData);
        })
    });
    // it('should call participant service', () => {
    //   fixture.$onInit();
    //   expect(participantService.get).toHaveBennCalled();
    // });


});

