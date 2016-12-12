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
    addEvent,
    congregationId,
    allReadyAdded,
    chooseARoom;

  beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

  beforeEach(inject(($injector) => {
    congregationId = 1;
    let selectedCongregation = { dp_RecordID: 15, dp_RecordName: 'Anywhere' };
    log = $injector.get('$log');
    rootScope = jasmine.createSpyObj('$rootScope', ['$emit']);
    modal = jasmine.createSpyObj('$modal', ['open']);
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
      ByCongregation: jasmine.createSpyObj('ByCongregation', ['query']),
      Equipment: jasmine.createSpyObj('Equipment', ['query'])
    };

    modal.open.and.returnValue({});
    room.Layouts.query.and.returnValue({});

    
    allReadyAdded = 'already chosen';
    chooseARoom = 'choose a room';
    rootScope.MESSAGES = {
      allReadyAdded: allReadyAdded,
      chooseARoom: chooseARoom
    };

    addEvent = { eventData: { rooms: [], event: { congregation: { dp_RecordID: congregationId } } } };

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

  it('should setRoomData()', () => {
    let data = [{ id: 123, name: 'kens room' }, { id: 456, name: 'markkus room' }];
    let roomData = [{ name: 'joes room' }, { name: 'kens room', id: 123 }, { id: 456 }, { id: 987654321 }];
    let expected = [{ name: 'joes room' }, { name: 'kens room', id: 123 }, { id: 456, name: 'markkus room' }];
    fixture.roomData = roomData;
    fixture.setRoomData(data);
    expect(fixture.roomData).toEqual(expected);
    expect(fixture.rooms).toEqual(data);
    expect(fixture.viewReady).toBe(true);
  });

  it('should setRoomData() with no data param', () => {
    let data = [];
    let roomData = [{ name: 'joes room' }, { name: 'kens room', id: 123 }, { id: 456 }, { id: 987654321 }];
    let expected = [{ name: 'joes room' }, { name: 'kens room', id: 123 }];
    fixture.roomData = roomData;
    fixture.setRoomData(data);
    expect(fixture.roomData).toEqual(expected);
    expect(fixture.rooms).toEqual(data);
    expect(fixture.viewReady).toBe(true);
  });

  it('should setRoomData() to []', () => {
    let data = [{ id: 123, name: 'kens room' }, { id: 456, name: 'markkus room' }];
    let roomData = [];
    let expected = [];
    fixture.roomData = roomData;
    fixture.setRoomData(data);
    expect(fixture.roomData).toEqual(expected);
    expect(fixture.rooms).toEqual(data);
    expect(fixture.viewReady).toBe(true);
  });

  it('should setCongregation()', () => {
    let data, roomData, eOne, eTwo, eThree;
    data = [{ Id: 1 }, { Id: 2 }, { Id: 3 }];
    roomData = [{ equipment: { Id: 1 } }, { equipment: { Id: 2 } }, { equipment: { Id: 3 } }];
    fixture.roomData = roomData;
    eOne = { name: 'tables' };
    eTwo = { name: 'chairs' };
    eThree = { name: 'flowers' };
    spyOn(fixture, 'mapEquipment').and.callFake((dataSet, e) => {
      switch (e.Id) {
        case 1:
          return eOne;
        case 2:
          return eTwo;
        case 3:
          return eThree;
        default:
          return;
      }
    })
    room.Equipment.query.and.callFake((obj, callBack) => {
      callBack(data);
    });
    fixture.setEquipmentData();
    expect(room.Equipment.query).toHaveBeenCalledWith({ congregationId: congregationId }, jasmine.any(Function))
    expect(fixture.equipmentList).toEqual(data);
    expect(fixture.roomData[0].equipment).toEqual(eOne);
    expect(fixture.roomData[1].equipment).toEqual(eTwo);
    expect(fixture.roomData[2].equipment).toEqual(eThree);
  });

  it('should setCongregation() and have nothing be set', () => {
    let data, roomData, eOne, eTwo, eThree;
    data = [];
    roomData = [];
    fixture.roomData = roomData;
    eOne = { name: 'tables' };
    eTwo = { name: 'chairs' };
    eThree = { name: 'flowers' };
    spyOn(fixture, 'mapEquipment').and.callFake((dataSet, e) => {
      switch (e.Id) {
        case 1:
          return eOne;
        case 2:
          return eTwo;
        case 3:
          return eThree;
        default:
          return;
      }
    })
    room.Equipment.query.and.callFake((obj, callBack) => {
      callBack(data);
    });
    fixture.setEquipmentData();
    expect(room.Equipment.query).toHaveBeenCalledWith({ congregationId: congregationId }, jasmine.any(Function))
    expect(fixture.equipmentList).toEqual(data);
    expect(fixture.roomData[0]).toBe(undefined);
    expect(fixture.roomData[1]).toEqual(undefined);
    expect(fixture.roomData[2]).toEqual(undefined);
  });

  it('should return true for isCancelled()', () => {
    let equipment = {
      id: 1,
      cancelled: true
    };
    let result = fixture.isCancelled(equipment);
    expect(result).toBe(true);
  });

  it('should return false for isCancelled() because cancelled is false', () => {
    let equipment = {
      id: 1,
      cancelled: false
    };
    let result = fixture.isCancelled(equipment);
    expect(result).toBe(false);
  });

  it('should return false for isCancelled() because cancelled doesnt exist', () => {
    let equipment = {
      id: 1
    };
    let result = fixture.isCancelled(equipment);
    expect(result).toBe(false);
  });

  it('should open removeRoomModal()', () => {
    let eController, eControllerAs, eTemplateUrl, eResolve, room, returnValue, eModal;
    eController = 'RemoveRoomController';
    eControllerAs = 'removeRoom';
    eTemplateUrl = 'remove_room/remove_room.html';
    room = {id:1, name: 'room1'};
    eResolve = { items() { return room; } };

    eModal = {
      controller: eController,
      controllerAs: eControllerAs,
      templateUrl: eTemplateUrl,
      resolve: eResolve
    }

    returnValue = fixture.removeRoomModal(room);
    expect(modal.open).toHaveBeenCalled();
    expect(modal.open.calls.mostRecent().args[0].controller).toBe(eController);
    expect(modal.open.calls.mostRecent().args[0].controllerAs).toBe(eControllerAs);
    expect(modal.open.calls.mostRecent().args[0].templateUrl).toBe(eTemplateUrl);
    expect(modal.open.calls.mostRecent().args[0].resolve.items).toEqual(jasmine.any(Function));
  })

  it('add stuff onAdd()', () => {
    let chosenRoom, roomData, dougsRoom, kensRoom, joesRoom;
    dougsRoom = {id:3, name: 'dougs room'};
    kensRoom = {id:2, name: 'kens room'};
    joesRoom = {id: 1, name: 'joes room'};

    roomData = [kensRoom, dougsRoom]

    fixture.chosenRoom = joesRoom;
    fixture.roomData = roomData;

    fixture.onAdd();

    expect(fixture.roomData.length).toBe(3);
    expect(fixture.roomData[2]).toEqual(joesRoom);
    expect(fixture.rootScope.$emit).not.toHaveBeenCalledWith('notify', allReadyAdded);
    expect(fixture.rootScope.$emit).not.toHaveBeenCalledWith('notify', chooseARoom);
  })

  it('add stuff onAdd() alreadyAdded, but cancelled', () => {
    let chosenRoom, roomData, dougsRoom, kensRoom, joesRoom, joesRoomCancelled, joesRoomNotCancelled, message;

    dougsRoom = {id:3, name: 'dougs room'};
    kensRoom = {id:2, name: 'kens room'};
    joesRoom = {id: 1, name: 'joes room'};
    joesRoomCancelled = {id: 1, name: 'joes room', cancelled: true};
    joesRoomNotCancelled = {id: 1, name: 'joes room', cancelled: false};

    roomData = [joesRoomCancelled, kensRoom, dougsRoom]

    fixture.chosenRoom = joesRoom;
    fixture.roomData = roomData;

    fixture.onAdd();

    expect(fixture.roomData.length).toBe(3);
    expect(fixture.roomData[0]).toEqual(joesRoomNotCancelled);
    expect(fixture.rootScope.$emit).not.toHaveBeenCalledWith('notify', allReadyAdded);
    expect(fixture.rootScope.$emit).not.toHaveBeenCalledWith('notify', chooseARoom);
  })

  it('add stuff onAdd() alreadyAdded, room data shouldnt change', () => {
    let chosenRoom, roomData, dougsRoom, kensRoom, joesRoom, message;

    dougsRoom = {id:3, name: 'dougs room'};
    kensRoom = {id:2, name: 'kens room'};
    joesRoom = {id: 1, name: 'joes room'};

    roomData = [joesRoom, kensRoom, dougsRoom]

    fixture.chosenRoom = joesRoom;
    fixture.roomData = roomData;

    fixture.onAdd();

    expect(fixture.roomData.length).toBe(3);
    expect(fixture.roomData[2]).toEqual(dougsRoom);
    expect(fixture.rootScope.$emit).toHaveBeenCalledWith('notify', allReadyAdded);
    expect(fixture.rootScope.$emit).not.toHaveBeenCalledWith('notify', chooseARoom);
  })

  it('add stuff onAdd() selectedRoom is null, room data shouldnt change', () => {
    let chosenRoom, roomData, dougsRoom, kensRoom, joesRoom, message;

    dougsRoom = {id:3, name: 'dougs room'};
    kensRoom = {id:2, name: 'kens room'};
    joesRoom = {id: 1, name: 'joes room'};

    roomData = [joesRoom, kensRoom, dougsRoom]

    fixture.chosenRoom = null;
    fixture.roomData = roomData;

    fixture.onAdd();

    expect(fixture.roomData.length).toBe(3);
    expect(fixture.rootScope.$emit).not.toHaveBeenCalledWith('notify', allReadyAdded);
    expect(fixture.rootScope.$emit).toHaveBeenCalledWith('notify', chooseARoom);
  })

  it('should mapEquipment()', () => {
    let currentEquipmentList, equipmentLookup;

    currentEquipmentList = [];

    fixture.mapEquipment(equipmentLookup, currentEquipmentList)
    expect().toBe();
  })

});