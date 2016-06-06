class ChildcareDecisionController {
  /*@ngInject*/
  constructor(
      $rootScope,
      MPTools,
      CRDS_TOOLS_CONSTANTS,
      $log,
      $window,
      ChildcareDecisionService
  ) {

    this.allowAccess = MPTools.allowAccess(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.ChildcareDecisionTool);
    this.childcareDecisionService = ChildcareDecisionService;
    this.log = $log;
    this.mptools = MPTools;
    this.name = 'childcare-decision';
    this._window = $window;

    if ( this.allowAccess) {
      this.recordId = Number(MPTools.getParams().recordId);
      if (this.recordId === -1) {
        this.viewReady = true;
        this.error = true;
        this.errorMessage = $rootScope.MESSAGES.mptool_access_error;
      } else {
        this.request = this.childcareDecisionService.getChildcareRequest(this.recordId);
        this.request.$promise.then(() => {
          this.viewReady = true;
        });
      }
    }
  }

  showError() {
    return this.error === true ? true : false;
  }

  submit() {
    this.saving = true;
    this.saved = this.childcareDecisionService.saveRequest(this.request, (data) => {
      this.log('success!', data);
    }, (err) => {
      this.log.error('error!', err);
    });
    this.saved.$promise.then(this.saving = false );
  }

}
export default ChildcareDecisionController;

