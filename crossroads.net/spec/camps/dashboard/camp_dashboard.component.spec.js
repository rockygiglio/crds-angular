import DashboardController from '../../../app/camps/dashboard/camps_dashboard.controller';

import campsModule from '../../../app/camps/camps.module';
import campHelpers from '../campHelpers';

describe('Camp Dashboard Component', () => {

  let dashboardComponent,
      campsService;

  beforeEach(angular.mock.module(campsModule));

  describe('Dashboard data is empty', () => {
    beforeEach(inject((_$componentController_, _CampsService_) => {
      campsService = _CampsService_;
      campsService.dashboard = [];
      dashboardComponent = _$componentController_('campsDashboard', null, {});
      dashboardComponent.$onInit();
    }));

    it('should have the dashboard data defined', () => {
      expect(dashboardComponent.data).toBeDefined();
    });

    it('should set the view state as ready', () => {
      expect(dashboardComponent.viewReady).toBeTruthy();
    });

    it('should set the full name', () => {
      var fullName = dashboardComponent.fullName('silbernagel', 'matt');
      expect(fullName).toBe('matt silbernagel');
    });

    it('should display cms message when data is empty', () => {
      expect(dashboardComponent.isDashboardEmpty()).toBeTruthy();
    });
  });

  describe('Dashboard data is not empty', () => {
    beforeEach(inject((_$componentController_, _CampsService_) => {
      campsService = _CampsService_;
      campsService.dashboard = [{ 'key', 'value' }];
      dashboardComponent = _$componentController_('campsDashboard', null, {});
      dashboardComponent.$onInit();
    }));

    it('should display cms message when data is empty', () => {
      expect(dashboardComponent.isDashboardEmpty()).toBeFalsy();
    });
  });
});
