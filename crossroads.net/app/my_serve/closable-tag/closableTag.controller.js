import CONSTANTS from 'crds-constants';

export default class ClosableTagController {
  /*@ngInject*/
  constructor() {
    this.maxSVAge = CONSTANTS.SERVING.MAXSTUDENTVOLUNTEERAGE;
    this.svText = CONSTANTS.SERVING.STUDENTVOLUNTEERTEXT;
  }

  click() {
    this.onClick({});
  }

  close() {
    this.opportunityId = 0;
    this.onClose({});
  }
}
