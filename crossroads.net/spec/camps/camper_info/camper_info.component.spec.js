import constants from 'crds-constants';

/* jshint unused: false */
import campsModule from '../../../app/camps/camps.module';

describe('Camper Info Component', () => {
  let $componentController,
      $httpBackend,
      camperInfo,
      camperInfoForm;

  const endpoint = window.__env__['CRDS_API_ENDPOINT'] + 'api';

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  beforeEach(inject((_$componentController_, _$httpBackend_, _CamperInfoForm_) => {
    $componentController = _$componentController_;
    camperInfoForm = _CamperInfoForm_;
    $httpBackend = _$httpBackend_;

    spyOn(camperInfoForm, 'getFields').and.callThrough();
    spyOn(camperInfoForm, 'getModel').and.callThrough();

    var bindings = {};
    camperInfo = $componentController('camperInfo', null, bindings);
    camperInfo.$onInit();
  }));

  it('should set the view as ready', () => {
    expect(camperInfo.viewReady).toBe(true);
  });

  it('should get the field list', () => {
    expect(camperInfoForm.getFields).toHaveBeenCalled();
    expect(camperInfo.fields).toBeDefined();
    expect(camperInfo.fields.length).toBeGreaterThan(0);
  });

  it('should get the model', () => {
    expect(camperInfoForm.getModel).toHaveBeenCalled();
    expect(camperInfo.model).toBeDefined();
  });

  it('should validate and submit the form to the service', () => {

  });
});
