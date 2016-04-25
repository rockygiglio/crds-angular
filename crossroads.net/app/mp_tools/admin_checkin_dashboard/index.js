(function() {
  'use strict()';

  var MODULE = 'crossroads.mptools';

  require('./templates/adminCheckinDashboard.html');

  var app = angular.module(MODULE);
  app.controller('AdminCheckinDashboardController', require('./adminCheckinDashboard.controller'));

})();
