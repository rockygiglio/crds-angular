(function() {
  'use strict';

  var MODULES = require('crds-constants');

  require('./dashboard.html');
  require('./group_detail.html');
  require('./templates/dashboard_header.html');
  require('./templates/groups.html');
  require('./templates/group_resources.html');
  require('./templates/group_contact_modal.html');

  angular.module('crossroads.group_finder')
    .controller('GroupContactCtrl', require('./group_contact.controller.js'))
    .controller('GroupDetailCtrl', require('./group_detail.controller.js'))
    .controller('DashboardCtrl', require('./dashboard.controller.js'));
})();
