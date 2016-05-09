(function() {
  'use strict';

  module.exports = EventRooms;

  EventRooms.$inject = ['$log', '$rootScope', 'Focus', 'AdminCheckinDashboardService'];

  function EventRooms($log, $rootScope, Focus, AdminCheckinDashboardService) {
    return {
      restrict: 'EA',
      replace: true,
      scope: {
        eventId: '=',
        rooms: '='
      },
      templateUrl: 'templates/eventRooms.html',
      link: link
    };

    function link(scope, element, attrs) {
      scope.ratio = ratio;
      scope.editRoom = editRoom;
      scope.updateRoom = updateRoom;
      scope.update = update;

      /////////////////////////////////
      ////// IMPLMENTATION DETAILS ////
      /////////////////////////////////

      function update(indx) {
        return AdminCheckinDashboardService.checkinDashboard.update({eventId: scope.eventId}, scope.rooms[indx]).$promise.then(function(result) {
                $rootScope.$emit('notify', $rootScope.MESSAGES.eventUpdateSuccess);
                scope.rooms[indx] = result;
              },

              function(err) {
                $log.error(err);
                $rootScope.$emit('notify', $rootScope.MESSAGES.eventToolProblemSaving);
              });
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
        if (room.editLabel) {
          update(indx).then(function() {
              room.editLabel = !room.editLabel;
            });
        } else {
          room.editLabel = !room.editLabel;
          Focus.focus('label' + indx);
        }
      }

      function updateRoom(indx) {
        update(indx);
      }
    }
  }
})();
