import constants from 'crds-constants';
import RequestChildcareService from '../../../app/mp_tools/request_childcare/requestChildcare.service';

describe('Request Childcare Service', () => {
  let requestChildcareService, lookupService, log, httpBackend;

  beforeEach(angular.mock.module(constants.MODULES.MPTOOLS));

  beforeEach(inject(function($injector) {
    lookupService = $injector.get('LookupService');
    log = $injector.get('$log');
    httpBackend = $injector.get('$httpBackend');
    requestChildcareService = new RequestChildcareService(log, lookupService);
  }));

  afterEach(() => {
    httpBackend.verifyNoOutstandingExpectation();
    httpBackend.verifyNoOutstandingRequest();
  });

  it('should get congregations for childcare', () => {
    httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] +
                          'api/lookup/childcarelocations').respond(200,[] );
    var request = requestChildcareService.getCongregations();
    httpBackend.flush();
  });
  
  it('should handle getCongregations error', () => {   
    httpBackend.expectGET(window.__env__['CRDS_API_ENDPOINT'] +
                          'api/lookup/childcarelocations').respond(500,[] );
    var request = requestChildcareService.getCongregations();
    httpBackend.flush();
  });

});
