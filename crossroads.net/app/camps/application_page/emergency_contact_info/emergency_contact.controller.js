class EmergencyContactController {
  constructor(EmergencyContactForm, $rootScope, $stateParams) {
    this.emergencyContactForm = EmergencyContactForm;
    this.rootScope = $rootScope;
    this.stateParams = $stateParams;
    this.viewReady = false;
    this.submitting = false;
  }

  $onInit() {
    this.viewReady = true;
    this.model = this.emergencyContactForm.getModel();
    this.fields = this.emergencyContactForm.getFields();
  }

  submit() {
    // TODO: throw error if not all required fields
    // TODO: disable button until request completes

    this.submitting = true;
    if (this.emergencyContact.$valid) {
      this.emergencyContactForm.save(this.stateParams.campId, this.stateParams.contactId).then(() => {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.successfulSubmission);
        this.options.resetModel();
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

export default EmergencyContactController;
