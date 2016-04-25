(function() {
  'use strict()';

  var MODULE = 'crossroads.mptools';

  require('./templates/adminCheckinDashboard.html');
  require('./templates/eventRooms.html');

  var app = angular.module(MODULE);
  app.controller('AdminCheckinDashboardController', require('./adminCheckinDashboard.controller'));
  app.directive('EventRooms', require('./eventRooms.directive'));

})();
