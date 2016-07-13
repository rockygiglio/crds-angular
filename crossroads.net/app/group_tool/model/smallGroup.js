
import CONSTANTS from '../../constants';
import Address from './address';
import Participant from './participant';
import Category from './category';

export default class SymallGroup {

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
      this.participants = 
        jsonObject.Participants.map((particpant) => {
          return new Participant(particpant);
        });
    }

    this.categories = [];
    if(jsonObject.attributeTypes != undefined && jsonObject.attributeTypes != null &&
        jsonObject.attributeTypes[CONSTANTS.GROUP.ATTRIBUTE_TYPE_ID] != undefined &&
        jsonObject.attributeTypes[CONSTANTS.GROUP.ATTRIBUTE_TYPE_ID] != null &&
        jsonObject.attributeTypes[CONSTANTS.GROUP.ATTRIBUTE_TYPE_ID].attributes != undefined &&
        jsonObject.attributeTypes[CONSTANTS.GROUP.ATTRIBUTE_TYPE_ID].attributes != null)
    {
      jsonObject.attributeTypes[CONSTANTS.GROUP.ATTRIBUTE_TYPE_ID].attributes.forEach(function(attribute) {
        if(attribute.selected) {
          this.categories.push(new Category(attribute));
        }
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

  categoriesToString() {
    let categoriesString = this.categories.length > 0 ? `${this.categories[0]}` : '';

    for(let idx=1; idx < this.categories.length; idx++) {
      categoriesString += `, ${this.categories[idx].toString()}`;
    }

    return categoriesString;
  }
}