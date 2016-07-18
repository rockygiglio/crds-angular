
export default class GroupInquiry {

  constructor(jsonObject){
    if(jsonObject) {
      Object.assign(this, jsonObject);
    } else {
      this.groupId = undefined;
      this.contactId = undefined;
      this.inquiryId = undefined;
      this.emailAddress = undefined;
      this.phoneNumber = undefined;
      this.firstName = '';
      this.lastName = '';
      this.nickName = this.firstName;
      this.requestDate = undefined;
      this.placed = undefined;
      this.imageUrl = undefined;
      this.defaultProfileImageUrl = undefined;
    }
  }

  recipientName() {
    return `${this.firstName} ${this.lastName}`;
  }
}