class EmergencyContactController {
  constructor(EmergencyContactForm) {
    this.emergencyContactForm = EmergencyContactForm;
  }

  $onInit() {
    this.viewReady = true;
    this.model = this.emergencyContactForm.getModel();
    this.fields = this.emergencyContactForm.getFields();
  }
}

export default EmergencyContactController;
