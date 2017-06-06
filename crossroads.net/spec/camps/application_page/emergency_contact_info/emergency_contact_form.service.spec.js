import cloneDeep from 'lodash/lang/cloneDeep';

import applicationmodule from '../../../../app/camps/application_page/application_page.module';
import campsModule from '../../../../app/camps/camps.module';

import campHelpers from '../../campHelpers';


describe('Camps Emergency Contact Form', () => {
  let fixture;
  let httpBackend;
  let campsService;

  // eslint-disable-next-line no-underscore-dangle
  const endpoint = `${window.__env__.CRDS_GATEWAY_CLIENT_ENDPOINT}api`;
  const contactId = 1234;
  const campId = 4321;

  beforeEach(angular.mock.module(campsModule));
  beforeEach(angular.mock.module(applicationmodule));

  beforeEach(inject((_EmergencyContactForm_, _CampsService_, _$httpBackend_) => {
    fixture = _EmergencyContactForm_.createForm();
    campsService = _CampsService_;
    httpBackend = _$httpBackend_;

    campsService.emergencyContacts = campHelpers().emergencyContacts;
  }));

  it('should save the emergency contact', () => {
    fixture.formModel = campHelpers().emergencyContactFormModel;

    httpBackend.expectPOST(`${endpoint}/v1.0.0/camps/${campId}/emergencycontact/${contactId}`, campHelpers.emergencyContactModel).respond(200);
    fixture.save(campId, contactId);
    httpBackend.flush();
  });

  describe('Prepopulate Model', () => {
    it('should prepopulate the model', () => {
      const expected = campHelpers().emergencyContactFormModel;
      expected.additionalContact = true;

      fixture.initFormModel(campHelpers().emergencyContacts);
      expect(fixture.formModel).toEqual(expected);
    });
  });

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });
});
