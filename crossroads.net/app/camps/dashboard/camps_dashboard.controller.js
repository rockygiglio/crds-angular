class CampDashboardController {
  constructor(CampsService, $state, $rootScope) {
    this.campsService = CampsService;
    this.state = $state;
    this.viewReady = false;
    this.state = $state;
    this.rootScope = $rootScope;
  }

  $onInit() {
    this.data = this.campsService.dashboard;
    this.viewReady = true;
    this.onPayment();
  }

  onPayment() {
    const paymentId = this.state.toParams.paymentId;
    if (paymentId) {
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.successfulSubmission);
    }
  }

  isDashboardEmpty() {
    return this.data.length < 1;
  }

  // eslint-disable-next-line class-methods-use-this
  fullName(lastName, nickName) {
    return `${nickName} ${lastName}`;
  }
}

export default CampDashboardController;
