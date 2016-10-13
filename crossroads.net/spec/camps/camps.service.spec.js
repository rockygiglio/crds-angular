import constants from 'crds-constants';

/* jshint unused: false */
import campsModule from '../../app/camps/camps.module';

describe('Camp Service', () => {
  const endpoint = window.__env__['CRDS_API_ENDPOINT'] + 'api';
  let campsService;
  let httpBackend;

  beforeEach(angular.mock.module(constants.MODULES.CAMPS));

  beforeEach(inject((_CampsService_, _$httpBackend_) => {
    campsService = _CampsService_;
    httpBackend = _$httpBackend_;
  }));

  it('Should make the API call to Camp Service', () => {
    let eventId = 4525285;
    httpBackend.expectGET(`${endpoint}/camps/${eventId}`)
      .respond(200, {});
    campsService.getCampInfo(eventId);

    httpBackend.flush();
  });

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });
});
