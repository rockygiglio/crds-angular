
import constants from 'crds-constants';
import ChangeParticipantRoleController from '../../../../../app/group_tool/group_detail/participants/change_participant_role/changeParticipantRole.controller';

describe('ChangeParticipantRoleController', () => {
    let fixture,
        groupService,
        anchorScroll,
        rootScope,
        groupDetailService;

    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));
    var profile;
    beforeEach(angular.mock.module(($provide) => {
        mockProfile = jasmine.createSpyObj('Profile', ['Personal']);
        $provide.value('Profile', mockProfile);
    }));

    describe('the constructor', () => {
        it('should initialize properties', () => {
            //expect(fixture).toBeDefined();
            //expect(fixture.groupService).toBeDefined();
            //expect(fixture.groupDetailService).toBeDefined();
            //expect(fixture.processing).toBeFalsy();
        });
    });

    // describe('warningLeaderMax() function', () => {
    //     it('should return true if less than max', () => {

    //         fixture.$onInit();

    //         expect(fixture.defaultProfileImageUrl).toBeDefined();
    //         expect(fixture.defaultProfileImageUrl).toEqual(imageService.DefaultProfileImage);
    //     }),
    //         it('should return false if greater than max', () => {

    //             fixture.$onInit();

    //             expect(fixture.defaultProfileImageUrl).toBeDefined();
    //             expect(fixture.defaultProfileImageUrl).toEqual(imageService.DefaultProfileImage);
    //         });
    // });


});