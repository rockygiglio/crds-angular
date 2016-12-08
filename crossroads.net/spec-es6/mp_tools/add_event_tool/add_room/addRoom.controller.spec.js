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
  let modal;

  beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

  beforeEach(inject(($injector) => {
    log = $injector.get('$log');
    modal = $injector.get('$modal');
    qApi = $injector.get('$q');
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

    fixture = new AddRoomController(log, rootScope, modal, addEvent, lookup, room);
  }));

  describe('removeRoom()', () => {
    let modalInstance;

    beforeEach(() => {
      modalInstance = { result: {} };
      spyOn(fixture, 'removeRoomModal').and.returnValue(modalInstance);
      let deferred = qApi.defer();
      deferred.resolve({});
      modalInstance.result = deferred.promise;
    });

    it('should remove the current room from roomData if found', () => {
      let currentRoom = { id: 1, roomName: 'RoomyMcRoomFace' };
      fixture.roomData = [{ id: 1 }];
      fixture.removeRoom(currentRoom);
      rootScope.$apply();
      expect(fixture.roomData.length).toBe(0);
    });

    it('should set room and equipment to cancelled', () => {
      let currentRoom = { id: 1, roomName: 'RoomyMcCancelledFace', cancelled: false, equipment: [{ equipment: { id: 1 } }, { equipment: { id: 2 } }] };
      fixture.roomData = [];
      fixture.removeRoom(currentRoom);
      rootScope.$apply();
      expect(currentRoom.cancelled).toBe(true);
      expect(currentRoom.equipment[0].equipment.cancelled).toBe(true);
      expect(currentRoom.equipment[1].equipment.cancelled).toBe(true);
    });
  });

  describe('showNoRoomsMessage() ', () => {
    it('should return true if viewReady is false', () => {
      fixture.viewReady = false;
      const result = fixture.showNoRoomsMessage();
      expect(result).toBe(true);
    });

    it('should return false if viewReady is true and rooms.length > 1', () => {
      fixture.viewReady = true;
      fixture.rooms = [{ dougsWrong: true }];
      const result = fixture.showNoRoomsMessage();
      expect(result).toBe(false);
    });

    it('should return true if viewReady is true and rooms is undefined', () => {
      fixture.viewReady = true;
      const result = fixture.showNoRoomsMessage();
      expect(result).toBe(true);
    });

    it('should return true if viewReady is true and rooms.length is less than 1', () => {
      fixture.viewReady = true;
      fixture.rooms = [];
      const result = fixture.showNoRoomsMessage();
      expect(result).toBe(true);
    });

    it('should return true if viewReady is undefined and rooms.length is greater than 1', () => {
      fixture.rooms = [{ dougIsSuperWrong: true }];
      const result = fixture.showNoRoomsMessage();
      expect(result).toBe(true);
    });
  });
});