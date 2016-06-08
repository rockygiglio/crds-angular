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

    if ( this.allowAccess) {
      this.recordId = Number(MPTools.getParams().recordId);
      if (this.recordId === -1) {
        this.viewReady = true;
        this.error = true;
        this.errorMessage = $rootScope.MESSAGES.mptool_access_error;
      } else {
        this.request = this.childcareDecisionService.getChildcareRequest(this.recordId, (d) => {
          this.startDate = moment(d.StartDate).format('L');
          this.endDate = moment(d.EndDate).format('L');
        });
        this.request.$promise.then(() => {
          this.viewReady = true;
        });
        this.datesList = this.childcareDecisionService.getChildcareRequestDates(this.recordId);
        this.datesList.$promise.then((d)=>{
            this.datesList = d;
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

  showError() {
    return this.error === true ? true : false;
  }

  submit() {
    this.saving = true;
    this.saved = this.childcareDecisionService.saveRequest(this.recordId, this.request, (data) => {
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
      } else {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      }
      this.log.error('error!', err);
    });
  }

}
export default ChildcareDecisionController;

