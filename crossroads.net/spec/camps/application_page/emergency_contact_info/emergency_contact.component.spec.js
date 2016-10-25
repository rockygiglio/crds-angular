import campsModule from '../../../../app/camps/application_page/application_page.module';
import campHelpers from '../../campHelpers';

describe('Camps Emergency Contact Component', () => {
  let $componentController;
  let emergencyContactController;
  // let log;
  let rootScope;
  let resource;
  let emergencyContactForm;

  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_$componentController_, _EmergencyContactForm_, _$log_, _$rootScope_, _$resource_) => {
    $componentController = _$componentController_;
    // log = _$log_;
    emergencyContactForm = _EmergencyContactForm_;
    rootScope = _$rootScope_;
    resource = _$resource_;
    rootScope.MESSAGES = campHelpers.messages;

    spyOn(emergencyContactForm, 'getModel').and.callThrough();
    spyOn(emergencyContactForm, 'getFields').and.callThrough();

    const bindings = {};
    emergencyContactController = $componentController('emergencyContact', null, bindings);
    emergencyContactController.$onInit();
  }));

  it('should set the view as ready', () => {
    expect(emergencyContactController.viewReady).toBeTruthy();
  });

  it('should get the model', () => {
    expect(emergencyContactForm.getModel).toHaveBeenCalled();
    expect(emergencyContactController.model).toBeDefined();
  });

  it('should get the fields', () => {
    expect(emergencyContactForm.getFields).toHaveBeenCalled();
    expect(emergencyContactController.fields).toBeDefined();
    expect(emergencyContactController.fields.length).toBeGreaterThan(0);
  });
});
