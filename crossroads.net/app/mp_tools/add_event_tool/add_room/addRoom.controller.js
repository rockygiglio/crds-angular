import CONSTANTS from 'crds-constants';

export default class AddRoomController {
    /* @ngInject */
    constructor($log, $rootScope, AddEvent, Lookup, Room) {
        this.log = $log;
        this.rootScope = $rootScope;
        this.addEvent = AddEvent;
        this.lookup = Lookup;
        this.room = Room;
        this.equipmentList = [];
        this.roomError = false;
        this.viewReady = false;
        this.roomData = [];
    }

    $onInit() {
        this.layouts = this.room.Layouts.query();
        if (this.addEvent.editMode) {
            Lookup.query({ table: 'crossroadslocations' }, function (locations) {
                this.addEvent.eventData.event.congregation = _.find(locations, function (l) {
                    return l.dp_RecordID === this.addEvent.eventData.event.congregation.dp_RecordID;
                });
            });
        }
        if (this.addEvent.eventData.event.congregation !== undefined) {
            this.room.ByCongregation.query({
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
                }),
                this.room.Equipment.query({ congregationId: this.addEvent.eventData.event.congregation.dp_RecordID }, function (data) {
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
        if (this.choosenRoom) {
            // is this room already added?
            var alreadyAdded = _.find(this.roomData, function (r) {
                return r.id === this.choosenRoom.id;
            });

            if (alreadyAdded) {
                if (alreadyAdded.cancelled) {
                    alreadyAdded.cancelled = false;
                } else {
                    $rootScope.$emit('notify', $rootScope.MESSAGES.allReadyAdded);
                }

                return;
            }

            this.roomData.push(this.choosenRoom);
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
                this.roomData = _.filter(this.roomData, function (r) {
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
        return (!this.viewReady || this.rooms === undefined || this.rooms.length < 1);
    }

};