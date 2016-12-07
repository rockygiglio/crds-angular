import controller from './removeRoom.controller';
import template from './remove_room.html';

export default function RemoveRoomComponent() {
  return {
    restrict: 'E',
    template,
    controller,
    controllerAs: 'removeRoom'
  };
};