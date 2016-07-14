import NoTakeBacksController from './noTakeBacks.controller';
require('./noTakeBacks.html');

/*@ngInject*/
class ChildcareDashboardGroupController {
  constructor($rootScope, $scope, $modal, ChildcareDashboardService) {
    this.message = '';
    this.modal = $modal;
    this.root = $rootScope;
    this.childcareService = ChildcareDashboardService;
    if(this.isEventClosed()) {
      this.message = $rootScope.MESSAGES.childcareEventClosed.content;
    }
    if(!this.hasEligibleChildren()) {
      this.message = $rootScope.MESSAGES.noEligibleChildren.content;
    }
  }

  getCongregation(congregationId) {
      const record = this.childcareService.congregations.filter((con) => {
      return con.dp_RecordID === congregationId;
    });

    if (record.length > 0) {
      return record[0].dp_RecordName;
    }
    return 'Unknown';
  }

  hasEligibleChildren() {
    if (this.communityGroup === undefined) {
      return false;
    }
    return this.communityGroup.eligibleChildren.length > 0;
  }

  hasSignedUpChild() {
    const rsvpd = this.communityGroup.eligibleChildren.filter( (child) => {
      return child.rsvpness;
    });
    return rsvpd.length > 0;
  }

  isEventCancelled() {
    if (this.cancelled !== undefined && this.cancelled) {
      this.message = this.root.MESSAGES.childcareEventCancelled.content;
      return true;
    }

    return false;
  }

  isEventClosed() {
    const today = moment({ hour:0, minute:0 });
    const otherDate = moment(this.eventDate).set({ hour: 0, minute: 0});
    var diff = today.diff(otherDate, 'days');
    return diff >= -7;
  }

  rsvp(child) {
    var resp = this.childcareService.saveRSVP(child.contactId, this.communityGroup.childcareGroupId, child.rsvpness);
    resp.$promise.then(() => {

    }, (err) => {
      child.rsvpness = !child.rsvpness;
      // display an error message...
      if (err.statusCode === 412) {
        this.root.$emit('notify', this.root.MESSAGES.childcareRsvpFull);
      } else {
        this.root.$emit('notify', this.root.MESSAGES.childcareRsvpError);
      }
    });
  }

  shouldAsk(currentStatus) {
    return !currentStatus && this.isEventClosed();
  }

  showMessage(){
    return this.message.length >0;
  }

  showModal() {
    let modalInstance = this.modal.open({
      templateUrl: 'childcare_group/noTakeBacks.html',
      controller: NoTakeBacksController,
      controllerAs: 'noTakeBacks',
      size: 'sm'
    });

    return modalInstance.result;
  }

  validateAndRsvp(child) {
    if (this.shouldAsk(child.rsvpness)) {
      this.showModal().then( () => {
        this.rsvp(child);
      }, () => {
        child.rsvpness = !child.rsvpness;
      });
    } else { 
      this.rsvp(child);
    }
    return true;
  }

}
export default ChildcareDashboardGroupController;

