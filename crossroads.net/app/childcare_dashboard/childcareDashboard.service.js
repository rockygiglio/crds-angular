/* ngInject */
class ChildcareDashboardService { 
  constructor($resource, Session) {
    this.resource = $resource;
    this.session = Session;
    this.congregations = [];
    this.headOfHouseholdError = false;
    this.dashboard = $resource(__API_ENDPOINT__ + `api/childcare/dashboard/:contactId`);
  }

  fetchChildcareDates() {
    const contactId = this.session.exists('userId');
    return this.dashboard.get({contactId});
  }

  getChildcareDates() {
    return this.childcareDates;
  }

  updateHeadOfHouseholdError(state) {
    this.headOfHouseholdError = state;
  }
}

export default ChildcareDashboardService;
