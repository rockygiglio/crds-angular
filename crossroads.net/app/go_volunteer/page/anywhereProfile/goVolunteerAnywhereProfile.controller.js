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
    const { initiativeId, projectId } = this.stateParams;

    if (this.anywhereForm.$valid) {
      // TODO: handle the promise that comes back
      this.goVolunteerAnywhereProfileForm.save(parseInt(initiativeId, 10), projectId);
    } else {
      // TODO: throw error
    }
  }
}
