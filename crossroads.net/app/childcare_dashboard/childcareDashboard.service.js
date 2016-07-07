/* ngInject */
class ChildcareDashboardService { 
  constructor($resource, $cookies) {
    this.resource = $resource;
    this.cookies = $cookies;
    this.headOfHouseholdError = false;
    this.dashboard = $resource(__API_ENDPOINT__ + `api/childcare/dashboard/:contactId`);
  }

  fetchChildcareDates() {
    const contactId = this.cookies.get('userId');
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
