import applicationModule from '../../../../app/camps/application_page/application_page.module';
import campsModule from '../../../../app/camps/camps.module';


fdescribe('Camps Medical Info Component', () => {
  let $componentController;
  let medicalInfoController;
  // let log;
  let rootScope;
  let stateParams;
  let q;
  let medicalInfoForm;

  const campId = 123;
  const contactId = 456;

  beforeEach(angular.mock.module(campsModule));
  beforeEach(angular.mock.module(applicationModule));

  beforeEach(inject((_$componentController_, _MedicalInfoForm_, _$log_, _$rootScope_, _$state_, _$q_) => {
    $componentController = _$componentController_;
    // log = _$log_;
    stateParams = _$state_.params;
    rootScope = _$rootScope_;
    q = _$q_;

    stateParams.campId = campId;
    stateParams.contactId = contactId;

    // Set up the form
    medicalInfoForm = _MedicalInfoForm_.createForm();
    spyOn(_MedicalInfoForm_, 'createForm').and.callFake(() => medicalInfoForm);

    spyOn(medicalInfoForm, 'getModel').and.callThrough();
    spyOn(medicalInfoForm, 'getFields').and.callThrough();
  }));

  describe('Update flag set to undefined', () => {
    beforeEach(() => {
      stateParams.update = undefined;
      const bindings = {};
      medicalInfoController = $componentController('campMedicalInfo', null, bindings);
      medicalInfoController.$onInit();
    });

    it('should set the update flag to true if it is initialized to undefined', () => {
      expect(medicalInfoController.update).toBeTruthy();
    });
  });

  describe('Update flag set to true', () => {
    beforeEach(() => {
      stateParams.update = true;
      const bindings = {};
      medicalInfoController = $componentController('campMedicalInfo', null, bindings);
      medicalInfoController.$onInit();
    });

    it('should set the view as ready', () => {
      expect(medicalInfoController.viewReady).toBeTruthy();
    });

    it('should get the model', () => {
      expect(medicalInfoForm.getModel).toHaveBeenCalled();
      expect(medicalInfoController.model).toBeDefined();
    });

    it('should get the fields', () => {
      expect(medicalInfoForm.getFields).toHaveBeenCalled();
      expect(medicalInfoController.fields).toBeDefined();
      expect(medicalInfoController.fields.length).toBeGreaterThan(0);
    });

    it('should successfully save the form', () => {
      medicalInfoController.medicalInfo = { $valid: true };

      spyOn(medicalInfoForm, 'save').and.callFake(() => {
        const deferred = q.defer();
        deferred.resolve('success!');
        return deferred.promise;
      });

      medicalInfoController.submit();
      rootScope.$apply();
      expect(medicalInfoForm.save).toHaveBeenCalledWith(contactId);
    });

    it('should reject saving the form', () => {
      medicalInfoController.medicalInfo = { $valid: true };

      spyOn(medicalInfoForm, 'save').and.callFake(() => {
        const deferred = q.defer();
        deferred.reject('error!');
        return deferred.promise;
      });

      medicalInfoController.submit();
      rootScope.$apply();
      expect(medicalInfoForm.save).toHaveBeenCalledWith(contactId);
    });

    it('should disable the submit button while saving', () => {
      expect(medicalInfoController.submitting).toBe(false);

      medicalInfoController.medicalInfo = { $valid: true };

      spyOn(medicalInfoForm, 'save').and.callFake(() => {
        const deferred = q.defer();
        deferred.resolve('success!');
        return deferred.promise;
      });

      medicalInfoController.submit();
      expect(medicalInfoController.submitting).toBe(true);

      rootScope.$apply();
      expect(medicalInfoController.submitting).toBe(false);
    });
  });
});
