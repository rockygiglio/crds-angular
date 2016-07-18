
export default class Address {

  constructor(jsonObject) {
    Object.assign(this, jsonObject);
  }

  toString() {
    if(this.zip === null) {
      return 'Online';
    }
    else if(this.addressLine2 === null) {
      return `${this.addressLine1}, ${this.city} ${this.state}, ${this.zip}`;
    }
    
    return `${this.addressLine1}, ${this.addressLine2}, ${this.city} ${this.state}, ${this.zip}`;
  }

  getZip() {
    if (null === this.zip){
      return 'Online';
    }
    else {
      return `${this.zip}`;
    }
  }
}