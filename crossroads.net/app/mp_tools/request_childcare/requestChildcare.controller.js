import moment from 'moment';
require('moment-recur');

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
    this.datesList = [];
    this.customSessionSelected = false;
    this.customSessionTime = 'Customize My Childcare Session...';
    this.loadingGroups = false;
    this.log = $log;
    this.ministries = RequestChildcareService.getMinistries();
    this.minDate = new Date();
    this.minDate.setDate(this.minDate.getDate() + 7);
    this.name = 'request-childcare';
    this.requestChildcareService = RequestChildcareService;
    this.rootScope = $rootScope;
    this.runDateGenerator = true;
    this.startTime = new Date();
    this.startTime.setHours(9);
    this.startTime.setMinutes(30);
    this.endTime = this.startTime;
    this.uid = $cookies.get('userId');
    this.validation = Validation;
    this.viewReady = true;
    this.window = $window;
  }

  generateDateList() {
    if (this.runDateGenerator) {
      const start = moment(this.startDate);
      const end = moment(this.endDate);
      if (this.choosenFrequency === 'Weekly') {
        let weekly = moment().recur(start, end).every(this.choosenPreferredTime.Meeting_Day).daysOfWeek();
        this.datesList = weekly.all().map( (d) => {
          return { 
            unix: d.unix(),
            date: d,
            selected: true
          };
        });
      } else if (this.choosenFrequency === 'Monthly') {
        let weekOfMonth = this.getWeekOfMonth(start);
        let monthly = moment().recur(start, end)
          .every(this.choosenPreferredTime.Meeting_Day).daysOfWeek()
          .every(weekOfMonth -1).weeksOfMonthByDay();
        this.datesList = monthly.all().map( (d) => {
          return {
            unix: d.unix(),
            date: d,
            selected: true
          };
        });
      } else {
        this.datesList = [];
      }
      this.runDateGenerator = false;
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

  getWeekOfMonth(startDate) {
    return Math.ceil(startDate.date() / 7);
  }

  onEndDateChange() {
    this.runDateGenerator = true;
  }

  onFrequencyChange() {
    this.runDateGenerator = true;
  }

  onPreferredTimeChange() {
    this.runDateGenerator = true;
  }

  onStartDateChange(startDate) {
    this.runDateGenerator = true;
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
      const start = this.startDate.getTime();
      const end = this.endDate.getTime();
      if (start !== end && start < end) {
        this.generateDateList();
        return true;
      }
      return false;
    }
    return false;
  }

  showGroups() {
    return this.choosenCongregation && this.choosenMinistry && this.groups.length > 0;
  }

  getAvailableTimes() {
    var availableTimes = [];
    for (var time of this.filteredTimes)
    {
      availableTimes.push(this.formatPreferredTime(time));
    }

    availableTimes.push(this.customSessionTime);
    return availableTimes;
  }

  preferredTimeChanged() {
    if (this.choosenPreferredTime == this.customSessionTime) {
      this.customSessionSelected = true;
    } else {
      this.customSessionSelected = false;
    }
  }

  formatPreferredTime(time) {
    var startTime = {};
    var endTime = {};
    var day = '';

    if (this.customSessionSelected) {
      startTime = moment(this.startTime);
      endTime = moment(this.endTime);
      day = this.dayOfWeek;
    } else {
      const startTimeArr = time['Childcare_Start_Time'].split(':');
      const endTimeArr = time['Childcare_End_Time'].split(':');
      startTime = moment().set(
        {'hour': parseInt(startTimeArr[0]), 'minute': parseInt(startTimeArr[1])});
      endTime = moment().set(
        {'hour': parseInt(endTimeArr[0]), 'minute': parseInt(endTimeArr[1])});
      day = time['Meeting_Day'];
    }

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
        notes: this.notes,
        dates: this.datesList.filter( (d) => { return d.selected === true;}).map( (d) => { return d.date; })
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

  validTimeRange(form) {
    if (form === undefined) {
      return false;
    }

    //verify that times are valid;
    var start;
    var end;
    try {
      start =  moment(this.startTime);
      end = moment(this.endTime);
    } catch (err) {
      form.endTime.$error.invalidEnd = true;
      form.endTime.$valid = false;
      form.endTime.$invalid = true;
      form.endTime.$dirty = true;
      form.$valid = false;
      form.$invalid = true;
      return true;
    }

    if (start <= end) {
      form.endTime.$error.invalidEnd = false;
      form.endTime.$valid = true;
      form.endTime.$invalid = false;
      return false;
    }

    // set the endTime Invalid...
    form.endTime.$error.invalidEnd = true;
    form.endTime.$valid = false;
    form.endTime.$invalid = true;
    form.endTime.$dirty = true;
    form.$valid = false;
    form.$invalid = true;
    return true;
  }
}
export default RequestChildcareController;
