import CONSTANTS from 'crds-constants';

export default class AddRoomController {
    /* @ngInject */
  constructor($log, $rootScope, $modal, AddEvent, Lookup, Room) {
    this.log = $log;
    this.rootScope = $rootScope;
    this.modal = $modal;
    this.addEvent = AddEvent;
    this.lookup = Lookup;
    this.room = Room;
    this.equipmentList = [];
    this.roomError = false;
    this.viewReady = false;
    // this.selectedRooms = [];
    this.rooms = [];
    // this.chosenSite = ''; from binding
  }

  $onInit() {
    this.layouts = this.room.Layouts.query();
    if (this.addEvent.editMode) {
      this.lookup.query({ table: 'crossroadslocations' }, (locations) => {
        this.addEvent.eventData.event.congregation = _.find(locations, (l) => {
          return l.dp_RecordID === this.addEvent.eventData.event.congregation.dp_RecordID;
        });
      });
    }
    if (this.addEvent.eventData.event.congregation !== undefined) {
      this.chosenSite = this.addEvent.eventData.event.congregation.dp_RecordName;
      this.room.ByCongregation.query({
        congregationId: this.addEvent.eventData.event.congregation.dp_RecordID
      }, ((data) => {
        this.rooms = data;
        this.viewReady = true;
        this.roomData = _.filter(this.roomData, (r) => {
          if (r.name === undefined) {
            const tempRoom = _.find(data, (roo) => {
              return roo.id === r.id;
            });

            if (tempRoom) {
              r.name = tempRoom.name;
              return true;
            }

            return false;
          }

          return true;
        });
        this.room.Equipment.query({ congregationId: this.addEvent.eventData.event.congregation.dp_RecordID },
                    (data) => {
                      this.equipmentList = data;
                      _.forEach(this.roomData, (roomD) => {
                        roomD.equipment = this.mapEquipment(data, roomD.equipment);
                      });
                    });
      }));
     // this.viewReady = true;
      return;
    }
    this.log.error('The congregation was not passed in so we can\'t get the list of rooms or equipment');
    return;
  }

  isCancelled(currentRoom) {
    return _.has(currentRoom, 'cancelled') && currentRoom.cancelled;
  }

  removeRoomModal(room) {
    const modalInstance = this.modal.open({
      controller: 'RemoveRoomController',
      controllerAs: 'removeRoom',
      templateUrl: 'remove_room/remove_room.html',
      resolve: {
        items() {
          return room;
        }
      }
    });
    return modalInstance;
  }

  onAdd() {
    if (this.chosenRoom) {
            // is this room already added?
      const alreadyAdded = _.find(this.roomData, (r) => {
        return r.id === this.chosenRoom.id;
      });

      if (alreadyAdded) {
        if (alreadyAdded.cancelled) {
          alreadyAdded.cancelled = false;
        } else {
          this.rootScope.$emit('notify', this.rootScope.MESSAGES.allReadyAdded);
        }

        return;
      }

      this.roomData.push(this.chosenRoom);
      return;
    }

    this.rootScope.$emit('notify', this.rootScope.MESSAGES.chooseARoom);
  }

  mapEquipment(equipmentLookup, currentEquipmentList) {
    return _.map(currentEquipmentList, (current) => {
      if (current.equipment.name.quantity === undefined) {
        const found = _.find(equipmentLookup, (e) => {
          return e.id === current.equipment.name.id;
        });

        if (found) {
          current.equipment.name.quantity = found.quantity;
        }

        return current;
      }
    });
  }

  removeRoom(currentRoom) {
    this.log.debug(`remove room: ${currentRoom}`);
    const modalInstance = this.removeRoomModal(currentRoom);

    modalInstance.result.then(() => {
      if (!_.has(currentRoom, 'cancelled')) {
        this.roomData = _.filter(this.roomData, (r) => {
                    // only return elements that aren't currentRoom
          return r.id !== currentRoom.id;
        });
      } else {
        currentRoom.cancelled = true;
        _.each(currentRoom.equipment, (e) => {
          e.equipment.cancelled = true;
        });
      }
    },

            () => {
              if (!_.has(currentRoom, 'cancelled') && currentRoom.cancelled) {
                this.log.info('user doesn\'t want to delete this room...');
                currentRoom.cancelled = false;
              }
            });
  }

  showNoRoomsMessage() {
    return (!this.viewReady || this.rooms === undefined || this.rooms.length < 1);
  }

}

// (function() {
//   'use strict';

//   module.exports = AddRoom;

//   AddRoom.$inject = ['$log', '$rootScope', '$modal', 'AddEvent', 'Lookup', 'Room'];

//   function AddRoom($log, $rootScope, $modal, AddEvent, Lookup, Room) {
//     return {
//       restrict: 'E',
//       scope: {
//         roomData: '='
//       },
//       templateUrl: 'add_room/add_room.html',
//       controller: AddRoomController,
//       controllerAs: 'addRoom',
//       bindToController: true
//     };

