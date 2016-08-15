
export default class GroupEmailController {
  /*@ngInject*/
  constructor($rootScope) {
    this.rootScope = $rootScope
    this.allowSubject = (this.allowSubject === undefined) ? false : this.allowSubject;
    this.header = (this.header === undefined) ? 'Email' : this.header;
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
    if(this.canCancel()) {
      this.cancelAction({message: message});
    }
  }

  canCancel() {
    return this.cancelAction !== undefined;
  }
}