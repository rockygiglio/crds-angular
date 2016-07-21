
export default class Profile {

  constructor(jsonObject) {
    if(jsonObject) {
      Object.assign(this, jsonObject);
    } else {
      this.addressLine1 = '';
      this.city = '';
      this.congregationId = '';
      this.contactId = '';
      this.country = '';
      this.dateOfBirth = '';
      this.emailAddress = '';
      this.genderId = '';
      this.oldEmail = '';
      this.postalCode = '';
      this.state = '';
    }
  }
}