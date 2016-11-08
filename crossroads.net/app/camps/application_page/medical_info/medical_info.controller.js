class MedicalInfoController {
  constructor(MedicalInfoForm, $rootScope, $stateParams, $state) {
    this.medicalInfoForm = MedicalInfoForm;
    this.rootScope = $rootScope;
    this.stateParams = $stateParams;
    this.state = $state;
    this.viewReady = false;
    this.submitting = false;
    this.update = true;
  }

  $onInit() {
    this.viewReady = true;
    this.model = this.medicalInfoForm.getModel();
    this.fields = this.medicalInfoForm.getFields();
  }

  submit() {
    this.submitting = true;
    if (this.medicalInfo.$valid) {
      this.medicalInfoForm.save(this.stateParams.contactId).then(() => {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.successfulSubmission);
      }).catch(() => {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      }).finally(() => {
        this.submitting = false;
        if (this.update) {
          // navigae back to mycamps page
          this.state.go('camps-dashboard');
        }
      });
    } else {
      this.submitting = false;
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
    }
  }
}

export default MedicalInfoController;
