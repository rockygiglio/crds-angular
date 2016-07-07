/*@ngInject*/
class ChildcareDashboardGroupController {
  constructor($rootScope, ChildcareDashboardService) {
    this.message = '';
    this.root = $rootScope;
    this.childcareService = ChildcareDashboardService;
    if(this.isEventClosed()) {
      this.message = $rootScope.MESSAGES.childcareEventClosed.content;
    }
    if(!this.hasEligibleChildren()) {
      this.message = $rootScope.MESSAGES.noEligibleChildren.content;
    }
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

  showMessage(){
    return this.message.length >0;
  }

  isEventClosed() {
    const today = moment();
    const otherDate = moment(this.eventDate);
    var diff = today.diff(otherDate, 'days');
    return diff >= -7;
  }

  isEventCancelled() {
    if (this.cancelled !== undefined && this.cancelled) {
      this.message = this.root.MESSAGES.childcareEventCancelled.content;
      return true;
    }

    return false;
  }

}
export default ChildcareDashboardGroupController;

