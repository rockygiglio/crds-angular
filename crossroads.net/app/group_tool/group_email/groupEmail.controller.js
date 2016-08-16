
export default class GroupEmailController {
  /*@ngInject*/
  constructor($rootScope) {
    this.rootScope = $rootScope
    
    this.allowSubject = (this.allowSubject === undefined) ? false : this.allowSubject;
    this.process = (this.process === undefined) ? true : this.process;
    this.header = (this.header === undefined) ? '' : this.header;
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
    if(this.process) {
      this.cancelAction({message: message});
    }
  }
}