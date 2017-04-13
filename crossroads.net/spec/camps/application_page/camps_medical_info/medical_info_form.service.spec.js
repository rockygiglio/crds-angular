import applicationModule from '../../../../app/camps/application_page/application_page.module';
import campsModule from '../../../../app/camps/camps.module';
import campHelpers from '../../campHelpers';

describe('Camps Medical Info Form', () => {
  let fixture;
  let httpBackend;

  // eslint-disable-next-line no-underscore-dangle
  const endpoint = `${window.__env__.CRDS_GATEWAY_CLIENT_ENDPOINT}api`;
  const contactId = 456;

  beforeEach(angular.mock.module(applicationModule));
  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_MedicalInfoForm_, _$httpBackend_) => {
    fixture = _MedicalInfoForm_.createForm();
    httpBackend = _$httpBackend_;
  }));

  it('should save the medical info', () => {
    httpBackend.expectPOST(`${endpoint}/camps/medical/${contactId}`, campHelpers.medicalInfoModel).respond(200);
    fixture.save(contactId);
    httpBackend.flush();
  });

  it('should get fields', () => {
    const fields = fixture.getFields();
    expect(fields).toBeDefined();
    expect(fields.length).toBeGreaterThan(0);
  });

  it('should set up the form model', () => {
    expect(fixture.getModel()).toEqual(campHelpers().medicalInfoModel);
  });

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });
});
