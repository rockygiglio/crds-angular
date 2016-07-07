/* ngInject */
class ChildcareDashboardService { 
  constructor($resource, $cookies) {
    this.resource = $resource;
    this.cookies = $cookies;
    this.dashboard = $resource(__API_ENDPOINT__ + `api/childcare/dashboard/:contactId`);
  }

  fetchChildcareDates() {
    const contactId = this.cookies.get('userId');
    return this.dashboard.get({contactId}, (data) => {
      this.childcareDates = data; 
    });
  }

  getChildcareDates() {
    return this.childcareDates;
  }
}

export default ChildcareDashboardService;
