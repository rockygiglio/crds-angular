
import Address from './Address';
import Participant from './Participant';

export default class SmallGroup {

  constructor(jsonObject){
    this.createSubObjects(jsonObject);
    this.deleteSubObjects(jsonObject);
    Object.assign(this, jsonObject);

    this.categories();
    this.isLeader();
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
      let random = Math.floor(Math.random()*100)%4;

      if(random === 0) {
        this.leader = true;
      } else if(random === 1) {
        this.leader =  false;
      } else if(random === 2) {
        this.leader = true;
      } else {
        this.leader = false;
      }
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
    if (this.category === null || this.category === undefined) {
      let random = Math.floor(Math.random()*100)%4;

      if(random === 0) {
        this.category = 'Financial';
      } else if(random === 1) {
        this.category = 'Bible Study';
      } else if(random === 2) {
        this.category = 'Marriage Growth, Listening, Financial';
      } else {
        this.category = 'Bible Study, Financial, Stress Managment';
      }
    }

    return this.category;
  }
}