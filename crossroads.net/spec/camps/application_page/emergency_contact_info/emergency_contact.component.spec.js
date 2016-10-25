import campsModule from '../../../../app/camps/application_page/application_page.module';
import campHelpers from '../../campHelpers';

describe('Camps Emergency Contact Component', () => {
  let $componentController;
  let emergencyContactController;
  // let log;
  let rootScope;
  let resource;
  let stateParams;
  let q;
  let emergencyContactForm;

  const campId = 123;
  const contactId = 456;
  const success = 'success';
  const error = 'error';

  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_$componentController_, _EmergencyContactForm_, _$log_, _$rootScope_, _$resource_, _$stateParams_, _$q_) => {
    $componentController = _$componentController_;
    // log = _$log_;
    emergencyContactForm = _EmergencyContactForm_;
    rootScope = _$rootScope_;
    resource = _$resource_;
    stateParams = _$stateParams_;
    q = _$q_;
    rootScope.MESSAGES = campHelpers.messages;

    stateParams.campId = campId;
    stateParams.contactId = contactId;

    spyOn(emergencyContactForm, 'getModel').and.callThrough();
    spyOn(emergencyContactForm, 'getFields').and.callThrough();
    spyOn(rootScope, '$emit').and.callThrough();

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

  it('should successfully save the form', () => {
    emergencyContactController.emergencyContact = { $valid: true };

    spyOn(emergencyContactForm, 'save').and.callFake(() => {
      const deferred = q.defer();
      deferred.resolve('success!');
      return deferred.promise;
    });

    emergencyContactController.submit();
    rootScope.$apply();
    expect(emergencyContactForm.save).toHaveBeenCalledWith(campId, contactId);
    expect(rootScope.$emit).toHaveBeenCalled();
    expect(rootScope.$emit).toHaveBeenCalledWith('notify', success);
  });

  it('should disable the submit button while saving', () => {
    expect(emergencyContactController.submitting).toBe(false);

    emergencyContactController.emergencyContact = { $valid: true };

    spyOn(emergencyContactForm, 'save').and.callFake(() => {
      const deferred = q.defer();
      deferred.resolve('success!');
      return deferred.promise;
    });

    emergencyContactController.submit();
    expect(emergencyContactController.submitting).toBe(true);

    rootScope.$apply();
    expect(emergencyContactController.submitting).toBe(false);
  });
});
