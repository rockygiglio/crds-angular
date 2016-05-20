class RequestChildcareController {
  /*@ngInject*/
  constructor($rootScope, MPTools, CRDS_TOOLS_CONSTANTS, $log, RequestChildcareService, Validation) {
    this.allowAccess = MPTools.allowAccess(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.ChildcareRequestTool);
    this.name = 'request-childcare';
    this._$rootScope = $rootScope;
    this._MPTools = MPTools;
    this._CRDS_TOOLS_CONSTANTS = CRDS_TOOLS_CONSTANTS;
    this.log = $log;
    this.params = MPTools.getParams();
    this.congregations = RequestChildcareService.getCongregations();
    this.ministries = RequestChildcareService.getMinistries();
    this.minDate = new Date();
    this.minDate.setDate(this.minDate.getDate() + 7);
    this.currentRequest = Number(this.params.recordId);
    this.validation = Validation;
    this.viewReady = true;
  }

  getGroups() {
    if (showGroups()) {
      //TODO:  
    }
  }

  showGroups() {
    return this.choosenCongregation && this.choosenMinistry;
  }

  submit() {
    if (this.childcareRequestForm.$invalid) {
      this.log.debug('form is not valid');
      return false;
    } else {
      this.log.debug('form is ready to be submitted');
    }
  }

  validateField(fieldName) {
    return this.validation.showErrors(this.childcareRequestForm, fieldName);
  }
}
export default RequestChildcareController;
