class MedicalInfoController {
  constructor(MedicalInfoForm, $rootScope, $stateParams) {
    this.medicalInfoForm = MedicalInfoForm;
    this.rootScope = $rootScope;
    this.stateParams = $stateParams;
    this.viewReady = false;
    this.submitting = false;
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
      });
    } else {
      this.submitting = false;
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
    }
  }
}

export default MedicalInfoController;
