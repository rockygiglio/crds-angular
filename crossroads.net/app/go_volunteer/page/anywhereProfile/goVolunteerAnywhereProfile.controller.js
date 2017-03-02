export default class GoVolunteerAnywhereProfileController {
  /* @ngInject */
  constructor(GoVolunteerAnywhereProfileForm, GoVolunteerService) {
    this.viewReady = false;
    this.goVolunteerAnywhereProfileForm = GoVolunteerAnywhereProfileForm;
    this.goVolunteerService = GoVolunteerService;
    this.project = GoVolunteerService.project;
  }

  $onInit() {
    this.viewReady = true;
    this.model = this.goVolunteerAnywhereProfileForm.getModel();
    this.fields = this.goVolunteerAnywhereProfileForm.getFields();
  }
}
