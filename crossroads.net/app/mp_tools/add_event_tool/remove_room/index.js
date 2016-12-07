import CONSTANTS from 'crds-constants';
import './remove_room.html';
import removeRoomComponent from './removeRoom.component';

export default angular
.module(CONSTANTS.MODULES.MPTOOLS)
.component('removeRoom', removeRoomComponent());
