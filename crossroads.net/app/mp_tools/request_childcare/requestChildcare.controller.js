class RequestChildcareController {
  /*@ngInject*/
  constructor($rootScope,
              MPTools,
              CRDS_TOOLS_CONSTANTS,
              $log,
              RequestChildcareService,
              Validation,
              $cookies,
              $window) {
    this.allowAccess = MPTools.allowAccess(CRDS_TOOLS_CONSTANTS.SECURITY_ROLES.ChildcareRequestTool);
    this.congregations = RequestChildcareService.getCongregations();
    this.currentRequest = Number(MPTools.getParams().recordId);
    this.loadingGroups = false;
    this.log = $log;
    this.ministries = RequestChildcareService.getMinistries();
    this.minDate = new Date();
    this.minDate.setDate(this.minDate.getDate() + 7);
    this.name = 'request-childcare';
    this.requestChildcareService = RequestChildcareService;
    this.rootScope = $rootScope;
    this.uid = $cookies.get('userId');
    this.validation = Validation;
    this.viewReady = true;
    this.window = $window;
  }

  getGroups() {
    if (this.choosenCongregation && this.choosenMinistry) {
      this.loadingGroups = true;
      this.groups = this.requestChildcareService
        .getGroups(this.choosenCongregation.dp_RecordID, this.choosenMinistry.dp_RecordID); 
      this.groups.$promise
        .then(data => this.loadingGroups = false, err => this.loadingGroups = false);
      this.preferredTimes = this.requestChildcareService.getPreferredTimes(this.choosenCongregation.dp_RecordID);
    }
  }

  showGroups() {
    return this.choosenCongregation && this.choosenMinistry && this.groups.length > 0 ;
  }

  formatPreferredTime(time) {
    const startTimeArr = time['Childcare_Start_Time'].split(':');
    const endTimeArr = time['Childcare_End_Time'].split(':');
    const startTime = moment().set(
      {'hour': parseInt(startTimeArr[0]), 'minute': parseInt(startTimeArr[1])});
    const endTime = moment().set(
      {'hour': parseInt(endTimeArr[0]), 'minute': parseInt(endTimeArr[1])});
    return `${startTime.format('h:mmA')} - ${endTime.format('h:mmA')}`;
  }

  submit() {
    this.saving = true;
    if (this.childcareRequestForm.$invalid) {
      this.saving = false;
      return false;
    } else {
      const dto = {
        requester: this.uid,
        site: this.choosenCongregation.dp_RecordID,
        ministry: this.choosenMinistry.dp_RecordID,
        group: this.choosenGroup.dp_RecordID,
        startDate: moment(this.startDate).utc(),
        endDate: moment(this.endDate).utc(),
        frequency: this.choosenFrequency,
        timeframe: this.formatPreferredTime(this.choosenPreferredTime),
        estimatedChildren: this.numberOfChildren,
        notes: this.notes
      };
      const save = this.requestChildcareService.saveRequest(dto);
      save.$promise.then((data) => {
        this.log.debug('saved!');
        this.saving = false;
        this.window.close();
      }, (err) => {
        this.saving = false;
        this.log.error('error!'); 
        this.saving = false;
      });
    }
  }

  validateField(fieldName) {
    return this.validation.showErrors(this.childcareRequestForm, fieldName);
  }
}
export default RequestChildcareController;
