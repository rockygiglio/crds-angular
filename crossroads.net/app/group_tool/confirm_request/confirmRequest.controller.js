export default class ConfirmRequestController {
  /*@ngInject*/
  constructor() {
    this.processing = false;
  }

  cancel() {
    this.modalInstance.dismiss();
  }

  submit() {
    this.processing = true;

    // TODO - Remove timeout faking submission for loading-button
    window.setTimeout(() => {
      this.modalInstance.dismiss();
    }, 2000);
  }
}
