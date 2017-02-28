import moment from 'moment';

export default class GoVolunteerAnywhereProfileController {
  /* @ngInject */
  constructor(GoVolunteerAnywhereProfileForm, GoVolunteerService, $state) {
    this.viewReady = false;
    this.goVolunteerAnywhereProfileForm = GoVolunteerAnywhereProfileForm;
    this.goVolunteerService = GoVolunteerService;
    this.stateParams = $state.toParams;
    this.project = GoVolunteerService.project;
  }

  $onInit() {
    this.viewReady = true;
    this.model = this.goVolunteerAnywhereProfileForm.getModel();
    this.fields = this.goVolunteerAnywhereProfileForm.getFields();
  }

  submit() {
    // TODO: implement

    if (this.anywhereForm.$valid) {
      console.log(this.stateParams);
      this.goVolunteerAnywhereProfileForm.save();
    }
  }
}
