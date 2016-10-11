/* @ngInject */
class CamperInfoController {
  constructor(LookupService, CamperInfoForm, $rootScope, $stateParams) {
    this.camperInfoForm = CamperInfoForm;
    this.lookupService = LookupService;
    this.options = {};
    this.rootScope = $rootScope;
    this.stateParams = $stateParams;
    this.viewReady = false;
  }

  $onInit() {
    this.viewReady = true;
    this.model = this.camperInfoForm.getModel();
    this.fields = this.camperInfoForm.getFields();
  }

  submit() {
    if(this.infoForm.$valid) {
      console.log('submitting app');
      //save the form
      this.camperInfoForm.save(this.stateParams.campId).then((data) => {
        console.log('successfully saved camperinfo', data);
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.successfullRegistration);
      }, (err) => {
        console.log('unable to save camperinfo', err);
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      });
    } else {
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
    }
  }
}

export default CamperInfoController;
