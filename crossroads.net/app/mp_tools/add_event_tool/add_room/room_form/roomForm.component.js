import controller from './roomForm.controller';
import template from './room_form.html';

export default function RoomFormComponent() {
  return {
    restrict: 'E',
    bindings: {
        currentRoom: '=',
        layouts: '=',
        equipmentLookup: '=',
        removeRoom: '&',
        editMode: '='
    },
    template: template,
    controller: controller,
    controllerAs: 'room'
  };
};