
import constants from 'crds-constants';

export default class Profile {

  constructor(jsonObject) {
    if(jsonObject) {
      Object.assign(this, jsonObject);
    } else {
      this.address1 = '';
      this.city = '';
      this.state = '';
      this.postalCode = '';
      this.country = '';
      this.congregationId = '';
      this.contactId = '';
      this.dateOfBirth = '';
      this.emailAddress = '';
      this.oldEmailAddress = '';
      this.genderId = '';
    }
  }

}
