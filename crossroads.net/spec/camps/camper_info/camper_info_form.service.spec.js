import campsModule from '../../../app/camps/camps.module';
import campHelpers from '../campHelpers';

describe('Camper Info Form Service', () => {
  let camperInfoForm;
  let campsService;
  let httpBackend;

  // eslint-disable-next-line no-underscore-dangle
  const endpoint = window.__env__.CRDS_GATEWAY_CLIENT_ENDPOINT + 'api';

  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_LookupService_, _CamperInfoForm_, _CampsService_, _$httpBackend_) => {
    camperInfoForm = _CamperInfoForm_.createForm();
    campsService = _CampsService_;
    httpBackend = _$httpBackend_;
  }));

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });

  it('should get an array of data', () => {
    const fields = camperInfoForm.getFields();
    expect(fields).toBeDefined();
    expect(fields.length).toBeGreaterThan(0);
  });

  it('should save the form and return a promise', () => {
    const campId = 12345;
    // create some fake data from the form...
    camperInfoForm.formModel.firstName = 'Matthew';
    camperInfoForm.formModel.lastName = 'Silber';
    camperInfoForm.formModel.middleName = 'M';
    camperInfoForm.formModel.preferredName = 'Matt';

    httpBackend.expectPOST(`${endpoint}/camps/${campId}/campers`, camperInfoForm.formModel).respond(200);
    camperInfoForm.save(campId);
    httpBackend.flush();
  });

  it('should setup the form model', () => {
    expect(camperInfoForm.getModel()).toEqual(
     campHelpers().camperInfoModel
    );
  });

  it('should prepopulate shirt size', () => {
    campsService.camperInfo = campHelpers().camperInfoModelWithShirtSize;
    camperInfoForm.initFormModel();

    expect(camperInfoForm.formModel.shirtSize).toEqual(6846);
  });
});
