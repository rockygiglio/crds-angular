
import CONSTANTS from '../../constants';
import Address from './address';
import Participant from './participant';

export default class SmallGroup {

  constructor(jsonObject){
    this.createSubObjects(jsonObject);
    this.deleteSubObjects(jsonObject);
    Object.assign(this, jsonObject);
  }

  createSubObjects(jsonObject) {
    this.address = (jsonObject.address === undefined || jsonObject.address === null) ? null : new Address(jsonObject.address);
    
    this.participants = [];
    if(jsonObject.Participants != undefined && jsonObject.Participants != null)
    {
      jsonObject.Participants.forEach(function(particpant) {
        this.participants.push(new Participant(particpant));
      }, this);
    }
  }

  deleteSubObjects(jsonObject) {
    delete jsonObject.address;
    delete jsonObject.Participants;
  }

  isLeader() {
   return this.groupRoleId === CONSTANTS.GROUP.ROLES.LEADER; 
  }

  role() {
    if(this.groupRoleId === CONSTANTS.GROUP.ROLES.LEADER) {
      return 'Leader';
    } else if (this.groupRoleId === CONSTANTS.GROUP.ROLES.APPRENTICE) {
      return 'Apprentice';
    }

    return 'Participant';
  }

  meetingLocation() {
    if(this.address === null || this.address === undefined) {
      return 'Online';
    }

    return this.address.toString();
  }

  categories() {
    this.category = 'Marriage Growth, Listening, Financial';
    return this.category;
  }
}