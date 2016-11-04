import campsModule from '../../../../app/camps/application_page/application_page.module';
import campHelpers from '../../campHelpers';

describe('Camps Emergency Contact Form', () => {
  let fixture;
  let httpBackend;

  // eslint-disable-next-line no-underscore-dangle
  const endpoint = `${window.__env__.CRDS_API_ENDPOINT}api`;
  const contactId = 1234;
  const campId = 4321;

  beforeEach(angular.mock.module(campsModule));

  beforeEach(inject((_EmergencyContactForm_, _$httpBackend_) => {
    fixture = _EmergencyContactForm_;
    httpBackend = _$httpBackend_;
  }));

  it('should save the emergency contact', () => {
    fixture.formModel = campHelpers().emergencyContactModel;

    httpBackend.expectPOST(`${endpoint}/camps/${campId}/emergencycontact/${contactId}`, campHelpers.emergencyContactModel).respond(200);
    fixture.save(campId, contactId);
    httpBackend.flush();
  });

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });
});
