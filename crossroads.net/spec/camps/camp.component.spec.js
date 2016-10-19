import constants from '../../app/constants';

// eslint-disable-next-line no-unused-vars
import campsModule from '../../app/camps/camps.module';
import campHelpers from './campHelpers';

describe('Camp Component', () => {
  let $componentController;
  let campController;
  let campsService;

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  describe('Camp Component with Summer Camp', () => {
    beforeEach(inject((_$componentController_, _$httpBackend_, _CampsService_) => {
      $componentController = _$componentController_;
      campsService = _CampsService_;
      const bindings = {
        isSummerCamp: true
      };
      campsService.campInfo = campHelpers().campInfo;
      campController = $componentController('crossroadsCamp', null, bindings);
      campController.$onInit();
    }));

    it('should set the summer camp flag to true', () => {
      expect(campController.isSummerCamp).toBeTruthy();
    });

    it('should set the summer camp title correctly', () => {
      expect(campController.campTitle).toBe('Summer Camp');
    });
  });

  describe('Camp Component without Summer Camp', () => {
    beforeEach(inject((_$componentController_, _$httpBackend_, _CampsService_) => {
      $componentController = _$componentController_;
      campsService = _CampsService_;
      const bindings = {};
      campsService.campInfo = campHelpers().campInfo;
      campController = $componentController('crossroadsCamp', null, bindings);
      campController.$onInit();
    }));

    it('should set the view as ready', () => {
      expect(campController.viewReady).toBe(true);
    });

    it('should not set the summer camp flag to true', () => {
      expect(campController.isSummerCamp).toBeFalsy();
    });

    it('should set the title correctly', () => {
      expect(campController.campTitle).toBe(campHelpers().campInfo.eventTitle);
    });
  });
});
