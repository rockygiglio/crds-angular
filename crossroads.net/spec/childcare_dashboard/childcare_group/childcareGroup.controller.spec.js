import constants from 'crds-constants';
import ChildcareDashboardService from '../../../app/childcare_dashboard/childcareDashboard.service';
import ChildcareDashboardGroupController from '../../../app/childcare_dashboard/childcare_group/childcareDashboardGroup.controller';

import childcareModule from '../../../app/childcare_dashboard/childcareDashboard.module'

describe('Childcare Group Component Controller', () => {

  let rootScope,
      log,
      childcareDashboardService;

  beforeEach(angular.mock.module(constants.MODULES.CHILDCARE_DASHBOARD));

  beforeEach(inject(function(_$rootScope_, $injector) {
    rootScope = _$rootScope_;
    log = $injector.get('$log');
    childcareDashboardService = new ChildcareDashboardService();
    let controller = new ChildcareDashboardGroupController('hello');
  }));

  it('should create the childcare group controller', () => {
    expect(true).toBe(true);
  });

});

