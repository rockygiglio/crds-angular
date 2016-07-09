
import Address from './Address';
import Participant from './Participant';

export default class SmallGroup {

  constructor(jsonObject){
    this.createSubObjects(jsonObject);
    this.deleteSubObjects(jsonObject);
    Object.assign(this, jsonObject);
  }

  createSubObjects(jsonObject) {
    this.address = (jsonObject.address === undefined || jsonObject.address === null) ? null : new Address(jsonObject.address);
    
    this.participants = []
    jsonObject.Participants.forEach(function(particpant) {
      this.participants.push(new Participant(particpant));
    }, this);
  }

  deleteSubObjects(jsonObject) {
    delete jsonObject.address;
    delete jsonObject.Participants;
  }

  isLeader() {
    //TODO:: Remove this after implemented on backend
    if(this.leader === null || this.leader === undefined) {
      this.groupRoleId = Math.floor((Math.random()*100)/4);
      this.leader = (this.groupRoleId === 22)
    }

    return this.leader;
  }

  meetingLocation() {
    if(this.address === null || this.address === undefined) {
      return 'Online';
    }

    return this.address.toString();
  }

  categories() {
    //TODO:: This will change when categories are passed in
    let random = Math.floor(Math.random()*100)%4;

    if(random === 0) {
      return 'Financial';
    } else if(random === 1) {
      return 'Bible Study';
    } else if(random === 2) {
      return 'Marriage Growth, Listening, Financial';
    } else {
      return 'Bible Study, Financial, Stress Managment';
    }
  }
}