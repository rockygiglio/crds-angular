import campsModule from '../../../../app/camps/application_page/application_page.module';
import campHelpers from '../../campHelpers';

describe('Camps Medical Info Form', () => {
  let fixture;
  let httpBackend;

  // eslint-disable-next-line no-underscore-dangle
  const endpoint = `${window.__env__.CRDS_API_ENDPOINT}api`;
  const contactId = 1234;
  const campId = 4321;

  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_MedicalInfoForm_, _$httpBackend_) => {
    fixture = _MedicalInfoForm_;
    httpBackend = _$httpBackend_;
  }));

  it('should save the medical info', () => {
    fixture.formModel = campHelpers.medicalInfoModel;
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
