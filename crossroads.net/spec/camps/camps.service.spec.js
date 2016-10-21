import constants from '../../app/constants';

import campsModule from '../../app/camps/camps.module';

describe('Camp Service', () => {
  /* eslint-disable-next-line no-underscore-dangle*/
  const endpoint = `${window.__env__.CRDS_API_ENDPOINT}api`;
  let campsService;
  let httpBackend;

  beforeEach(angular.mock.module(campsModule));

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

  it('should make the API call to get my camp family', () => {
    const campId = 21312;
    expect(campsService.family).toBeUndefined();
    httpBackend.expectGET(`${endpoint}/camps/${campId}/family`).respond(200, []);
    campsService.getCampFamily(campId);
    httpBackend.flush();
    expect(campsService.family).toBeDefined();
  });

  it('should make the API call to get my camp family and handle error', () => {
    const campId = 21312;
    expect(campsService.family).toBeUndefined();
    httpBackend.expectGET(`${endpoint}/camps/${campId}/family`).respond(500, []);
    campsService.getCampFamily(campId);
    httpBackend.flush();
    expect(campsService.family).toBeUndefined();
  });

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });
});
