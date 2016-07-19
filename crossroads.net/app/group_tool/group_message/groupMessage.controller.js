
export default class GroupMessageController {
  /*@ngInject*/
  constructor() {  
    this.person;
  }

  submit() {
    this.submitAction({person: this.person, message: this.message});
  }

  cancel() {
    this.cancelAction({person: this.person});
  }

  hasSubHeaderText() {
    return !(this.subHeaderText === null || this.subHeaderText === undefined)
  }

  hasContactText() {
    return !(this.contactText === null || this.contactText === undefined)
  }

  hasEmailTemplateText() {
    return !(this.emailTemplateText === null || this.emailTemplateText === undefined)
  }
}