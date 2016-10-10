/* @ngInject */
class CamperInfoController {
  constructor(LookupService, CamperInfoForm, $rootScope) {
    this.viewReady = false;
    this.lookupService = LookupService;
    this.camperInfoForm = CamperInfoForm;
    this.rootScope = $rootScope;
    this.options = {};
  }

  $onInit() {
    this.viewReady = true;
    this.model = this.camperInfoForm.getModel();
    this.fields = this.camperInfoForm.getFields();
  }

  submit() {
    console.log('submitting app');
    if(this.infoForm.$valid) {
      //save the form
      this.camperInfoForm.save();
    } else {
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
    }
  }
}

export default CamperInfoController;
