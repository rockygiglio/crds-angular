import constants from 'crds-constants';
import ChildcareDashboardService from '../../../app/childcare_dashboard/childcareDashboard.service';
import ChildcareGroupController from '../../../app/childcare_dashboard/childcare_group/childcareGroup.controller';

describe('Childcare Group Component Controller', () => {

  let rootScope,
      log,
      controller,
      childcareDashboardService;

  beforeEach(angular.mock.module(constants.MODULES.CHILDCARE_DASHBOARD));

  beforeEach(inject(function(_$rootScope_, $injector) {
   rootScope = _$rootScope_;
   childcareDashboardService = new ChildcareDashboardService();
   log = $injector.get('$log');
  }));

  it('should create the childcareDashboard controller', () => {
    commonExpectations();
  });

  function commonExpectations() {
    controller = new ChildcareGroupController(rootScope, childcareDashboardService);
    //expect(cookies.get).toHaveBeenCalledWith('userId');
    //expect(MPTools.getParams).toHaveBeenCalled();
  }


});

