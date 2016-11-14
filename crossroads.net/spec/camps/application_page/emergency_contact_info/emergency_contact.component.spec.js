import applicationModule from '../../../../app/camps/application_page/application_page.module';
import campsModule from '../../../../app/camps/camps.module';
import campHelpers from '../../campHelpers';

describe('Camps Emergency Contact Component', () => {
  let $componentController;
  let emergencyContactController;
  let campsService;
  // let log;
  let rootScope;
  let stateParams;
  let q;
  let emergencyContactForm;

  const campId = 123;
  const contactId = 456;

  beforeEach(angular.mock.module(campsModule));
  beforeEach(angular.mock.module(applicationModule));

  beforeEach(inject((_$componentController_, _EmergencyContactForm_, _CampsService_, _$log_, _$rootScope_, _$stateParams_, _$q_) => {
    $componentController = _$componentController_;
    // log = _$log_;
    campsService = _CampsService_;
    stateParams = _$stateParams_;
    rootScope = _$rootScope_;
    q = _$q_;

    stateParams.campId = campId;
    stateParams.contactId = contactId;

    // Set up the form instance
    emergencyContactForm = _EmergencyContactForm_.createForm();
    spyOn(_EmergencyContactForm_, 'createForm').and.callFake(() => emergencyContactForm);

    spyOn(emergencyContactForm, 'getModel').and.callThrough();
    spyOn(emergencyContactForm, 'getFields').and.callThrough();

    // Set up fake deferred formModel for the Form.load() call
    spyOn(emergencyContactForm, 'load').and.callFake(() => {
      const deferred = q.defer();
      deferred.resolve(campHelpers().emergencyContactFormModel);
      return deferred.promise;
    });

    campsService.emergencyContacts = campHelpers().emergencyContactModel;

    const bindings = {};
    emergencyContactController = $componentController('emergencyContact', null, bindings);
    emergencyContactController.$onInit();
  }));

  it('should set the view as ready', () => {
    rootScope.$apply();

    expect(emergencyContactController.viewReady).toBeTruthy();
  });

  it('should get the model', () => {
    rootScope.$apply();

    expect(emergencyContactController.model).toEqual(campHelpers().emergencyContactFormModel);
  });

  it('should get the fields', () => {
    rootScope.$apply();

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
  });

  it('should reject saving the form', () => {
    emergencyContactController.emergencyContact = { $valid: true };

    spyOn(emergencyContactForm, 'save').and.callFake(() => {
      const deferred = q.defer();
      deferred.reject('error!');
      return deferred.promise;
    });

    emergencyContactController.submit();
    rootScope.$apply();
    expect(emergencyContactForm.save).toHaveBeenCalledWith(campId, contactId);
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
