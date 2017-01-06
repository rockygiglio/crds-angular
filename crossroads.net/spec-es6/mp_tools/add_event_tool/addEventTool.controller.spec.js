import CONSTANTS from 'crds-constants';
import AddEventToolController from '../../../app/mp_tools/add_event_tool/addEventTool.controller';

describe('AddEventTool', () => {
  let fixture,
    log,
    AddEvent,
    rootScope,
    window,
    MPTools,
    modal,
    AuthService,
    EventService,
    sd1,
    sd2,
    ed1,
    ed2,
    time,
    origCongregation,
    newCongregation,
    CRDS_TOOLS_CONSTANTS = {
      SECURITY_ROLES: {
        EventsRoomsEquipment: 'test'
      }
    };

  beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

  beforeEach(inject(($injector) => {
    log = $injector.get('$log');
    rootScope = jasmine.createSpyObj('$rootScope', ['$emit']);
    window = $injector.get('$window');
    MPTools = jasmine.createSpyObj('MPTools', ['getParams']);
    AuthService = jasmine.createSpyObj('AuthService', ['isAuthenticated', 'isAuthorized']);
    modal = jasmine.createSpyObj('$modal', ['open']);
        // AddEvent = jasmine.createSpyObj('AddEvent', ['test']);
    AddEvent = {
      currentPage: 5,
      editMode: true,
      eventData: {
      }
    };
    EventService = $injector.get('EventService');
    CRDS_TOOLS_CONSTANTS = {};

    fixture = new AddEventToolController(rootScope, window, log, modal, MPTools, AuthService, EventService, CRDS_TOOLS_CONSTANTS, AddEvent);
  }));

  it('should go back()', () => {
    fixture.back();
    expect(fixture.AddEvent.currentPage).toBe(1);
  });

  it('should return correct currentPage()', () => {
    const curPage = fixture.currentPage();
    expect(curPage).toBe(5);
  });

  it('should return if isEditMode()', () => {
    const editMode = fixture.isEditMode();
    expect(editMode).toBe(true);
  });

  describe('next()', () => {
    beforeEach(() => {
      fixture.allData = {
        eventForm: {
          $setSubmitted: () => {
            return;
          },
          $valid: true,
          maximumChildren: {
            $valid: true
          },
          minimumChildren: {
            $valid: true
          }
        }
      };

      fixture.rootScope.MESSAGES = {
        generalError: 'error'
      };
    });

    it('should move page to next() is not edit', () => {
      fixture.AddEvent.editMode = false;
      fixture.event = {
        test: 'test'
      };

      spyOn(fixture, 'canSaveMaintainOldReservation');

      fixture.next();
      expect(fixture.canSaveMaintainOldReservation).not.toHaveBeenCalled();
      expect(fixture.AddEvent.currentPage).toBe(2);
      expect(fixture.AddEvent.eventData.event.test).toBe('test');
    });

    it('should move page to next() is edit no modal', () => {
      fixture.AddEvent.editMode = true;
      fixture.event = {
        test: 'test'
      };
      spyOn(fixture, 'canSaveMaintainOldReservation').and.returnValue(true);
      fixture.next();
      expect(fixture.canSaveMaintainOldReservation).toHaveBeenCalled();
      expect(fixture.AddEvent.currentPage).toBe(2);
      expect(fixture.AddEvent.eventData.event.test).toBe('test');
    });

    it('should move page to next() is edit modal', () => {
      fixture.AddEvent.editMode = true;
      fixture.event = {
        test: 'test'
      };
      spyOn(fixture, 'canSaveMaintainOldReservation').and.returnValue(false);
      spyOn(fixture, 'continueEdit');
      fixture.next();
      expect(fixture.canSaveMaintainOldReservation).toHaveBeenCalled();
      expect(fixture.continueEdit).toHaveBeenCalled();
      expect(fixture.AddEvent.eventData.event.test).toBe('test');
    });

    it('cancelEvent() should set all rooms, equipment and the event to cancelled', () => {
      fixture.AddEvent.editMode = true;
      fixture.AddEvent.eventData.event = {
        name: 'test'
      };
      fixture.rooms = [{
        cancelled: false,
        equipment: [{
          equipment: {
            cancelled: false,
            choosenQuantity: 2,
            notes: 'abcd',
            equipmentReservationId: 42,
            name: { id: 3 }
          }
        }, {
          equipment: {
            cancelled: false,
            choosenQuantity: 32,
            equipmentReservationId: 33,
            name: { id: 4 }
          }
        }],
        hidden: false,
        id: 1234,
        layout: { id: 1 },
        notes: 'Dem notes',
        roomReservationId: 1234
      }, {
        cancelled: false,
        equipment: [{
          equipment: {
            cancelled: false,
            notes: null,
            choosenQuantity: 2,
            equipmentReservationId: 23,
            name: { id: 6 }
          }
        }, {
          equipment: {
            cancelled: false,
            notes: '',
            choosenQuantity: 22,
            equipmentReservationId: 34,
            name: { id: 7 }
          }
        }],
        hidden: false,
        id: 1235,
        layout: { id: 1 },
        roomReservationId: 1235
      }];
      fixture.processEdit = jasmine.createSpy('processEdit');
      fixture.AddEvent.getEventDto = (data) => {
        return {
          startDateTime: '9:30:00',
          endDateTime: '10:30:00'
        };
      };

      fixture.cancelEvent();
      expect(fixture.processEdit).toHaveBeenCalled();
      expect(fixture.rootScope.$emit).not.toHaveBeenCalled();
      expect(fixture.processing).toBe(true);
      expect(fixture.AddEvent.eventData.event.cancelled).toBe(true);
      expect(fixture.AddEvent.eventData.rooms[0].cancelled).toBe(true);
      expect(fixture.AddEvent.eventData.rooms[1].cancelled).toBe(true);
      expect(fixture.AddEvent.eventData.rooms[0].notes).toBe('***Cancelled***Dem notes');
      expect(fixture.AddEvent.eventData.rooms[1].notes).toBe('***Cancelled***', 'Because notes is undefined');
      expect(fixture.AddEvent.eventData.rooms[0].equipment[0].equipment.cancelled).toBe(true);
      expect(fixture.AddEvent.eventData.rooms[0].equipment[0].equipment.notes).toBe('***Cancelled***abcd');
      expect(fixture.AddEvent.eventData.rooms[0].equipment[1].equipment.notes).toBe('***Cancelled***', 'Because notes is undefined');
      expect(fixture.AddEvent.eventData.rooms[1].equipment[1].equipment.cancelled).toBe(true);
      expect(fixture.AddEvent.eventData.rooms[1].equipment[0].equipment.notes).toBe('***Cancelled***', 'Because notes is null');
      expect(fixture.AddEvent.eventData.rooms[1].equipment[0].equipment.notes).toBe('***Cancelled***', 'Because notes is empty string');
    });

    it('should error when trying to move page to next()', () => {
      fixture.allData = {
        eventForm: {
          $setSubmitted: () => {
            return;
          },
          $valid: false,
          maximumChildren: {
            $valid: true
          },
          minimumChildren: {
            $valid: true
          }
        }
      };
      fixture.AddEvent.editMode = true;
      fixture.event = {
        test: 'test'
      };
      fixture.next();
      expect(fixture.AddEvent.currentPage).toBe(5);
      expect(fixture.AddEvent.eventData.event.test).toBe('test');
      expect(fixture.rootScope.$emit).toHaveBeenCalled();
    });
  });

  describe('Submit()', () => {
    beforeEach(() => {
      fixture.AddEvent.getEventDto = (data) => {
        return {
          startDateTime: '9:30:00',
          endDateTime: '10:30:00'
        };
      };
      fixture.allData = {
        roomForm: {
          $setSubmitted: () => {
            return;
          },
          equipmentForm: {
            $setSubmitted: () => {
              return;
            }
          }
        },
        $valid: true
      };
      fixture.rooms = [];
    });

    it('should submit() as save mode', () => {
      fixture.AddEvent.editMode = false;
      fixture.processSave = jasmine.createSpy('processSave');
      fixture.submit();
      expect(fixture.processSave).toHaveBeenCalled();
      expect(fixture.rootScope.$emit).not.toHaveBeenCalled();
      expect(fixture.processing).toBe(true);
    });

    it('should submit() as edit mode', () => {
      fixture.AddEvent.editMode = true;
      fixture.processEdit = jasmine.createSpy('processEdit');
      fixture.submit();
      expect(fixture.processEdit).toHaveBeenCalled();
      expect(fixture.rootScope.$emit).not.toHaveBeenCalled();
      expect(fixture.processing).toBe(true);
    });

    it('should not submit() it is invalid', () => {
      fixture.allData.$valid = false;
      fixture.rootScope.MESSAGES = {
        generalError: 'error'
      };
      fixture.submit();
      expect(fixture.rootScope.$emit).toHaveBeenCalled();
      expect(fixture.processing).toBe(false);
    });
  });

  describe('processEdit', () => {
    beforeEach(() => {
      fixture.processing = true;
      fixture.rootScope.MESSAGES = {
        generalError: 'error',
        eventUpdateSuccess: 'success',
        eventToolProblemSaving: 'err'
      };
      spyOn(window, 'close');
    });

    it('should processEdit()', () => {
      const expectedResult = {};
      spyOn(EventService.eventTool, 'update').and.callFake((data, event, callbackFn, errCallbackFn) => {
        callbackFn(expectedResult);
      });

      fixture.processEdit({});
      expect(fixture.processing).toBe(false);
      expect(fixture.rootScope.$emit).toHaveBeenCalled();
      expect(fixture.AddEvent.eventData).toEqual({});
      expect(window.close).toHaveBeenCalled();
    });

    it('should processEdit() with err', () => {
      const err = {};
      spyOn(EventService.eventTool, 'update').and.callFake((data, event, callbackFn, errCallbackFn) => {
        errCallbackFn(err);
      });

      fixture.processEdit({});
      expect(fixture.processing).toBe(false);
      expect(fixture.rootScope.$emit).toHaveBeenCalled();
    });
  });

  describe('processSave', () => {
    beforeEach(() => {
      fixture.processing = true;
      fixture.rootScope.MESSAGES = {
        generalError: 'error',
        eventSuccess: 'success',
        eventToolProblemSaving: 'err'
      };
      spyOn(window, 'close');
    });

    it('should processSave()', () => {
      const expectedResult = {};
      spyOn(EventService.create, 'save').and.callFake((event, callbackFn, errCallbackFn) => {
        callbackFn(expectedResult);
      });

      fixture.processSave({});
      expect(fixture.processing).toBe(false);
      expect(fixture.rootScope.$emit).toHaveBeenCalled();
      expect(fixture.AddEvent.eventData.event).toEqual({});
      expect(fixture.AddEvent.eventData.rooms).toEqual([]);
      expect(fixture.AddEvent.eventData.group).toEqual({});
      expect(window.close).toHaveBeenCalled();
      expect(fixture.rooms).toEqual([]);
      expect(fixture.event).toEqual({});
      expect(fixture.AddEvent.currentPage).toBe(1);
    });

    it('should processSave() with err', () => {
      const err = {};
      spyOn(EventService.create, 'save').and.callFake((event, callbackFn, errCallbackFn) => {
        errCallbackFn(err);
      });

      fixture.processSave({});
      expect(fixture.processing).toBe(false);
      expect(fixture.rootScope.$emit).toHaveBeenCalled();
    });
  });

  describe('Date Related Bool Functions', () => {
    beforeEach(() => {
      sd1 = new Date('3/12/2017');
      sd2 = new Date('3/20/2017');
      ed1 = new Date('4/20/2017');
      ed2 = new Date('4/12/2017');
      time = new Date();
      origCongregation = 1;
      newCongregation = 2;
      fixture.event = { congregation: {} };

      fixture.event.startDate = sd1;
      fixture.event.startTime = time;
      fixture.event.endDate = ed1;
      fixture.event.endTime = time;
      fixture.AddEvent.origStartDate = sd1;
      fixture.AddEvent.origStartTime = time;
      fixture.AddEvent.origEndDate = ed1;
      fixture.AddEvent.origEndTime = time;

      fixture.AddEvent.dateTime = (dateForDate, dateForTime) => {
        return new Date(
                    dateForDate.getFullYear(),
                    dateForDate.getMonth(),
                    dateForDate.getDate(),
                    dateForTime.getHours(),
                    dateForTime.getMinutes(),
                    dateForTime.getSeconds(),
                    dateForTime.getMilliseconds());
      };
    });

    it('should return true doesDateRangeFitInsideOtherDateRange()', () => {
      const result = fixture.doesDateRangeFitInsideOtherDateRange(sd1, ed1, sd2, ed2);
      expect(result).toBe(true);
    });

    it('should return false doesDateRangeFitInsideOtherDateRange()', () => {
      const result = fixture.doesDateRangeFitInsideOtherDateRange(sd2, ed2, sd1, sd1);
      expect(result).toBe(false);
    });

    it('should return true canSaveMaintainOldReservation()', () => {
      fixture.event.congregation.dp_RecordID = origCongregation;
      fixture.AddEvent.origCongregation = origCongregation;

      const result = fixture.canSaveMaintainOldReservation();
      expect(result).toBe(true);
    });

    it('should return false canSaveMaintainOldReservation() diff Congregation', () => {
      fixture.event.congregation.dp_RecordID = newCongregation;
      fixture.AddEvent.origCongregation = origCongregation;

      const result = fixture.canSaveMaintainOldReservation();
      expect(result).toBe(false);
    });
  });

  describe('AllowAccess()', () => {
    beforeEach(() => {
      fixture.CRDS_TOOLS_CONSTANTS = {
        SECURITY_ROLES: {
          EventsRoomsEquipment: 'test'
        }
      };
    });

    it('should allowAccess()', () => {
      AuthService.isAuthenticated.and.returnValue(true);
      AuthService.isAuthorized.and.returnValue(true);

      const allowAccessReturn = fixture.allowAccess();
      expect(allowAccessReturn).toBe(true);
    });

    it('should not allowAccess() because not authenticated', () => {
      AuthService.isAuthenticated.and.returnValue(false);
      AuthService.isAuthorized.and.returnValue(true);

      const allowAccessReturn = fixture.allowAccess();
      expect(allowAccessReturn).toBe(false);
    });

    it('should not allowAccess() because not authorized', () => {
      AuthService.isAuthenticated.and.returnValue(true);
      AuthService.isAuthorized.and.returnValue(false);

      const allowAccessReturn = fixture.allowAccess();
      expect(allowAccessReturn).toBe(false);
    });

    it('should not allowAccess() because not authenticated or authorized', () => {
      AuthService.isAuthenticated.and.returnValue(false);
      AuthService.isAuthorized.and.returnValue(false);

      const allowAccessReturn = fixture.allowAccess();
      expect(allowAccessReturn).toBe(false);
    });
  });
});