class CampsFamilyController {
  constructor(CampsService, CamperInfoForm, EmergencyContactForm, MedicalInfoForm, ProductSummaryForm, $log, $rootScope) {
    this.campsService = CampsService;
    this.log = $log;
    this.rootScope = $rootScope;
    this.formData = {};
  }

  $onInit() {
    this.log.debug('Camps Family Controller Initialized!');
    this.cmsMessage = this.rootScope.MESSAGES.summercampIntro.content;
    this.resetCampForm();
  }

  resetCampForm() {
    alert(JSON.stringify(this.formData), null, 2);
    this.CamperInfoForm.formData = this.formData;
  }

  // eslint-disable-next-line class-methods-use-this
  isSignedUp(member) {
    return member.signedUpDate !== null;
  }

  divClass(member) {
    if (!this.isSignedUp(member) && member.isEligible) {
      return 'col-sm-9 col-md-10';
    }
    return '';
  }

  pClass(member) {
    if (!this.isSignedUp(member)) {
      return 'flush-bottom';
    }

    return '';
  }
}
export default CampsFamilyController;
