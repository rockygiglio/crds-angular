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
    this.rootScope = $rootScope;
    this._window = $window;

    if (this.allowAccess) {
      this.recordId = Number(MPTools.getParams().recordId);
      if (!this.recordId || this.recordId === -1 ) {
        this.viewReady = true;
        this.error = true;
        this.errorMessage = $rootScope.MESSAGES.mptool_access_error;
      } else {
        this.request = this.childcareDecisionService.getChildcareRequest(this.recordId, (d) => {
          this.startDate = moment(d.StartDate).format('L');
          this.endDate = moment(d.EndDate).format('L');
          this.datesList = d.DatesList.map( (date) => {
            return { selected: false, date: moment(date) };
          });
        });
        this.request.$promise.then(() => {
          this.viewReady = true;
        });
      }
    }
  }

  allowApproval() {
    return this.request.Status !== 'Approved';
  }

  isLoading() {
    return this.saving || !this.allowApproval();
  }

  loadingText() {
    if (this.allowApproval()) {
      return 'Approving...';
    } else {
      return 'Approve';
    }
  }

  missingEventContent(dateList) {
    let dateListLI = dateList.map( (d) => {
      return `<li> ${moment(d).format('L')} </li>`;
    }).reduce((first, next) => {
     return `${first} ${next}`;
    }, '');
    let dateListUL = `<ul>${dateListLI} </ul>`;
    let content ='<p><strong>Missing Childcare Events</strong>' +
      dateListUL + '</p>';
    return content;
  }

  missingChildcareDates() {
      let content ='<p><strong>Childcare request has no associated dates.</strong></p>';
    return content;
  }

  showDates() {
    return this.datesList.length > 0;
  }

  showError() {
    return this.error === true ? true : false;
  }

  submit() {
    this.saving = true;
    if (!this.validDates()) {
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.noDatesChosen);
      this.saving = false;
      return false;
    }
    let dto = {
      requester: this.request.RequesterId,
      site: this.request.LocationId,
      ministry: this.request.MinistryId,
      group: this.request.GroupId,
      startDate: this.request.StartDate,
      endDate: this.request.EndDate,
      frequency: this.request.Frequency,
      dates: this.datesList.map((date) => {
        return date.date.utc();
      }),
      decisionNotes: this.request.decisionNotes
    };
    this.saved = this.childcareDecisionService.saveRequest(this.recordId, dto, (data) => {
      this.saving = false;
      this.log('success!', data);
      this._window.close();
    }, (err) => {
      this.saving = false;
      if (err.status === 416) {
        this.rootScope.$emit('notify', {
          content: this.missingEventContent(err.data.Errors),
          type: 'error'
        });
      }
      else if (err.status === 406) {
        this.rootScope.$emit('notify', {
          content: this.missingChildcareDates(),
          type: 'error'
        });
      }
      else {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      }
      this.log.error('error!', err);
    });
  }

  validDates() {
    if (!this.datesList || this.datesList.length < 1) {
      return false;
    }

    let found = this.datesList.filter((d) => {
      return d.selected;
    });
    return found.length > 0;
  }

}
export default ChildcareDecisionController;

