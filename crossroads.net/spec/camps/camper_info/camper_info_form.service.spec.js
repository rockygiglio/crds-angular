import constants from 'crds-constants';

/* jshint unused: false */
import campsModule from '../../../app/camps/camps.module';
import campHelpers from '../campHelpers';

describe('Camper Info Form Service', () => {
  let camperInfoForm,
      lookupService,
      httpBackend;

  const endpoint = window.__env__['CRDS_API_ENDPOINT'] + 'api';

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  beforeEach(inject((_LookupService_, _CamperInfoForm_, _$httpBackend_) => {
    camperInfoForm = _CamperInfoForm_;
    lookupService = _LookupService_;
    httpBackend = _$httpBackend_;
  }));

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });

  it('should get an array of data', () => {
    var fields = camperInfoForm.getFields();
    expect(fields).toBeDefined();
    expect(fields.length).toBeGreaterThan(0);
  });

  it('should save the form and return a promise', () => {
    var campId = 12345;
    // create some fake data from the form...
    camperInfoForm.formModel.firstName = 'Matthew';
    camperInfoForm.formModel.lastName = 'Silber';
    camperInfoForm.formModel.middleName = 'M';
    camperInfoForm.formModel.prefferedName = 'Matt';

    httpBackend.expectPOST(`${endpoint}/camps/${campId}`, camperInfoForm.formModel).respond(200);
    camperInfoForm.save(campId);
    httpBackend.flush();
  });

  it('should setup the form model', () => {
    expect(camperInfoForm.getModel()).toEqual(
     campHelpers().camperInfoModel
    );
  });

});
