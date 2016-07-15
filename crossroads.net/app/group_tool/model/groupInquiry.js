export default class GroupInquiry {

  constructor(jsonObject){
    if(jsonObject) {
      Object.assign(this, jsonObject);
    } else {
      this.groupId = undefined;
      this.inquiryId = undefined;
      this.emailAddress = undefined;
      this.phoneNumber = undefined;
      this.firstName = undefined;
      this.lastName = undefined;
      this.requestDate = undefined;
      this.placed = undefined;
    }
  }

  requestersName() {
    return `${this.firstName} ${this.lastName}`;
  }
}