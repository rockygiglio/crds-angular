import constants from 'crds-constants';
import DashboardController from '../../../app/camps/dashboard/camps_dashboard.controller';
//import campsModule from '../../app/camps/camps.module';
//import campHelpers from '../campHelpers';

describe('Camp Dashboard Controller', () => {

  let dashboardController;

  beforeEach(() => {
    dashboardController = new DashboardController();
    dashboardController.$onInit();
  });

  it('should set the view state as ready', () => {
    expect(dashboardController.viewReady).toBeTruthy();
  });

});

describe('Camp Dashboard Component', () => {

  let dashboardComponent;

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  beforeEach(inject((_$componentController_) => {
    dashboardComponent = _$componentController_('campsDashboard', null, {});
    dashboardComponent.$onInit();
  }));



});
