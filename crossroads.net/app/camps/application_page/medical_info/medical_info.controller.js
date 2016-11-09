/* @ngInject */
class MedicalInfoController {
  constructor(MedicalInfoForm, $rootScope, $state) {
    this.medicalInfoForm = MedicalInfoForm;
    this.rootScope = $rootScope;
    this.go = $state.go;
    this.stateParams = $state.params;
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
        this.nextPage(this.model.contactId);
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

  nextPage(camperId) {
    this.go('campsignup.application', { page: 'product-summary', camperId });
  }
}

export default MedicalInfoController;
