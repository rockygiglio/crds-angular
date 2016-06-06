import constants from 'crds-constants';
import RequestChildcareService from '../../../app/mp_tools/request_childcare/requestChildcare.service';
import RequestChildcareController from '../../../app/mp_tools/request_childcare/requestChildcare.controller';

describe('Request Childcare Controller', () => {

  let rootScope,
      MPTools,
      CRDS_TOOLS_CONSTANTS,
      log,
      controller,
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
        { then: function() {
          return [];
        }
      }
    });
    spyOn(requestChildcareService, 'getPreferredTimes').and.returnValue(
      {
        $promise: {
          then: function() {
            return [];
          }
        }
      });
    spyOn(requestChildcareService, 'getGroups').and.returnValue(
      // return a fake implementation of a promise
      {$promise: 
        { then: function() { 
            return [];
          }
        }
      });
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

  it('should show the gaps in frequency widget', () => {
    commonExpectations();
    controller.choosenPreferredTime = { 
      Childcare_Day_ID: 3,
      Childcare_End_Time: '19:00:00',
      Childcare_Preferred_Time_ID: 1,
      Childcare_Start_Time: '18:00:00',
      Congregation_ID: 1,
      Deactivate_Date: null,
      Meeting_Day: 'Tuesday',
      dp_FileID: null,
      dp_RecordID: 1,
      dp_RecordName: 1,
      dp_RecordStatus: 0,
      dp_Selected: 0
    };
    controller.choosenFrequency = 'Weekly';
    controller.startDate = new Date(2016, 5, 1);
    controller.endDate = new Date(2016, 10, 2);
    expect(controller.showGaps()).toBe(true);
  });

  it('should not show the gaps in frequency widget', () => {
    commonExpectations();
    expect(controller.showGaps()).toBe(false);
  });

  it('should get a list of dates for a weekly occurence', () => {
    commonExpectations();
    controller.choosenPreferredTime = {
      Childcare_Day_ID: 3,
      Childcare_End_Time: '19:00:00',
      Childcare_Preferred_Time_ID: 1,
      Childcare_Start_Time: '18:00:00',
      Congregation_ID: 1,
      Deactivate_Date: null,
      Meeting_Day: 'Tuesday',
      dp_FileID: null,
      dp_RecordID: 1,
      dp_RecordName: 1,
      dp_RecordStatus: 0,
      dp_Selected: 0
    };
    controller.choosenFrequency = 'Weekly';
    controller.startDate = new Date(2016, 5, 1);
    controller.endDate = new Date(2016, 7, 2);
    var expectedDateList = [
      moment(new Date(2016, 5, 7)),
      moment(new Date(2016, 5, 14)),
      moment(new Date(2016, 5, 21)),
      moment(new Date(2016, 5, 28)),
      moment(new Date(2016, 6, 5)),
      moment(new Date(2016, 6, 12)),
      moment(new Date(2016, 6, 19)),
      moment(new Date(2016,6, 26)),
      moment(new Date(2016,7,2))
    ];
    var size = expectedDateList.length;

    controller.generateDateList();
    expect(controller.datesList.length).toBe(size);
    for(var i = 0; i<size; i++) {
      expect(controller.datesList[i].date.date()).toEqual(expectedDateList[i].date());
    }
  });

  it('should get a list of dates for a montly occurence', () => {
    commonExpectations();
    controller.choosenPreferredTime = {
      Childcare_Day_ID: 3,
      Childcare_End_Time: '19:00:00',
      Childcare_Preferred_Time_ID: 1,
      Childcare_Start_Time: '18:00:00',
      Congregation_ID: 1,
      Deactivate_Date: null,
      Meeting_Day: 'Tuesday',
      dp_FileID: null,
      dp_RecordID: 1,
      dp_RecordName: 1,
      dp_RecordStatus: 0,
      dp_Selected: 0
    };
    controller.choosenFrequency = 'Monthly';

    controller.startDate = new Date(2016, 5, 1);
    controller.endDate = new Date(2016, 11, 1);

    controller.generateDateList();
    var expectedDateList = [
      moment(new Date(2016, 5, 7)),
      moment(new Date(2016, 6, 5)),
      moment(new Date(2016, 7, 2)),
      moment(new Date(2016, 8, 6)),
      moment(new Date(2016, 9, 4)),
      moment(new Date(2016, 10, 1))
    ];
    var size = expectedDateList.length;
    expect(controller.datesList.length).toBe(size);
    for(var i = 0; i<size; i++) {
      expect(controller.datesList[i].date.date()).toEqual(expectedDateList[i].date());
    }
  });

  it('should get the correct week of the month', function() {
    commonExpectations();
    var firstTuesday =  moment(new Date(2016, 5, 7));
    var weekOfMonth = controller.getWeekOfMonth(firstTuesday);
    expect(weekOfMonth).toBe(1);
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
