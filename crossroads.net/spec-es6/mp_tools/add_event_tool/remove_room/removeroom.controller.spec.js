import CONSTANTS from 'crds-constants';
import RemoveRoomController from '../../../../app/mp_tools/add_event_tool/remove_room/removeRoom.controller';

describe('AddEventTool RemoveRoom', () => {
    let fixture,
        modalInstance,
        items

    beforeEach(angular.mock.module(CONSTANTS.MODULES.MPTOOLS));

    beforeEach(inject(($injector) => {
        modalInstance = jasmine.createSpyObj('modalInstance', ['close', 'dismiss'])
        items = {};

        fixture = new RemoveRoomController(modalInstance, items);
    }));

    it('should call modalInstance.close from ok()', () => {
        fixture.ok();
        expect(fixture.modalInstance.close).toHaveBeenCalled();
    });

    it('should call modalInstance.dismiss(cancel) from cancel()', () => {
        fixture.cancel();
        expect(fixture.modalInstance.dismiss).toHaveBeenCalledWith("cancel");
    });
});