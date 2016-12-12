import CONSTANTS from 'crds-constants';
import AddRoomController from '../../../../app/mp_tools/add_event_tool/add_room/addRoom.controller';

describe('component: addEvent controller', () => {
  let fixture,
    log,
    lookup,
    rootScope,
    room,
    qApi,
    modal,
    addEvent;

  beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

  beforeEach(inject(($injector) => {
    let selectedCongregation = { dp_RecordID: 15, dp_RecordName: 'Anywhere' };
    log = $injector.get('$log');
    rootScope = $injector.get('$rootScope');
    modal = $injector.get('$modal');
    lookup = jasmine.createSpyObj('lookup', ['query']);
    lookup.query.and.callFake((params, callback) => {
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
          let results = [selectedCongregation, { dp_RecordID: 2, dp_RecordName: 'I do not attend Crossroads' }, { dp_RecordID: 5, dp_RecordName: 'Childcare1' }];
          if (callback !== undefined)
            callback(results);
          return results;
        case 'childcarelocations':
          return [{ dp_RecordID: 5, dp_RecordName: 'ChildCare1' }, { dp_RecordID: 6, dp_RecordName: 'childCare2' }];
        default:
          throw `${params} not expected`;
      }
    });

    room = {
      Layouts: jasmine.createSpyObj('Layouts', ['query']),
      ByCongregation: jasmine.createSpyObj('ByCongregation', ['query'])
    };

    room.Layouts.query.and.returnValue({});

    addEvent = { eventData: { rooms: [] } };

    fixture = new AddRoomController(log, rootScope, modal, addEvent, lookup, room);
  }));

  describe('$onInit', () => {
    it('$onInit() should init location, chosenSite, roomData, EquipmentData', () => {
      let addEvent = {
        editMode: true,
        eventData: {
          event: {
            congregation: {
              dp_RecordName: 'something',
              dp_RecordID: 1
            }
          }
        }
      };
      let data = { id: 1 };

      room.ByCongregation.query.and.callFake((obj, callBack) => {
        callBack(data);
      });
      spyOn(fixture, 'setCongregation');
      spyOn(fixture, 'setRoomData');
      spyOn(fixture, 'setEquipmentData');
      spyOn(log, 'error');

      fixture.addEvent = addEvent;
      fixture.$onInit();

      expect(room.Layouts.query).toHaveBeenCalled();
      expect(fixture.setCongregation).toHaveBeenCalled();
      expect(fixture.chosenSite).toBe(addEvent.eventData.event.congregation.dp_RecordName);
      expect(room.ByCongregation.query).toHaveBeenCalledWith({ congregationId: addEvent.eventData.event.congregation.dp_RecordID }, jasmine.any(Function));
      expect(fixture.setRoomData).toHaveBeenCalledWith(data);
      expect(fixture.setEquipmentData).toHaveBeenCalled();
      expect(log.error).not.toHaveBeenCalled();
    });

    it('$onInit() should init location, nothing else', () => {
      let addEvent = {
        editMode: true,
        eventData: {
          event: {}
        }
      };

      room.ByCongregation.query.and.callFake((obj, callBack) => {
        callBack(data);
      });
      spyOn(fixture, 'setCongregation');
      spyOn(fixture, 'setRoomData');
      spyOn(fixture, 'setEquipmentData');
      spyOn(log, 'error');

      fixture.addEvent = addEvent;
      fixture.$onInit();

      expect(room.Layouts.query).toHaveBeenCalled();
      expect(fixture.setCongregation).toHaveBeenCalled();
      expect(fixture.chosenSite).toBe(undefined);
      expect(room.ByCongregation.query).not.toHaveBeenCalled();
      expect(fixture.setRoomData).not.toHaveBeenCalled();
      expect(fixture.setEquipmentData).not.toHaveBeenCalled();
      expect(log.error).toHaveBeenCalled();
    });

    it('$onInit() should init chosenSite, roomData, EquipmentData not location', () => {
      let addEvent = {
        editMode: false,
        eventData: {
          event: {
            congregation: {
              dp_RecordName: 'something',
              dp_RecordID: 1
            }
          }
        }
      };
      let data = { id: 1 };

      room.ByCongregation.query.and.callFake((obj, callBack) => {
        callBack(data);
      });
      spyOn(fixture, 'setCongregation');
      spyOn(fixture, 'setRoomData');
      spyOn(fixture, 'setEquipmentData');
      spyOn(log, 'error');

      fixture.addEvent = addEvent;
      fixture.$onInit();

      expect(room.Layouts.query).toHaveBeenCalled();
      expect(fixture.setCongregation).not.toHaveBeenCalled();
      expect(fixture.chosenSite).toBe(addEvent.eventData.event.congregation.dp_RecordName);
      expect(room.ByCongregation.query).toHaveBeenCalledWith({ congregationId: addEvent.eventData.event.congregation.dp_RecordID }, jasmine.any(Function));
      expect(fixture.setRoomData).toHaveBeenCalledWith(data);
      expect(fixture.setEquipmentData).toHaveBeenCalled();
      expect(log.error).not.toHaveBeenCalled();
    });

    it('$onInit() should init nothing but layouts', () => {
      let addEvent = {
        editMode: false,
        eventData: {
          event: {}
        }
      };

      room.ByCongregation.query.and.callFake((obj, callBack) => {
        callBack(data);
      });
      spyOn(fixture, 'setCongregation');
      spyOn(fixture, 'setRoomData');
      spyOn(fixture, 'setEquipmentData');
      spyOn(log, 'error');

      fixture.addEvent = addEvent;
      fixture.$onInit();

      expect(room.Layouts.query).toHaveBeenCalled();
      expect(fixture.setCongregation).not.toHaveBeenCalled();
      expect(fixture.chosenSite).toBe(undefined);
      expect(room.ByCongregation.query).not.toHaveBeenCalled();
      expect(fixture.setRoomData).not.toHaveBeenCalled();
      expect(fixture.setEquipmentData).not.toHaveBeenCalled();
      expect(log.error).toHaveBeenCalled();
    });
  });

  it('should setCongregation() to {id 15, name anywhere}', () => {
    let addEvent = {
      editMode: false,
      eventData: {
        event: {
          congregation: {
            dp_RecordID: 15
          }
        }
      }
    };
    fixture.addEvent = addEvent;
    fixture.setCongregation();
    expect(lookup.query).toHaveBeenCalledWith({ table: 'crossroadslocations' }, jasmine.any(Function));
    expect(fixture.addEvent.eventData.event.congregation).toEqual({ dp_RecordID: 15, dp_RecordName: 'Anywhere' });
  });

  it('should setCongregation() to undefined', () => {
    let addEvent = {
      editMode: false,
      eventData: {
        event: {
          congregation: {
            dp_RecordID: 8289282
          }
        }
      }
    };
    fixture.addEvent = addEvent;
    fixture.setCongregation();
    expect(lookup.query).toHaveBeenCalledWith({ table: 'crossroadslocations' }, jasmine.any(Function));
    expect(fixture.addEvent.eventData.event.congregation).toEqual(undefined);
  });

  fit('should setRoomData()', () => {
    let data = [{ id: 123, name: 'kens room' }, { id: 456, name: 'markkus room' }];
    let roomData = [{ name: 'joes room' }, { name: 'kens room', id: 123 }, { id: 456 }, { id: 987654321 }];
    let expected = [{ name: 'joes room' }, { name: 'kens room', id: 123 }, { id: 456, name: 'markkus room' }]
    fixture.roomData = roomData;
    fixture.setRoomData(data);
    expect(fixture.roomData).toEqual(expected);
    expect(fixture.rooms).toEqual(data);
    expect(fixture.viewReady).toBe(true);
  });

  fit('should setRoomData() to be []', () => {
    let data = [];
    let roomData = [{ name: 'joes room' }, { name: 'kens room', id: 123 }, { id: 456 }, { id: 987654321 }];
    let expected = [{ name: 'joes room' }, { name: 'kens room', id: 123 }, { id: 456, name: 'markkus room' }]
    fixture.roomData = roomData;
    fixture.setRoomData(data);
    expect(fixture.roomData).toEqual([]);
    expect(fixture.rooms).toEqual(data);
    expect(fixture.viewReady).toBe(true);
  });

});