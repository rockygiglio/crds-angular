
import GroupMessage from '../model/groupMessage';

export default class ConfirmRequestController {
  /*@ngInject*/
  constructor(MessageService) {
    this.messageService = MessageService;

    this.processing = false;
    this.emailLeader = (this.emailLeader === undefined) ? false : this.emailLeader;
  }

  $onInit() {
    if(this.emailLeader){
      this.emailGroupLeader();
    }
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

      this.groupMessage = new GroupMessage();
      this.groupMessage.groupId = this.group.groupId;
      this.groupMessage.subject = '';
      this.groupMessage.body = '';
    }
  }

  sendEmail() {
    this.processing = true;

    this.messageService.sendGroupMessage(this.groupId, this.groupMessage).then(
        () => {
          this.groupMessage = undefined;
          this.modalInstance.dismiss();
          this.rootScope.$emit('notify', this.rootScope.MESSAGES.emailSent);
        },
        (error) => {
          this.rootScope.$emit('notify', this.rootScope.MESSAGES.emailSendingError);
        }
    ).finally(() => {
      this.processing = false;
    });
  }

  submit() {
    this.processing = true;

    // TODO - Remove timeout faking submission for loading-button
    window.setTimeout(() => {
      this.modalInstance.dismiss();
    }, 2000);
  }
}