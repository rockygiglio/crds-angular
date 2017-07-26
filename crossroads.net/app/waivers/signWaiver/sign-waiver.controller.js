
/* @ngInject */
export default class SignWaiverController {
  constructor($log, $state, $rootScope, WaiversService) {
    this.log = $log;
    this.state = $state;
    this.rootScope = $rootScope;
    this.waiversService = WaiversService;
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

    this.waiversService.sendAcceptEmail(waiverId, eventParticipantId).then(() => {
      this.processing = false;
    }).catch((err) => {
      this.processing = false;
      this.log.error(err);
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
    })
  }

  cancel() {
    this.state.go('mytrips');
  }
}
