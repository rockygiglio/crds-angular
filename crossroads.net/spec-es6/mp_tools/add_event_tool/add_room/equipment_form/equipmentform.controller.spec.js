import CONSTANTS from 'crds-constants';
import EquipmentFormController from '../../../../../app/mp_tools/add_event_tool/add_room/equipment_form/equipmentForm.controller';

describe('AddEventTool EquipmentForm', () => {
    let fixture,
        AddEvent,
        Validation

    beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

    beforeEach(inject(($injector) => {
        AddEvent = {
            currentPage: 5,
            editMode: true,
            eventData: {
            }
        };
        Validation = jasmine.createSpyObj('Validation', ['showErrors'])

        fixture = new EquipmentFormController(AddEvent, Validation);
    }));

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
        let idx = 0;
        let equipment = {
            id: 1,
            cancelled: false
        };

        let result = fixture.existing(equipment);
        expect(result).toBe(true);
    });

    it('should return false for existing() equipment', () => {
        let idx = 0;
        let equipment = {
            id: 1
        };

        let result = fixture.existing(equipment);
        expect(result).toBe(false);
    });

    it('should compose fieldName() correctly', () => {
        let name = 'field';
        let idx = 2
        let fieldName = name + '-' + idx;
        let fieldNameResult = fixture.fieldName(name, idx);
        expect(fieldName).toBe(fieldNameResult);
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

        let idx = 1;
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

        let idx = 1;
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
        let result = fixture.showError({});
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
        let result = fixture.showError({});
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
        let result = fixture.showError({});

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
        let result = fixture.showError({});

        expect(result).toBe(false);
    });

    it('should return true from showFieldError()', () => {
        Validation.showErrors.and.callFake((form, params) => {
            if (params == 'testField')
                return true;
        });
        let result = fixture.showFieldError({}, 'testField');

        expect(result).toBe(true);
    });

    it('should return false from showFieldError()', () => {
        Validation.showErrors.and.callFake((form, params) => {
            if (params == 'testField')
                return false;
        });
        let result = fixture.showFieldError({}, 'testField');

        expect(result).toBe(false);
    });

    it('should not undo() anything because equipment is undefined', () => {
        let idx = 3000;
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
        let idx = 1;
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
        let idx = 1;
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
        fixture.currentEquipment = [{equipment: {id: 27, name: 'Dat Equipment', quantity:42}},{equipment: {id: 21, name: 'Dat Equipment 2', quantity:2}}];
        fixture.toggleEquipmentRequired();
        expect(fixture.currentEquipment[0].equipment.name.id).toBe(0);
        expect(fixture.currentEquipment.length).toBe(1);
    });

    it('toggleEquipmentRequired should not clear current equipment if equipmentRequired is true', () => {
        fixture.equipmentRequired = true;
        fixture.currentEquipment = [{equipment: {id: 27, name: 'Dat Equipment', quantity:42}},{equipment: {id: 41, name: 'Dat Equipment 2', quantity:1}}];
        fixture.toggleEquipmentRequired();
        expect(fixture.currentEquipment[0].equipment.id).toBe(27);
        expect(fixture.currentEquipment.length).toBe(2);
    });
});