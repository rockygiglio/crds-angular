import CONSTANTS from 'crds-constants';
import AddEventController from '../../../../app/mp_tools/add_event_tool/add_event/addEvent.controller';

describe('component: addEvent controller', () => {
  let fixture;
  let log;
  let lookup;
  let programs;
  let staffContact;
  let validation;
  let qApi;
  let addEvent;

  beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

  beforeEach(inject(($injector) => {
    log = $injector.get('$log');
    lookup = jasmine.createSpyObj('lookup', ['query']);
    lookup.query.and.callFake((params) => {
      switch (params.table)
        {
        case 'eventtypes':
          return [{
            dp_RecordID: 1,
            dp_RecordName: 'EventsYo',
            Allow_Multiday_Event: true
          }, {
            dp_RecordID: 2,
            dp_Recordname: 'childcare',
            Allow_Multiday_Event: false
          }];
        case 'reminderdays':
          return [{ dp_RecordID: 1, dp_RecordName: '1' }, { dp_RecordID: 2, dp_RecordName: '2' }];
        case 'crossroadslocations':
          return [{ dp_RecordID: 15, dp_RecordName: 'Anywhere' }, { dp_RecordID: 2, dp_RecordName: 'I do not attend Crossroads' }, { dp_RecordID: 5, dp_RecordName: 'Childcare1' }];
        case 'childcarelocations':
          return [{ dp_RecordID: 5, dp_RecordName: 'ChildCare1' }, { dp_RecordID: 6, dp_RecordName: 'childCare2' }];
        default:
          throw `${params} not expected`;
      }
    }
    );
    programs = {AllPrograms: jasmine.createSpyObj('programs.AllPrograms', ['query'])};
    programs.AllPrograms.query.and.returnValue([{ programId: 1, programName: 'Program1' }, { programid: 2, programName: 'Program2' }]);
    staffContact = jasmine.createSpyObj('staffContact', ['query']);
    staffContact.query.and.returnValue([{ contactId: 1, displayName: 'Nukem, Duke', email: 'daduke@compuserve.net' },
        { contactId: 2, displayName: 'JoeKer', email: 'joker@gmail.com' }]);
    validation = $injector.get('Validation');
    qApi = $injector.get('$q');
    addEvent = { eventData: { rooms: [] } };

    fixture = new AddEventController(log, addEvent, lookup, programs, staffContact, validation);
  }));

  beforeEach(() => {
    const startDate = new Date();
    startDate.setMinutes(0);
    startDate.setSeconds(0);
    const endDate = new Date(startDate);
    endDate.setHours(startDate.getHours() + 1);
    fixture.eventData = {
      donationBatch: 0,
      sendReminder: 0,
      minutesSetup: 0,
      minutesCleanup: 0,
      startDate: new Date(),
      endDate: new Date(),
      startTime: startDate,
      endTime: endDate
    };
  });

  it('should init() without eventData', () => {
    delete fixture.eventData;
    fixture.$onInit();

    expect(fixture.eventTypes.length).toBe(2);
    expect(fixture.eventTypes[0].dp_RecordName).toBe('EventsYo');
    expect(fixture.reminderDays.length).toBe(2);
    expect(fixture.staffContacts.length).toBe(2);
    expect(fixture.staffContacts[1].email).toBe('joker@gmail.com');
    expect(fixture.programs.length).toBe(2);
    expect(fixture.programs[0].programId).toBe(1);
    expect(fixture.eventData).not.toBeUndefined();
    expect(fixture.eventData).not.toBeNull();
    expect(fixture.eventData.donationBatch).toBe(0);

  });

  it('should resetRooms()', () => {
    fixture.addEvent.eventData.rooms.push({ roomId: 1 });
    fixture.addEvent.eventData.rooms.push({ roomId: 2 });
    expect(fixture.addEvent.eventData.rooms.length).toBe(2);
    fixture.resetRooms();
    expect(fixture.addEvent.eventData.rooms.length).toBe(0);
  });

  it('should return childCareSelected flag', () => {
    fixture.childcareSelectedFlag = false;
    expect(fixture.childcareSelected()).toBeFalsy();
    fixture.childcareSelectedFlag = true;
    expect(fixture.childcareSelected()).toBeTruthy();
  });

  describe('addEvent Controller eventTypeChanged()', () => {
    it('should set childcareSelected to true and replace locations with childcare locations', () => {
      fixture.eventData.eventType = {
        dp_RecordID: 1,
        dp_RecordName: 'Childcare'
      };
      fixture.childcareSelectedFlag = false;

      fixture.eventTypeChanged();
      expect(fixture.childcareSelectedFlag).toBeTruthy();
      expect(lookup.query).toHaveBeenCalledWith({ table: 'childcarelocations' }, jasmine.any(Function));
    });

    it('should set childcareSelectedFlag to false and get all locations', () => {
      fixture.eventData.eventType = {
        dp_RecordID: 1,
        dp_RecordName: 'Not Childcare'
      };
      fixture.childcareSelectedFlag = false;

      fixture.eventTypeChanged();
      expect(fixture.childcareSelectedFlag).toBeFalsy();
      expect(lookup.query).toHaveBeenCalledWith({ table: 'crossroadslocations' }, jasmine.any(Function));
    });
  });

  describe('addEvent Controller validDateRange()', () => {
    it('Should copy start date to end date when the selected event_type does not allow multiday events', () => {
      fixture.eventData.eventType = {
        dp_RecordID: 1,
        dp_RecordName: 'EventsYo',
        Allow_Multiday_Event: false
      };

      const addEventForm = {
        $valid: true,
        $invalid: false,
        endDate: { $valid: true, $invalid: false, $dirty: true, $error: { endDate: false } }
      };
      // setup form and data
      fixture.form = addEventForm;
      fixture.eventData.startDate = new Date(+new Date() + 86400000);
      expect(fixture.eventData.endDate).toBeLessThan(fixture.eventData.startDate);
      // call function and verify the date was copied
      const result = fixture.validDateRange(fixture.form);
      expect(result).toBeFalsy();
      expect(fixture.form).toBe(addEventForm);
      expect(fixture.eventData.endDate).toBe(fixture.eventData.startDate);
    });

    it('Should not copy start date to end date when the selected event_type does allow multiday events', () => {
      fixture.eventData.eventType = {
        dp_RecordID: 1,
        dp_RecordName: 'EventsYo',
        Allow_Multiday_Event: true
      };

      const addEventForm = {
        $valid: true,
        $invalid: false,
        endDate: { $valid: true, $invalid: false, $dirty: true, $error: { endDate: false } }
      };
      // setup form and data
      fixture.form = addEventForm;
      fixture.eventData.startDate = new Date(+new Date() + 86400000);
      expect(fixture.eventData.endDate).toBeLessThan(fixture.eventData.startDate);
      // call function and verify the date was not copied and the form is in error state
      const result = fixture.validDateRange(fixture.form);
      expect(result).toBeTruthy();
      expect(fixture.form.$valid).toBeFalsy();
      expect(fixture.form.endDate.$error.endDate).toBeTruthy();
      expect(fixture.eventData.endDate).toBeLessThan(fixture.eventData.startDate);
    });

    it('should return invalid if dateTime call errors out', () => {
      fixture.eventData.eventType = {
        dp_RecordID: 1,
        dp_RecordName: 'EventsYo',
        Allow_Multiday_Event: true
      };

      const addEventForm = {
        $valid: true,
        $invalid: false,
        endDate: { $valid: true, $invalid: false, $dirty: true, $error: { endDate: false } }
      };
      // setup form and data
      fixture.form = addEventForm;
      fixture.eventData.startDate = 'abc';
      fixture.eventData.startTime = 'abc';
      const result = fixture.validDateRange(fixture.form);
      expect(result).toBeTruthy();
      expect(fixture.form.endDate.$error.endDate).toBeTruthy();
      expect(fixture.form.$invalid).toBeTruthy();
      expect(fixture.form.$valid).toBeFalsy();
    });

    it('should return false if form is undefined', () => {
      fixture.form = undefined;
      const result = fixture.validDateRange(fixture.form);
      expect(result).toBeFalsy();
    });
  });
});
