class CamperInfoController {
  /* @ngInject */
  constructor(LookupService, CamperInfoForm, $rootScope, $state) {
    this.camperInfoForm = CamperInfoForm.createForm();
    this.lookupService = LookupService;
    this.rootScope = $rootScope;
    this.state = $state;
    this.submitting = false;
    this.viewReady = false;
  }

  $onInit() {
    this.viewReady = true;
    this.model = this.camperInfoForm.getModel();
    this.fields = this.camperInfoForm.getFields();
  }

  submit() {
    // TODO: change contactId if it === 'new'
    this.submitting = true;

    if (this.infoForm.$valid) {
      this.camperInfoForm.save(this.state.toParams.campId).then(() => {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.successfullRegistration);

        this.state.go('campsignup.application', {
          page: 'emergency-contact',
          contactId: this.state.toParams.contactId
        });
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
