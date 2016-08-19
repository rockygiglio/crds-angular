
import constants from 'crds-constants';
import ChangeParticipantRoleController from '../../../../../app/group_tool/group_detail/participants/change_participant_role/changeParticipantRole.controller';
import Participant from '../../../../../app/group_tool/model/participant';

describe('ChangeParticipantRoleController', () => {
    let fixture,
        groupService,
        anchorScroll,
        groupDetailService,
        rootScope;

    var mockProfile;

    beforeEach(angular.mock.module(constants.MODULES.GROUP_TOOL));

    beforeEach(angular.mock.module(($provide) => {
      mockProfile = jasmine.createSpyObj('Profile', ['Personal']);
      $provide.value('Profile', mockProfile);
    }));

    beforeEach(inject(function ($injector) {
      groupService = $injector.get('GroupService');
      anchorScroll = $injector.get('$anchorScroll');
      groupDetailService = $injector.get('GroupDetailService');
      rootScope = $injector.get('$rootScope');

      fixture = new ChangeParticipantRoleController(groupService, anchorScroll, rootScope, groupDetailService);
    }));

    describe('the constructor', () => {
        it('should initialize properties', () => {
          expect(fixture.processing).toBeFalsy();
        });
    });

    describe('warningLeaderMax', () => {
        it('should return false when less than 5', () => {
          // spyOn(groupDetailService, 'participants').and.callFake(function() {
          //   return(4);
          let participants = [
            new Participant({nickName: 'f1', lastName: 'l1', groupRoleId: constants.GROUP.ROLES.LEADER, participantId: 11}),
            new Participant({nickName: 'f2', lastName: 'l2', groupRoleId: constants.GROUP.ROLES.LEADER, participantId: 22}),
            new Participant({nickName: 'f3', lastName: 'l3', groupRoleId: constants.GROUP.ROLES.LEADER, participantId: 33}),
            new Participant({nickName: 'f4', lastName: 'l4', groupRoleId: constants.GROUP.ROLES.LEADER, participantId: 44}),
            new Participant({nickName: 'f5', lastName: 'l5', groupRoleId: constants.GROUP.ROLES.MEMBER, participantId: 55}),
            new Participant({nickName: 'f6', lastName: 'l6', groupRoleId: constants.GROUP.ROLES.APPRENTICE, participantId: 66})
          ];
          fixture.participants = participants;
          expect(fixture.warningLeaderMax(), false);
          });
    });


    describe('warningLeaderMax', () => {
        it('should return true when greater than 5', () => {

        });
    });

    describe('warningApprenticeMax', () => {
        it('should return false when less than 5', () => {

        });
    });

    describe('warningApprenticeMax', () => {
        it('should return true when greater than 5', () => {

        });
    });

});
