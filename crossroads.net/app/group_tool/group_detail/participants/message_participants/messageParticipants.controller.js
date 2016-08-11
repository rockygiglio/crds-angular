
export default class MessageParticipantsController {
  /*@ngInject*/
  constructor($rootScope) {
    this.rootScope = $rootScope
  }

  submit(form, message) {

    if(!form.$valid) {
      this.processing = false;
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      return;
    }

    this.submitAction({message: message});
  }

  cancel(message) {
    this.cancelAction({message: message});
  }
}