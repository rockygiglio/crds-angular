import constants from 'crds-constants';

/* jshint unused: false */
import campsModule from '../../app/camps/camps.module';

describe('Camp Service', () => {
  /* eslint-disable-next-line */
  const endpoint = `${window.__env__.CRDS_API_ENDPOINT}api`;
  let campsService;
  let httpBackend;

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  beforeEach(inject((_CampsService_, _$httpBackend_) => {
    campsService = _CampsService_;
    httpBackend = _$httpBackend_;
  }));

  it('Should make the API call to Camp Service', () => {
    const eventId = 4525285;
    httpBackend.expectGET(`${endpoint}/camps/${eventId}`)
      .respond(200, {});
    campsService.getCampInfo(eventId);

    httpBackend.flush();
  });

  it('should make the API call to get my dashboard', () => {
    httpBackend.expectGET(`${endpoint}/my-camp`).respond(200, []);
    campsService.getCampDashboard();
    httpBackend.flush();
  });

  it('should make the API call to get my dashboard and handle error', () => {
    httpBackend.expectGET(`${endpoint}/my-camp`).respond(500, []);
    campsService.getCampDashboard();
    httpBackend.flush();
  });

  it('should make the API call to get my summer camp family', () => {

  });

  it('should make the API call to get my summer camp family and handle error', () => {

  });

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });
});
