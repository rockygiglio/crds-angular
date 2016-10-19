import CONSTANTS from 'crds-constants';

export default class ClosableTagController {
  /*@ngInject*/
  constructor() {
    this.maxSVAge = CONSTANTS.SERVING.MAXSTUDENTVOLUNTEERAGE;
    this.svText = CONSTANTS.SERVING.STUDENTVOLUNTEERTEXT;
    console.log(this.age);
  }

  click() {
    this.onClick({});
  }

  close() {
    this.onClose({});
  }
}