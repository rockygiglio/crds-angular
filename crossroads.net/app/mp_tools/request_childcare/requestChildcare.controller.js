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

  generateDateList(startDate, endDate, dayOfTheWeek, frequency) {
    const start = moment(startDate);
    const end = moment(endDate);
    const day = moment().day(dayOfTheWeek).day();
    const actualStart = this.getStartDate(start, day);
    let dateList = [actualStart];
    if (frequency === 'Weekly') {
      let dateIter = moment(actualStart);
      dateIter.add(1, 'w');
      while (dateIter.isBefore(end)) {
        const nextDate = moment(dateIter);
        dateList = [...dateList, nextDate];
      }
    } else if (frequency === 'Monthly') {
      let dateIter = moment(actualStart);
      dateIter.add(1, 'm');
      while (dateIter.isBefore(end)) {
        const nextDate = moment(dateIter);
        dateList = [...dateList, nextDate];
      }
    }
    return dateList;
  }

  getStartDate(startDate, dayOfWeek) {
    if(startDate.day() === dayOfWeek) {
      return startDate;
    } else if(startDate.day() < dayOfWeek){
      return startDate.add(dayOfWeek - startDate.day(), 'd');
    } else {
      startDate.add(8 - startDate.day(), 'd');
      startDate.add(dayOfWeek - startDate.day(), 'd');
      return startDate;
    }
  }

  getGroups() {
    if (this.choosenCongregation && this.choosenMinistry) {
      this.loadingGroups = true;
      this.groups = this.requestChildcareService
        .getGroups(this.choosenCongregation.dp_RecordID, this.choosenMinistry.dp_RecordID); 
      this.groups.$promise
        .then(() => this.loadingGroups = false, () => this.loadingGroups = false);
      this.preferredTimes = this.requestChildcareService.getPreferredTimes(this.choosenCongregation.dp_RecordID);
      this.filteredTimes = this.preferredTimes;
    }
  }

  onStartDateChange(startDate) {
    this.filteredTimes = this.preferredTimes.filter((time) => {
      if (time.Deactivate_Date === null) { return true; }
      var preferredStart = moment(startDate);
      var deactivateDate = moment(time.Deactivate_Date);
      return preferredStart.isBefore(deactivateDate) || preferredStart.isSame(deactivateDate); 
    });
  }

  showGaps() {
    if (this.choosenPreferredTime && this.choosenFrequency &&
        this.choosenFrequency !== 'Once' &&
        this.startDate &&
        this.endDate) {
      const start = this.startDate.getDate() + this.startDate.getMonth() + this.startDate.getFullYear();
      const end = this.endDate.getDate() + this.endDate.getMonth() + this.endDate.getFullYear();
      console.log(this.choosenPreferredTime);
      return start !== end;
    }
    return false;
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
    const day = time['Meeting_Day'];
    return `${day}, ${startTime.format('h:mmA')} - ${endTime.format('h:mmA')}`;
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
        notes: this.notes
      };
      const save = this.requestChildcareService.saveRequest(dto);
      save.$promise.then(() => {
        this.log.debug('saved!');
        this.saving = false;
        this.window.close();
      }, () => {
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
