export default class GoVolunteerAnywhereProfileConfirmController {
  /* @ngInject */
  constructor( GoVolunteerService) {
    this.viewReady = false;
    this.goVolunteerService = GoVolunteerService;
    this.project = GoVolunteerService.project;
  }

  $onInit() {
    this.viewReady = true;
  }
}
