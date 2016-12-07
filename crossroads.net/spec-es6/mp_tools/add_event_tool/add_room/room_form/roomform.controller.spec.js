import CONSTANTS from 'crds-constants';
import RoomFormController from '../../../../../app/mp_tools/add_event_tool/add_room/room_form/roomForm.controller';

describe('AddEventTool EquipmentForm', () => {
    let fixture,
        AddEvent,
        Validation

    beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

    beforeEach(inject(($injector) => {
        AddEvent = {};
        Validation = jasmine.createSpyObj('Validation', ['showErrors'])

        fixture = new RoomFormController(AddEvent, Validation);
    }));

    it('should return true for existing() room', () => {
        fixture.currentRoom = {
            id: 1,
            cancelled: false
        };

        let result = fixture.existing();
        expect(result).toBe(true);
    });

    it('should return true for existing() room because even though cancelled is true', () => {
        fixture.currentRoom = {
            id: 1,
            cancelled: true
        };

        let result = fixture.existing();
        expect(result).toBe(true);
    });

    it('should return false for existing() room because cancelled is undefined', () => {
        fixture.currentRoom = {
            id: 1
        };

        let result = fixture.existing();
        expect(fixture.currentRoom.cancelled).toBe(undefined);
        expect(result).toBe(false);
    });

    it('should return true for isCancelled()', () => {
        fixture.currentRoom = {
            id: 1,
            cancelled: true
        };
        let result = fixture.isCancelled();
        expect(result).toBe(true);
    });

    it('should return false for isCancelled() because cancelled is false', () => {
        fixture.currentRoom = {
            id: 1,
            cancelled: false
        };
        let result = fixture.isCancelled();
        expect(result).toBe(false);
    });

    it('should return false for isCancelled() because cancelled doesnt exist', () => {
        fixture.currentRoom = {
            id: 1
        };
        let result = fixture.isCancelled();
        expect(result).toBe(false);
    });

    it('removeRoom should be called from remove()', () => {
        fixture.removeRoom = () => { return; };
        spyOn(fixture, 'removeRoom');
        fixture.remove();
        expect(fixture.removeRoom).toHaveBeenCalled();
    });
});