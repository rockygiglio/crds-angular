class CampDashboardController {
  constructor(CampsService) {
    this.campsService = CampsService;
    this.viewReady = false;
  }

  $onInit() {
    this.data = this.campsService.dashboard;
    this.viewReady = true;
  }

  isDashboardEmpty() {
    return this.data.length < 1;
  }

  fullName(lastName, nickName) {
    return `${nickName} ${lastName}`;
  }
}

export default CampDashboardController;
