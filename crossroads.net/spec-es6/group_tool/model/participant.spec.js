
import Participant from '../../../app/group_tool/model/participant';
import constants from 'crds-constants';

describe('Participant', () => {

  let participant,
    mockJson;

  beforeEach(() => {
    mockJson = {
      'nickName': 'first',
      'lastName': 'last',
      'groupRoleId': constants.GROUP.ROLES.LEADER
    };
  });

  describe('creation', () => {
    it('should have the following values when created with json', () => {
      participant = new Participant(mockJson);

      expect(participant.nickName).toEqual(mockJson.nickName);
      expect(participant.lastName).toEqual(mockJson.lastName);
      expect(participant.groupRoleId).toEqual(mockJson.groupRoleId);
      expect(participant.displayName()).toEqual(`${mockJson.nickName} ${mockJson.lastName}`);
    });

    it('should have the following values when created without json', () => {
      participant = new Participant();

      expect(participant.nickName).toEqual('');
      expect(participant.lastName).toEqual('');
      expect(participant.groupRoleId).toEqual(-1);
    });
  });

  describe('isLeader', () => {
    it('should be true', () => {
      participant = new Participant();
      participant.groupRoleId = constants.GROUP.ROLES.LEADER;
      expect(participant.isLeader()).toBeTruthy();
    });

    it('should be false', () => {
      participant = new Participant();
      participant.groupRoleId = constants.GROUP.ROLES.APPRENTICE;
      expect(participant.isLeader()).toBeFalsy();
    });
  });

  describe('isApprentice', () => {
    it('should be true', () => {
      participant = new Participant();
      participant.groupRoleId = constants.GROUP.ROLES.APPRENTICE;
      expect(participant.isApprentice()).toBeTruthy();
    });

    it('should be false', () => {
      participant = new Participant();
      participant.groupRoleId = constants.GROUP.ROLES.LEADER;
      expect(participant.isApprentice()).toBeFalsy();
    });
  });

  describe('displayName', () => {
    it('should be set properly', () => {
      participant = new Participant();
      participant.nickName = 'first';
      participant.lastName = 'last';
      participant.groupRoleId = constants.GROUP.ROLES.APPRENTICE;
      expect(participant.displayName()).toEqual('first last');
    });

    it('should be blank', () => {
      participant = new Participant();
      participant.groupRoleId = constants.GROUP.ROLES.LEADER;
      expect(participant.displayName()).toEqual('');
    });
  });
});
