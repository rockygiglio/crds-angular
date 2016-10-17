export default class ClosableTagController {
  /*@ngInject*/
  constructor() {
  }

  click() {
    this.onClick({});
  }

  close() {
    this.onClose({});
  }
}