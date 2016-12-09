import constants from 'crds-constants';

/* jshint unused: false */
import campsModule from '../../../app/camps/camps.module';

describe('Camper Info Component', () => {
  let $componentController,
      $httpBackend,
      camperInfo,
      camperInfoForm,
      q,
      stateParams,
      rootScope,
      state;

  const endpoint = window.__env__['CRDS_API_ENDPOINT'] + 'api';
  const eventId = 12323;

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  beforeEach(inject((_$state_, _$componentController_, _$httpBackend_, _CamperInfoForm_, _$q_, _$stateParams_, _$rootScope_) => {
    $componentController = _$componentController_;
    $httpBackend = _$httpBackend_;
    state = _$state_;
    q = _$q_;
    stateParams = _$stateParams_;
    rootScope = _$rootScope_.$new();
    rootScope.MESSAGES = {
      successfullRegistration: { content: 'success' },
      generalError: { content: 'error' }
    };

    // Set up the camper info form instance that will be returned by the CamperInfoForm.createForm() factory function
    camperInfoForm = _CamperInfoForm_.createForm();
    spyOn(_CamperInfoForm_, 'createForm').and.callFake(() => camperInfoForm);

    spyOn(camperInfoForm, 'getFields').and.callThrough();
    spyOn(camperInfoForm, 'getModel').and.callThrough();
    spyOn(rootScope, '$emit').and.callThrough();

    state.toParams = {
      campId: eventId
    };

    stateParams.campId = eventId;

    const bindings = {};
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

  it('should successfully save the form', () => {
    camperInfo.infoForm = { $valid: true };

    spyOn(camperInfoForm, 'save').and.callFake((data) => {
      console.log('called faked with', data);
      const deferred = q.defer();
      deferred.resolve('success!');
      return deferred.promise;
    });

    camperInfo.submit();
    rootScope.$apply(); // must be called to resolve the promise
    expect(camperInfoForm.save).toHaveBeenCalledWith(eventId);
  });

  it('should set the button as disabled when submitting the form', () => {
    expect(camperInfo.submitting).toBeFalsy();

    // allow for the submit fuctionality to called
    camperInfo.infoForm = { $valid: true };

    spyOn(camperInfoForm, 'save').and.callFake((data) => {
      console.log('called faked with', data);
      const deferred = q.defer();
      deferred.resolve('success!');
      return deferred.promise;
    });
    camperInfo.submit();
    expect(camperInfo.submitting).toBeTruthy();
    rootScope.$apply(); // must be called to resolve the promise
    expect(camperInfo.submitting).toBeFalsy();
  });

  it('should get new data when selecting another family member', () => {
    // FIXME: do we need to implement this test case?
    //   Jonathan Horner
  });
});
