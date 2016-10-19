import campsModule from '../../../app/camps/camps.module';
import campHelpers from '../campHelpers';

describe('Camps Family Select Tool', () => {
  let $componentController;
  let familySelectController;
  let campsService;
  let log;
  let rootScope;

  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_$componentController_, _CampsService_, _$log_, _$rootScope_) => {
    $componentController = _$componentController_;
    campsService = _CampsService_;
    campsService.family = campHelpers.family; // TODO: create the family helper
    campsService.campTitle = 'Summer Camp';
    log = _$log_;
    rootScope = _$rootScope_;

    spyOn(log, 'debug').and.callThrough();

    rootScope.MESSAGES = {
      summercampIntro: {
        content: 'summer camp intro text'
      }
    };

    const bindings = { };
    familySelectController = $componentController('campsFamily', null, bindings);
    familySelectController.$onInit();
  }));

  it('should initialize the component', () => {
    expect(log.debug).toHaveBeenCalled();
  });

  it('should create the correct identifier for the camp intro text', () => {
    expect(familySelectController.buildCMSIdentifier()).toBe('summercampIntro');
    expect(familySelectController.campsService.campTitle).toBe('Summer Camp');
  });

  it('should retrieve the correct CMS message', () => {
    expect(familySelectController.cmsMessage).toBe('summer camp intro text');
  });
});
