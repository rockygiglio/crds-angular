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
      this.imageUrl = undefined;
      this.defaultProfileImageUrl = undefined;
    }

    // Always set to a Group invitation type    
    this.invitationType = CONSTANTS.INVITATION.TYPES.GROUP;
  }
}