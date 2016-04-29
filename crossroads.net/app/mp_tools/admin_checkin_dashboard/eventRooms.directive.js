(function() {
  'use strict';

  module.exports = EventRooms;

  EventRooms.$inject = ['$log', 'Focus'];

  function EventRooms($log, Focus) {
    return {
      restrict: 'EA',
      replace: true,
      scope: {
        rooms: '=',
      },
      templateUrl: 'templates/eventRooms.html',
      link: link
    };

    function link(scope, element, attrs) {
      scope.ratio = ratio;
      scope.editRoom = editRoom;
      scope.editCheckinAllowed = editCheckinAllowed;

      /////////////////////////////////
      ////// IMPLMENTATION DETAILS ////
      /////////////////////////////////

      function update() {
      }

      function ratio(room) {
        if (room.volunteers == undefined || room.volunteers == 0) {
          return 'N/A';
        } else if (room.participantsAssigned % room.volunteers == 0 && room.participantsAssigned != 0) {
          return room.participantsAssigned / room.volunteers + '/1';
        }

        return room.participantsAssigned + '/' + room.volunteers;
      }

      function editRoom(room, indx) {
        room.editLabel = !room.editLabel;

        if (room.editLabel) {
          Focus.focus('label' + indx);
        }
      }

      function editCheckinAllowed(room, indx) {
        room.editCheckinAllowed = !room.editCheckinAllowed;

        if (room.editCheckinAllowed) {
          Focus.focus('checkinAllowed' + indx);
        }
      }
    }
  }
})();
