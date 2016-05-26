import constants from 'crds-constants';
import RequestChildcareService from '../../../app/mp_tools/request_childcare/requestChildcare.service';
import RequestChildcareController from '../../../app/mp_tools/request_childcare/requestChildcare.controller';
import moment from 'moment';

describe('Request Childcare Controller', () => {
  
  let rootScope,
      MPTools,
      CRDS_TOOLS_CONSTANTS,
      log,
      controller,
      lookupService,
      requestChildcareService,
      validation,
      cookies,
      _window;

  const uid = 123456789;

  beforeEach(angular.mock.module(constants.MODULES.MPTOOLS));

  beforeEach(inject(function(_$rootScope_, $injector) {
    rootScope = _$rootScope_;
    MPTools = $injector.get('MPTools');
    CRDS_TOOLS_CONSTANTS = $injector.get('CRDS_TOOLS_CONSTANTS');
    log = $injector.get('$log');
    validation = $injector.get('Validation'); 
    cookies = $injector.get('$cookies');
    _window = $injector.get('$window');

    // TODO: figure out why I can't get this through the injector
    requestChildcareService = new RequestChildcareService();

    // set up spys
    spyOn(requestChildcareService, 'getCongregations');
    spyOn(requestChildcareService, 'getMinistries');
    spyOn(requestChildcareService, 'saveRequest').and.returnValue(
      { $promise: 
        { then: function(fn) {
          return [];
        }
      }
    });
    spyOn(requestChildcareService, 'getGroups').and.returnValue(
      // return a fake implementation of a promise
      {$promise: 
        { then: function(fn) { 
            return [];
          }
        }
      });
    spyOn(requestChildcareService, 'getPreferredTimes');
    spyOn(cookies, 'get').and.returnValue(uid);
    spyOn(MPTools, 'getParams').and.returnValue({
      recordId: null 
    });

  }));

  it('should set allowAccess to false if not authorized', () => {
    spyOn(MPTools, 'allowAccess').and.returnValue(false); 
    commonExpectations();
    expect(controller.allowAccess).toBe(false);
  });
 
  it('should get groups and preferred times', () => {
    commonExpectations();
    controller.choosenCongregation = { dp_RecordID: 2 };
    controller.choosenMinistry = {dp_RecordID: 3};
    controller.getGroups();
    expect(requestChildcareService.getGroups).toHaveBeenCalledWith(2, 3);
    expect(requestChildcareService.getPreferredTimes).toHaveBeenCalledWith(2);
  });

  it('should show groups', () => {
    commonExpectations();
    controller.choosenCongregation = { dp_RecordID: 2 };
    controller.choosenMinistry = { dp_RecordID: 3 };
    controller.groups = [
      {groupName: 'groupName'}
    ];
    expect(controller.showGroups()).toBe(true);
  });

  it('should not show groups if ministry and congregation are not set', () => {
    commonExpectations();
    expect(controller.showGroups()).toBeFalsy(); 
  });

  it('should not show groups if the groups list is empty', () => {
    controller.choosenCongregation = { dp_RecordID: 2 };
    controller.choosenMinistry = { dp_RecordID: 3 };
    controller.groups = [];
    expect(controller.showGroups()).toBe(false);
  });

  it('should format the preferred time correctly', () => {
    commonExpectations();
    const time = {
      'Childcare_Start_Time': '09:00:00',
      'Childcare_End_Time': '19:00:00',
      'Meeting_Day': 'Monday'
    };
    expect(controller.formatPreferredTime(time)).toBe('Monday, 9:00AM - 7:00PM');
  });

  it('should submit the form', () => {
    commonExpectations();    
    // fake form 
    controller.childcareRequestForm = {
      $invalid: false  
    };
    const now = new Date(); 

    controller.choosenCongregation = { dp_RecordID: 1 };
    controller.choosenMinistry = { dp_RecordID: 2 };
    controller.choosenGroup = { dp_RecordID: 3 };
    controller.startDate = now;
    controller.endDate = now;
    controller.choosenFrequency = 'once';
    controller.choosenPreferredTime = { Childcare_Start_Time: '9:00:00' , Childcare_End_Time: '10:00:00' }; 
    controller.numberOfChildren = 12;
    controller.notes = 'some long note';

    const expectedDto = {
      requester: uid,
      site: 1,
      ministry: 2,
      group: 3,
      startDate: moment(now).utc(),
      endDate: moment(now).utc(),
      frequency: 'once',
      timeframe: 4,
      estimatedChildren: 12,
      notes: 'some long note'
    };
 
    controller.submit();
    expect(requestChildcareService.saveRequest).toHaveBeenCalled();
  });

  it('should not submit the form if $invalid', () => {
    commonExpectations();
    // fake form 
    controller.childcareRequestForm = {
      $invalid: true  
    };
    expect(controller.submit()).toBe(false);
  });

  function commonExpectations() {
    controller = new RequestChildcareController(rootScope,
                                                MPTools,
                                                CRDS_TOOLS_CONSTANTS,
                                                log,
                                                requestChildcareService,
                                                validation,
                                                cookies,
                                                _window);
    expect(requestChildcareService.getCongregations).toHaveBeenCalled();  
    expect(requestChildcareService.getMinistries).toHaveBeenCalled();
    expect(cookies.get).toHaveBeenCalledWith('userId');
    expect(MPTools.getParams).toHaveBeenCalled();
  }

});
