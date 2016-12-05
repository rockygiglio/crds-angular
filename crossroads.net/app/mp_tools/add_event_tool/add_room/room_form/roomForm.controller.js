import CONSTANTS from 'crds-constants';

export default class RoomController {
    /* @ngInject */
    constructor(AddEvent, Validation) {
        this.addEvent = AddEvent;
        this.validation = Validation;
    }

    $onInit() {
        if (this.currentRoom.equipment === undefined) {
            this.currentRoom.equipment = [];
        }
    }

    existing() {
        if (_.has(this.currentRoom, 'cancelled')) {
            return this.currentRoom.cancelled;
        }

        return false;
    }

    isCancelled() {
        return existing() && this.currentRoom.cancelled;
    }

    remove() {
        this.removeRoom();
    }
}



// (function() {
//   'use strict';

//   module.exports = RoomForm;

//   RoomForm.$inject = ['AddEvent', 'Validation'];

//   function RoomForm(AddEvent, Validation) {
// return {
//   restrict: 'E',
//   scope: {
//     currentRoom: '=',
//     layouts: '=',
//     equipmentLookup: '=',
//     removeRoom: '&',
//     editMode: '='
//   },
//   templateUrl: 'room_form/roomForm.html',
//   bindToController: true,
//   controller: RoomController,
//   controllerAs: 'room'
// };

// function RoomController() {
//     var vm = this;
//     vm.existing = existing;
//     vm.isCancelled = isCancelled;
//     vm.remove = remove;
//     vm.validation = Validation;

//     activate();

//     ////////////////////

//     function activate() {

//         if (vm.currentRoom.equipment === undefined) {
//             vm.currentRoom.equipment = [];
//         }
//     }

//     function existing() {
//         if (_.has(vm.currentRoom, 'cancelled')) {
//             return vm.currentRoom.cancelled;
//         }

//         return false;
//     }

//     function isCancelled() {
//         return existing() && vm.currentRoom.cancelled;
//     }

//     function remove() {
//         vm.removeRoom();
//     }

// }
//   }
// })();
