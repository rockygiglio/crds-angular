import constants from 'crds-constants';
import ChildcareDashboardService from '../../app/childcare_dashboard/childcareDashboard.service';

/* jshint unused: false */
import childcareModule from '../../app/childcare_dashboard/childcareDashboard.module'

describe('Childcare Dashboard Service', () => {

  let childcareService;

  beforeEach(angular.mock.module(constants.MODULES.CHILDCARE_DASHBOARD));

  beforeEach(inject(function($injector) {
    console.log(ChildcareDashboardService);
    childcareService = new ChildcareDashboardService();
  }));

  it('should be true', () => {
    expect(true).toBe(true);
  });
});
