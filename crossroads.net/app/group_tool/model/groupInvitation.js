import CONSTANTS from '../../constants';

export default class GroupInvitation {

  constructor(jsonObject){
    if(jsonObject) {
      Object.assign(this, jsonObject);
    } else {
      this.sourceId = undefined;
      this.groupRoleId = undefined;
      this.emailAddress = undefined;
      this.recipientName = undefined;
      this.requestDate = undefined;
    }

    // Always set to a Group invitation type    
    this.invitationType = CONSTANTS.INVITATION.TYPES.GROUP;
  }

  isLeader() {
   return this.groupRoleId === CONSTANTS.GROUP.ROLES.LEADER; 
  }

  role() {
    if(this.groupRoleId === CONSTANTS.GROUP.ROLES.LEADER) {
      return 'Leader';
    } else if (this.groupRoleId === CONSTANTS.GROUP.ROLES.APPRENTICE) {
      return 'Apprentice';
    } else if (this.groupRoleId === CONSTANTS.GROUP.ROLES.MEMBER) {
      return 'Participant';
    }

    return null;
  }
}