
export default class RemoveParticipantController {
  /*@ngInject*/
  constructor() {  }

  submit(participant) {
    this.submitAction(participant);
  }

  cancel(participant) {
    this.cancelAction(participant);
  }
}