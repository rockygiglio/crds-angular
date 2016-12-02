import CONSTANTS from 'crds-constants';

export default class AddRoomController {
    /* @ngInject */
    constructor($log, $rootScope, AddEvent, Lookup, Room, StaffContact, Validation) {
        this.log = $log;
        this.rootScope = $rootScope;
        this.addEvent = AddEvent;
        this.lookup = Lookup;
        this.room = Room;
        this.staffContact = StaffContact;
        this.validation = Validation;
        this.equipmentList = [];
        this.layouts = Room.Layouts.query();
        this.roomError = false;
        this.viewReady = false;
    }

    $onInit() {
        if (this.addEvent.editMode) {
            Lookup.query({ table: 'crossroadslocations' }, function (locations) {
                this.addEvent.eventData.event.congregation = _.find(locations, function (l) {
                    return l.dp_RecordID === this.addEvent.eventData.event.congregation.dp_RecordID;
                });
            });
        }
        if (this.addEvent.eventData.event.congregation !== undefined) {
            Room.ByCongregation.query({
                congregationId: this.addEvent.eventData.event.congregation.dp_RecordID
            }, function (data) {
                this.roomData = _.filter(this.roomData, function (r) {
                    if (r.name === undefined) {
                        var tempRoom = _.find(data, function (roo) {
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

                Room.Equipment.query({ congregationId: this.addEvent.eventData.event.congregation.dp_RecordID }, function (data) {
                    this.equipmentList = data;
                    _.forEach(this.roomData, function (roomD) {
                        roomD.equipment = mapEquipment(data, roomD.equipment);
                    });

                    this.viewReady = true;
                });
            });

            return;
        }

        this.log.error('The congregation was not passed in so we can\'t get the list of rooms or equipment');
    };

    chosenSite() {
        return AddEvent.eventData.event.congregation.dp_RecordName;
    }

    isCancelled(currentRoom) {
        return _.has(currentRoom, 'cancelled') && currentRoom.cancelled;
    }

    removeRoomModal(room) {
        var modalInstance = $modal.open({
            controller: 'RemoveRoomController as removeRoom',
            templateUrl: 'remove_room/remove_room.html',
            resolve: {
                items: function () {
                    return room;
                }
            }
        });
        return modalInstance;
    }

    onAdd() {
        if (vm.choosenRoom) {
            // is this room already added?
            var alreadyAdded = _.find(vm.roomData, function (r) {
                return r.id === vm.choosenRoom.id;
            });

            if (alreadyAdded) {
                if (alreadyAdded.cancelled) {
                    alreadyAdded.cancelled = false;
                } else {
                    $rootScope.$emit('notify', $rootScope.MESSAGES.allReadyAdded);
                }

                return;
            }

            vm.roomData.push(vm.choosenRoom);
            return;
        }

        $rootScope.$emit('notify', $rootScope.MESSAGES.chooseARoom);
    }

    mapEquipment(equipmentLookup, currentEquipmentList) {
        return _.map(currentEquipmentList, function (current) {
            if (current.equipment.name.quantity === undefined) {
                var found = _.find(equipmentLookup, function (e) {
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
        $log.debug('remove room: ' + currentRoom);
        var modalInstance = removeRoomModal(currentRoom);

        modalInstance.result.then(function () {
            if (!_.has(currentRoom, 'cancelled')) {
                vm.roomData = _.filter(vm.roomData, function (r) {
                    // only return elements that aren't currentRoom
                    return r.id !== currentRoom.id;
                });
            } else {
                currentRoom.cancelled = true;
                _.each(currentRoom.equipment, function (e) {
                    e.equipment.cancelled = true;
                });
            }
        },

            function () {
                if (!_.has(currentRoom, 'cancelled') && currentRoom.cancelled) {
                    $log.info('user doesn\'t want to delete this room...');
                    currentRoom.cancelled = false;
                }
            });
    }

    showNoRoomsMessage() {
        return (!vm.viewReady || vm.rooms === undefined || vm.rooms.length < 1);
    }

};