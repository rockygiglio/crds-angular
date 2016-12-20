import campsModule from '../../../app/camps/camps.module';

describe('Camp Dashboard Component', () => {
  let dashboardComponent;
  let campsService;
  let state;

  beforeEach(angular.mock.module(campsModule));

  describe('Dashboard data is empty', () => {
    beforeEach(inject((_$componentController_, _CampsService_, _$state_) => {
      campsService = _CampsService_;
      campsService.dashboard = [];
      state = _$state_;
      state.toParams = {
        paymentId: 34
      };

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
      const fullName = dashboardComponent.fullName('silbernagel', 'matt');
      expect(fullName).toBe('matt silbernagel');
    });

    it('should display cms message when data is empty', () => {
      expect(dashboardComponent.isDashboardEmpty()).toBeTruthy();
    });
  });

  describe('Dashboard data is not empty', () => {
    beforeEach(inject((_$componentController_, _CampsService_, _$state_) => {
      campsService = _CampsService_;
      campsService.dashboard = [
        { key: 'value' }
      ];
      state = _$state_;
      state.toParams = {
        paymentId: 34
      };
      dashboardComponent = _$componentController_('campsDashboard', null, {});
      dashboardComponent.$onInit();
    }));

    it('should display cms message when data is empty', () => {
      expect(dashboardComponent.isDashboardEmpty()).toBeFalsy();
    });
  });
});
