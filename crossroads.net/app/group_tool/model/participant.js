
import constants from 'crds-constants';

export default class Participant {

  constructor(jsonObject) {
    Object.assign(this, jsonObject);
  }

  isLeader() {
    return this.groupRoleId === constants.GROUP.ROLES.LEADER; 
  }

  isApprentice() {
    return this.groupRoleId === constants.GROUP.ROLES.APPRENTICE; 
  }

  displayName() {
    return `${this.nickName} ${this.lastName}`;
  }
}