import applicationModule from '../../../../app/camps/application_page/application_page.module';
import campsModule from '../../../../app/camps/camps.module';

describe('Camp Product Summary Form', () => {
  let fixture;
  let httpBackend;

  // eslint-disable-next-line no-underscore-dangle
  const endpoint = `${window.__env__.CRDS_API_ENDPOINT}api`;
  const eventId = 1234;
  const contactId = 4567;

  beforeEach(angular.mock.module(campsModule));
  beforeEach(angular.mock.module(applicationModule));

  beforeEach(inject((_ProductSummaryForm_, _$httpBackend_) => {
    fixture = _ProductSummaryForm_;
    httpBackend = _$httpBackend_;
  }));

  it('should save the product summary page', () => {
    fixture.formModel = {};

    const expected = {
      eventId,
      contactId,
      financialAssistance: false
    };

    fixture.formModel = expected;

    httpBackend.expectPOST(`${endpoint}/camps/product`, expected).respond(200);
    fixture.save(eventId, contactId);
    httpBackend.flush();
  });

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });
});
