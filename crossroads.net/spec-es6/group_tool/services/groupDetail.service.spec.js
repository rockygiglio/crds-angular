import CONSTANTS from 'crds-constants';
import GroupDetailService from '../../../app/group_tool/services/groupDetail.service'


describe('Group Tool GroupDetail Service', () => {
  let fixture,
    log;

  const endpoint = window.__env__['CRDS_GATEWAY_CLIENT_ENDPOINT'] + 'api';

  beforeEach(angular.mock.module(CONSTANTS.MODULES.GROUP_TOOL));

  beforeEach(inject(function($injector) {
    log = $injector.get('$log');

    fixture = new GroupDetailService(log);
  }));

  describe('canAccessParticipants()', () => {
    it('should return true, user is leader', () => {
        let isLeader = true;
        let groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ID.ONSITE_GROUPS;

        let result = fixture.canAccessParticipants(isLeader, groupTypeId);

        expect(result).toBeTruthy();
    });
    
    it('should return true, user is leader and it is not an onsite group', () => {
        let isLeader = true;
        let groupTypeId = 7726373;

        let result = fixture.canAccessParticipants(isLeader, groupTypeId);

        expect(result).toBeTruthy();
    });
    
    it('should return true, user is not leader but it is not Onsite Group', () => {
        let isLeader = false;
        let groupTypeId = 7732323;

        let result = fixture.canAccessParticipants(isLeader, groupTypeId);

        expect(result).toBeTruthy();
    });
    
    it('should return false, user is not leader and it is an onsite group', () => {
        let isLeader = false;
        let groupTypeId = CONSTANTS.GROUP.GROUP_TYPE_ID.ONSITE_GROUPS;

        let result = fixture.canAccessParticipants(isLeader, groupTypeId);

        expect(result).toBeFalsy();
    });
  });
});