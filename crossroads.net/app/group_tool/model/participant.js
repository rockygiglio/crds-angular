
import constants from 'crds-constants';

export default class Participant {

  constructor(jsonObject) {
    if(jsonObject) {
      Object.assign(this, jsonObject);
    } else {
      this.groupRoleId = -1;
      this.nickName = '';
      this.lastName = '';
    }
  }

  isLeader() {
    return this.groupRoleId === constants.GROUP.ROLES.LEADER; 
  }

  isApprentice() {
    return this.groupRoleId === constants.GROUP.ROLES.APPRENTICE; 
  }

  displayName() {
    return `${this.nickName} ${this.lastName}`.replace(/^\s+|\s+$/g,'');
  }
}