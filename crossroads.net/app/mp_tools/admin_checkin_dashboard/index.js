(function() {
  'use strict()';

  var MODULE = 'crossroads.mptools';

  require('./templates/adminCheckinDashboard.html');
  require('./templates/eventRooms.html');

  var app = angular.module(MODULE);
  app.directive('eventRooms', require('./eventRooms.directive'));
  app.factory('Focus', require('./focus.service'));
  app.controller('AdminCheckinDashboardController', require('./adminCheckinDashboard.controller'));

})();
