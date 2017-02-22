export default class GoVolunteerAnywhereProfileController {
  /* @ngInject */
  constructor(GoVolunteerAnywhereProfileForm) {
    this.viewReady = false;
    this.goVolunteerAnywhereProfileForm = GoVolunteerAnywhereProfileForm;
  }

  $onInit() {
    this.viewReady = true;
    this.model = this.goVolunteerAnywhereProfileForm.getModel();
    this.fields = this.goVolunteerAnywhereProfileForm.getFields();
  }
}
