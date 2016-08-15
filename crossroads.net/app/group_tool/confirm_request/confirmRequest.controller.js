export default class ConfirmRequestController {
  /*@ngInject*/
  constructor() {
    this.processing = false;
  }

  cancel() {
    if(!this.processing) {
      this.modalInstance.dismiss();
    }
  }

  requestToJoin() {
    if(!this.processing) {
      this.emailLeader = false;
    }
  }

  emailGroupLeader() {
    if(!this.processing) {
      this.emailLeader = true;
    }
  }

  sendEmail() {
    this.processing = true;
  }

  submit() {
    this.processing = true;

    // TODO - Remove timeout faking submission for loading-button
    window.setTimeout(() => {
      this.modalInstance.dismiss();
    }, 2000);
  }
}
