class CampDashboardController {
  constructor(CampsService, $state) {
    this.campsService = CampsService;
    this.state = $state;
    this.viewReady = false;
  }

  $onInit() {
    this.data = this.campsService.dashboard;
    this.viewReady = true;
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
