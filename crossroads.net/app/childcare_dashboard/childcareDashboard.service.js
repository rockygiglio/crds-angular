/* ngInject */
class ChildcareDashboardService { 
  constructor($resource, Session) {
    this.resource = $resource;
    this.session = Session;
    this.congregations = [];
    this.headOfHouseholdError = false;
    this.dashboard = $resource(__GATEWAY_CLIENT_ENDPOINT__ + `api/childcare/dashboard/:contactId`);
    this.saveRsvp = $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/childcare/rsvp');
  }

  fetchChildcareDates() {
    const contactId = this.session.exists('userId');
    return this.dashboard.get({contactId});
  }

  getChildcareDates() {
    return this.childcareDates;
  }

  saveRSVP(childId, groupId, enrolledBy, registered) {
    const dto = {
      groupId,
      childId,
      registered,
      enrolledBy
    };
    return this.saveRsvp.save(dto);
  }

  updateHeadOfHouseholdError(state) {
    this.headOfHouseholdError = state;
  }
}

export default ChildcareDashboardService;
