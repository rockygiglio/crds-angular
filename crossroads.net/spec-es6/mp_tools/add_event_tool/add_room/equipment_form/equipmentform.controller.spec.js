import CONSTANTS from 'crds-constants';
import EquipmentFormController from '../../../../../app/mp_tools/add_event_tool/add_room/equipment_form/equipmentForm.controller';

describe('AddEventTool EquipmentForm', () => {
  let fixture,
    AddEvent,
    Validation;

  beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

  beforeEach(inject(($injector) => {
    AddEvent = {
      currentPage: 5,
      editMode: true,
      eventData: {
      }
    };
    Validation = jasmine.createSpyObj('Validation', ['showErrors']);

    fixture = new EquipmentFormController(AddEvent, Validation);
  }));

  it('$onInit() should not set equipmentRequired if edit mode is false and there is no equipment', () => {
    fixture.addEvent = { editMode: false };
    fixture.currentEquipment = [];
    fixture.currentRoom = { cancelled: false };
    fixture.$onInit();
    expect(fixture.equipmentRequired).toBeUndefined();
  });

  it('$onInit() should not set equipmentRequired if edit mode is false and the only equipment has id of 0', () => {
    fixture.addEvent = { editMode: false };
    fixture.currentEquipment = [{ equipment: { name: { id: 0 } } }];
    fixture.currentRoom = { cancelled: false };
    fixture.$onInit();
    expect(fixture.equipmentRequired).toBeUndefined();
  });

  it('$onInit() should set equipment required to true if editMode is false and current equipment has id > 0', () => {
    fixture.addEvent = { editMode: false };
    fixture.currentEquipment = [{ equipment: { name: { id: 23 } } }];
    fixture.currentRoom = { cancelled: false };
    fixture.$onInit();
    expect(fixture.equipmentRequired).toBe(true);
  });

  it('$onInit() should set equipmentRequired to false if edit mode is true and there is no equipment', () => {
    fixture.currentEquipment = [];
    fixture.currentRoom = { cancelled: false };
    fixture.$onInit();
    expect(fixture.equipmentRequired).toBe(false);
  });

  it('should set equipmentRequired to false if edit mode is true and there is no equipment with id > 0', () => {
    fixture.currentEquipment = [{
      equipment: {
        name: {
          id: 0
        }
      }
    }];
    fixture.currentRoom = { cancelled: false };
    fixture.$onInit();
    expect(fixture.equipmentRequired).toBe(false);
  });
  it('should set equipmentRequired to undefined if edit mode is true but cancelled is not set', () => {
    fixture.currentEquipment = [{
      equipment: {
        name: {
          id: 0
        }
      }
    }];
    fixture.currentRoom = {};
    fixture.$onInit();
    expect(fixture.equipmentRequired).toBe(undefined);
  });

  it('$onInit() should set equipmentRequired to true if edit mode is true and there is some equipment', () => {
    fixture.currentEquipment = [{
      equipment: {
        name: {
          id: 123
        }
      }
    }];
    fixture.currentRoom = { cancelled: false };
    fixture.$onInit();
    expect(fixture.equipmentRequired).toBe(true);
  });

  it('should addEquipment', () => {
    fixture.currentEquipment = [{
      equipment: {
        id: 1
      }
    }];

    fixture.addEquipment();
    expect(fixture.currentEquipment.length).toBe(2);
  });

  it('should return true for existing() equipment', () => {
    const idx = 0;
    const equipment = {
      id: 1,
      cancelled: false
    };

    const result = fixture.existing(equipment);
    expect(result).toBe(true);
  });

  it('should return false for existing() equipment', () => {
    const idx = 0;
    const equipment = {
      id: 1
    };

    const result = fixture.existing(equipment);
    expect(result).toBe(false);
  });

  it('should compose fieldName() correctly', () => {
    const name = 'field';
    const idx = 2;
    const fieldName = `${name}-${idx}`;
    const fieldNameResult = fixture.fieldName(name, idx);
    expect(fieldName).toBe(fieldNameResult);
  });

  it('should return true for isCancelled()', () => {
    const equipment = {
      id: 1,
      cancelled: true
    };
    const result = fixture.isCancelled(equipment);
    expect(result).toBe(true);
  });

  it('should return false for isCancelled() because cancelled is false', () => {
    const equipment = {
      id: 1,
      cancelled: false
    };
    const result = fixture.isCancelled(equipment);
    expect(result).toBe(false);
  });

  it('should return false for isCancelled() because cancelled doesnt exist', () => {
    const equipment = {
      id: 1
    };
    const result = fixture.isCancelled(equipment);
    expect(result).toBe(false);
  });

  it('should be remove()ed from the array completely', () => {
    fixture.currentEquipment = [{
      equipment: {
        id: 1
      }
    }, {
      equipment: {
        id: 2
      }
    }, {
      equipment: {
          id: 3
        }
    }];

    const idx = 1;
    fixture.remove(idx);
    expect(fixture.currentEquipment.length).toBe(2);
    expect(fixture.currentEquipment[0].equipment.id).toBe(1);
    expect(fixture.currentEquipment[1].equipment.id).toBe(3);
  });

  it('should be remove()ed by setting cancelled to true', () => {
    fixture.currentEquipment = [{
      equipment: {
        id: 1,
        cancelled: false
      }
    }, {
      equipment: {
        id: 2,
        cancelled: false
      }
    }, {
      equipment: {
          id: 3,
          cancelled: false
        }
    }];

    const idx = 1;
    fixture.remove(idx);
    expect(fixture.currentEquipment.length).toBe(3);
    expect(fixture.currentEquipment[0].equipment.cancelled).toBe(false);
    expect(fixture.currentEquipment[1].equipment.cancelled).toBe(true);
    expect(fixture.currentEquipment[2].equipment.cancelled).toBe(false);
  });

  it('should return true for showErrors()', () => {
    Validation.showErrors.and.callFake((form, params) => {
      if (params == 'equipmentChooser') {
        return true;
      } else if (params == 'equip.quantity') {
        return true;
      }
    });
    const result = fixture.showError({});
    expect(result).toBe(true);
  });

  it('should return false for showErrors() because equipmentChooser is false', () => {
    Validation.showErrors.and.callFake((form, params) => {
      if (params == 'equipmentChooser') {
        return false;
      } else if (params == 'equip.quantity') {
        return true;
      }
    });
    const result = fixture.showError({});
    expect(result).toBe(true);
  });

  it('should return false for showErrors() because equip.quantity is false', () => {
    Validation.showErrors.and.callFake((form, params) => {
      if (params == 'equipmentChooser') {
        return true;
      } else if (params == 'equip.quantity') {
        return false;
      }
    });
    const result = fixture.showError({});

    expect(result).toBe(true);
  });

  it('should return false for showErrors() because both are false', () => {
    Validation.showErrors.and.callFake((form, params) => {
      if (params == 'equipmentChooser') {
        return false;
      } else if (params == 'equip.quantity') {
        return false;
      }
    });
    const result = fixture.showError({});

    expect(result).toBe(false);
  });

  it('should return true from showFieldError()', () => {
    Validation.showErrors.and.callFake((form, params) => {
      if (params == 'testField')
              { return true; }
    });
    const result = fixture.showFieldError({}, 'testField');

    expect(result).toBe(true);
  });

  it('should return false from showFieldError()', () => {
    Validation.showErrors.and.callFake((form, params) => {
      if (params == 'testField')
              { return false; }
    });
    const result = fixture.showFieldError({}, 'testField');

    expect(result).toBe(false);
  });

  it('should not undo() anything because equipment is undefined', () => {
    const idx = 3000;
    fixture.currentEquipment = [{
      equipment: {
        id: 1
      }
    }, {
      equipment: {
        id: 2
      }
    }, {
      equipment: {
          id: 3
        }
    }];
    spyOn(fixture, 'existing');
    fixture.undo(idx);
    expect(fixture.existing).not.toHaveBeenCalled();
  });

  it('should not undo() anything because equipment is not existing', () => {
    const idx = 1;
    fixture.currentEquipment = [{
      equipment: {
        id: 1
      }
    }, {
      equipment: {
        id: 2
      }
    }, {
      equipment: {
          id: 3
        }
    }];
    spyOn(fixture, 'existing').and.returnValue(false);
    fixture.undo(idx);
    expect(fixture.existing).toHaveBeenCalled();
    expect(fixture.currentEquipment[1].equipment.cancelled).toBe(undefined);
  });

  it('should undo()', () => {
    const idx = 1;
    fixture.currentEquipment = [{
      equipment: {
        id: 1,
        cancelled: true
      }
    }, {
      equipment: {
        id: 2,
        cancelled: true
      }
    }, {
      equipment: {
          id: 3,
          cancelled: true
        }
    }];
    spyOn(fixture, 'existing').and.returnValue(true);
    fixture.undo(idx);
    expect(fixture.existing).toHaveBeenCalled();
    expect(fixture.currentEquipment[1].equipment.cancelled).toBe(false);
  });

  it('toggleEquipmentRequired should clear current equipment if equipmentRequired is false', () => {
    fixture.equipmentRequired = false;
    fixture.currentEquipment = [{ equipment: { id: 27, name: 'Dat Equipment', quantity: 42 } }, { equipment: { id: 21, name: 'Dat Equipment 2', quantity: 2 } }];
    fixture.toggleEquipmentRequired();
    expect(fixture.currentEquipment[0].equipment.name.id).toBe(0);
    expect(fixture.currentEquipment.length).toBe(1);
  });

  it('toggleEquipmentRequired should not clear current equipment if equipmentRequired is true', () => {
    fixture.equipmentRequired = true;
    fixture.currentEquipment = [{ equipment: { id: 27, name: 'Dat Equipment', quantity: 42 } }, { equipment: { id: 41, name: 'Dat Equipment 2', quantity: 1 } }];
    fixture.toggleEquipmentRequired();
    expect(fixture.currentEquipment[0].equipment.id).toBe(27);
    expect(fixture.currentEquipment.length).toBe(2);
  });
});