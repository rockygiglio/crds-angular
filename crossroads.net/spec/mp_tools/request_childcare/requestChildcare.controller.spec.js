import constants from 'crds-constants';
import RequestChildcareService from '../../../app/mp_tools/request_childcare/requestChildcare.service';
import RequestChildcareController from '../../../app/mp_tools/request_childcare/requestChildcare.controller';

describe('Request Childcare Controller', () => {
  
  let rootScope,
      MPTools,
      CRDS_TOOLS_CONSTANTS,
      log,
      controller,
      lookupService,
      requestChildcareService;

  beforeEach(angular.mock.module(constants.MODULES.MPTOOLS));

  beforeEach(inject(function(_$rootScope_, $injector) {
    rootScope = _$rootScope_;
    MPTools = $injector.get('MPTools');
    CRDS_TOOLS_CONSTANTS = $injector.get('CRDS_TOOLS_CONSTANTS');
    log = $injector.get('$log');
  
    // TODO: figure out why I can't get this through the injector
    requestChildcareService = new RequestChildcareService();

    spyOn(requestChildcareService, 'getCongregations');
    spyOn(MPTools, 'getParams').and.returnValue({
      recordId: null 
    });
  }));

  it('should set allowAccess to false if not authorized', () => {
    spyOn(MPTools, 'allowAccess').and.returnValue(false); 
    commonExpectations();
    expect(controller.allowAccess).toBe(false);
  });
  
  it('should submit the form', () => {
    commonExpectations();    
    //TODO: Put submit() expectations here
  });

  function commonExpectations() {
    controller = new RequestChildcareController(rootScope,
                                                MPTools,
                                                CRDS_TOOLS_CONSTANTS,
                                                log,
                                                requestChildcareService);
    expect(requestChildcareService.getCongregations).toHaveBeenCalled();  
    expect(MPTools.getParams).toHaveBeenCalled();
  }

});
