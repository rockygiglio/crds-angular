export default class GoVolunteerAnywhereProfileController {
  /* @ngInject */
  constructor(GoVolunteerAnywhereProfileForm, GoVolunteerService, $state, $rootScope, $log) {
    this.viewReady = false;
    this.submitting = false;
    this.goVolunteerAnywhereProfileForm = GoVolunteerAnywhereProfileForm;
    this.goVolunteerService = GoVolunteerService;
    this.state = $state;
    this.stateParams = $state.toParams;
    this.rootScope = $rootScope;
    this.log = $log;
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
      this.submitting = true;
      this.goVolunteerAnywhereProfileForm.save(parseInt(initiativeId, 10), projectId)
        .then(() => {
          this.state.go('go-local.anywhereconfirm', this.stateParams, { inherit: true });
        }).catch((err) => {
          this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
          this.log.error(err);
        }).finally(() => {
          this.submitting = false;
        });
    } else {
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
    }
  }
}
