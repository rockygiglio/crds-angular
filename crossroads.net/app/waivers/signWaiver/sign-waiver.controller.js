import confirmationModal from './confirmationModal.html';

/* @ngInject */
export default class SignWaiverController {
  constructor($log, $state, $rootScope, WaiversService, $modal) {
    this.log = $log;
    this.state = $state;
    this.rootScope = $rootScope;
    this.waiversService = WaiversService;
    this.modal = $modal;
  }

  $onInit() {
    this.viewReady = false;
    this.processing = false;

    this.waiversService.getWaiver(this.state.params.waiverId).then((res) => {
      this.waiver = res;
      this.viewReady = true;
    }).catch((err) => {
      this.viewReady = true;
      this.log.error(err);
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
    });
  }

  submit() {
    this.processing = true;

    const { waiverId, eventParticipantId } = this.state.params;

    this.waiversService.sendAcceptEmail(parseInt(waiverId, 10), eventParticipantId).then(() => {
      this.processing = false;
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.waiverEmailSent);
      this.state.go('mytrips');
    }).catch((err) => {
      this.processing = false;
      this.log.error(err);
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
    });
  }

  cancel() {
    this.state.go('mytrips');
  }

  open() {
    const modal = this.modal.open({
      animation: true,
      ariaLabelledBy: 'modal-title',
      ariaDescribedBy: 'modal-body',
      template: confirmationModal,
      size: undefined,
      appendTo: undefined
    });

    modal.result.then(() => console.log('yes'), () => console.error('no'));
  }
}
