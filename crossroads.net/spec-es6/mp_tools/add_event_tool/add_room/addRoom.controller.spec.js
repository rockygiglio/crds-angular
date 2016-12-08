import CONSTANTS from 'crds-constants';
import AddRoomController from '../../../../app/mp_tools/add_event_tool/add_room/addRoom.controller';

describe('component: addEvent controller', () => {
  let fixture;
  let log;
  let lookup;
  let rootScope;
  let room;
  let qApi;
  let addEvent;

  beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

  beforeEach(inject(($injector) => {
    log = $injector.get('$log');
    rootScope = $injector.get('$rootScope');
    lookup = jasmine.createSpyObj('lookup', ['query']);
    lookup.query.and.callFake((params) => {
      switch (params.table) {
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
    });

    room = jasmine.createSpyObj('room.byCongregation', ['room.byCongregation']);
    addEvent = { eventData: { rooms: [] } };

    fixture = new AddRoomController(log, rootScope, addEvent, lookup, room);
  }));

  
});