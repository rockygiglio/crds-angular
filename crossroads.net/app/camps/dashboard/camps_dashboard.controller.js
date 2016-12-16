class CampDashboardController {
  constructor(CampsService, $state, $rootScope, $window) {
    this.campsService = CampsService;
    this.viewReady = false;
    this.state = $state;
    this.rootScope = $rootScope;
    this.window = $window;
  }

  $onInit() {
    this.data = this.campsService.dashboard;
    this.viewReady = true;
    this.onPayment();
  }

  onPayment() {
    this.url = this.window.location.href.split('=');
    this.paymentId = this.url[2];
    if (!(this.paymentId === null)) {
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.successfulSubmission);
    }
  }

  isDashboardEmpty() {
    return this.data.length < 1;
  }

  fullName(lastName, nickName) {
    return `${nickName} ${lastName}`;
  }
}

export default CampDashboardController;
