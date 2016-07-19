
export default class MessageParticipantsController {
  /*@ngInject*/
  constructor() {  }

  submit(message) {
    this.submitAction({message: message});
  }

  cancel(message) {
    this.cancelAction({message: message});
  }
}