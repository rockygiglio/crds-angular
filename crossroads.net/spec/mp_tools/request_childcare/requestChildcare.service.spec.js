import constants from 'crds-constants';
import RequestChildcareService from '../../../app/mp_tools/request_childcare/requestChildcare.service';

describe('Request Childcare Service', () => {
  let requestChildcareService, lookupService, log, httpBackend;

  beforeEach(angular.mock.module(constants.MODULES.MPTOOLS));

  beforeEach(inject(function($injector) {
    lookupService = $injector.get('LookupService');
    log = $injector.get('$log');
    requestChildcareService = new RequestChildcareService(log, lookupService);
  }));

  it('should get congregations for childcare', () => {
    
  });


});