//     function AddRoomController() {
//       var vm = this;
//       vm.choosenSite = choosenSite;
//       vm.equipmentList = [];
//       vm.isCancelled = isCancelled;
//       vm.layouts = Room.Layouts.query();
//       vm.onAdd = onAdd;
//       vm.removeRoom = removeRoom;
//       vm.roomError = false;
//       vm.showNoRoomsMessage = showNoRoomsMessage;
//       vm.viewReady = false;

//       activate();

//       //////////////////

    //   function activate() {
    //     if (AddEvent.editMode) {
    //       Lookup.query({ table: 'crossroadslocations' }, function(locations) {
    //         AddEvent.eventData.event.congregation = _.find(locations, function(l) {
    //           return l.dp_RecordID === AddEvent.eventData.event.congregation.dp_RecordID;
    //         });
    //       });
    //     }

    //     if (AddEvent.eventData.event.congregation !== undefined) {
    //       Room.ByCongregation.query({
    //         congregationId: AddEvent.eventData.event.congregation.dp_RecordID
    //       }, function(data) {
    //         vm.rooms = data;
    //         vm.roomData = _.filter(vm.roomData, function(r) {
    //           if (r.name === undefined) {
    //             var tempRoom = _.find(data, function(roo) {
    //               return roo.id === r.id;
    //             });

    //             if (tempRoom) {
    //               r.name = tempRoom.name;
    //               return true;
    //             }

    //             return false;
    //           }

    //           return true;
    //         });

    //         Room.Equipment.query({congregationId: AddEvent.eventData.event.congregation.dp_RecordID}, function(data) {
    //           vm.equipmentList = data;
    //           _.forEach(vm.roomData, function(roomD) {
    //             roomD.equipment = mapEquipment(data, roomD.equipment);
    //           });

    //           vm.viewReady = true;
    //         });
    //       });

    //       return;
    //     }

    //     $log.error('The congregation was not passed in so we can\'t get the list of rooms or equipment');
    //     return;
    //   }

//       function choosenSite() {
//         return AddEvent.eventData.event.congregation.dp_RecordName;
//       }

//       function isCancelled(currentRoom) {
//         return _.has(currentRoom, 'cancelled') && currentRoom.cancelled;
//       }

//       function mapEquipment(equipmentLookup, currentEquipmentList) {
//         return _.map(currentEquipmentList, function(current) {
//           if (current.equipment.name.quantity === undefined) {
//             var found = _.find(equipmentLookup, function(e) {
//               return e.id === current.equipment.name.id;
//             });

//             if (found) {
//               current.equipment.name.quantity = found.quantity;
//             }

//             return current;
//           }
//         });
//       }

//       function removeRoomModal(room) {
//         var modalInstance = $modal.open({
//           controller: 'RemoveRoomController as removeRoom',
//           templateUrl: 'remove_room/remove_room.html',
//           resolve: {
//             items: function() {
//               return room;
//             }
//           }
//         });
//         return modalInstance;
//       }

//       function onAdd() {
//         if (vm.choosenRoom) {
//           // is this room already added?
//           var alreadyAdded = _.find(vm.roomData, function(r) {
//             return r.id === vm.choosenRoom.id;
//           });

//           if (alreadyAdded) {
//             if (alreadyAdded.cancelled) {
//               alreadyAdded.cancelled = false;
//             } else {
//               $rootScope.$emit('notify', $rootScope.MESSAGES.allReadyAdded);
//             }

//             return;
//           }

//           vm.roomData.push(vm.choosenRoom);
//           return;
//         }

//         $rootScope.$emit('notify', $rootScope.MESSAGES.chooseARoom);
//       }

//       function removeRoom(currentRoom) {
//         $log.debug('remove room: ' + currentRoom);
//         var modalInstance = removeRoomModal(currentRoom);

//         modalInstance.result.then(function() {
//           if (!_.has(currentRoom, 'cancelled')) {
//             vm.roomData = _.filter(vm.roomData, function(r) {
//               // only return elements that aren't currentRoom
//               return r.id !== currentRoom.id;
//             });
//           } else {
//             currentRoom.cancelled = true;
//             _.each(currentRoom.equipment, function(e) {
//               e.equipment.cancelled = true;
//             });
//           }
//         },

//         function() {
//           if (!_.has(currentRoom, 'cancelled') && currentRoom.cancelled) {
//             $log.info('user doesn\'t want to delete this room...');
//             currentRoom.cancelled = false;
//           }
//         });
//       }

//       function showNoRoomsMessage() {
//         return (!vm.viewReady || vm.rooms === undefined || vm.rooms.length < 1);
//       }
//     }
//   }

// })();