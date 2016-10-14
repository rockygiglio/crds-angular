/* @ngInject */
class CamperInfoController {
  constructor(LookupService, CamperInfoForm, $rootScope, $stateParams) {
    this.camperInfoForm = CamperInfoForm;
    this.lookupService = LookupService;
    this.rootScope = $rootScope;
    this.stateParams = $stateParams;
    this.submitting = false;
    this.viewReady = false;
  }

  $onInit() {
    this.viewReady = true;
    this.model = this.camperInfoForm.getModel();
    this.fields = this.camperInfoForm.getFields();
  }

  submit() {
    this.submitting = true;
    if (this.infoForm.$valid) {
      this.camperInfoForm.save(this.stateParams.campId).then(() => {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.successfullRegistration);

        this.options.resetModel();
      },

      () => {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      }).finally(() => {
        this.submitting = false;
      });
    } else {
      this.submitting = false;
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
    }
  }
}

export default CamperInfoController;
