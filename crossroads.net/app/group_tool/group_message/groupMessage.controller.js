
export default class GroupMessageController {
  /*@ngInject*/
  constructor() { }

  submit() {
    this.submitAction({person: this.person});
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