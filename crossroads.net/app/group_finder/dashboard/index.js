(function() {
  'use strict';

  var MODULES = require('crds-constants');

  require('./templates/groups.html');
  require('./templates/resources.html');
  require('./dashboard.html');

  angular.module('crossroads.group_finder')
    .controller('DashboardCtrl', require('./dashboard.controller.js'));
})();
