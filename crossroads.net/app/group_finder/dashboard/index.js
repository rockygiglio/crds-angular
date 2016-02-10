(function() {
  'use strict';

  var MODULES = require('crds-constants');

  require('./templates/groups.html');
  require('./templates/resources.html');
  require('./templates/group_contact_modal.html');
  require('./dashboard.html');

  angular.module('crossroads.group_finder')
    .controller('GroupContactCtrl', require('./group_contact.controller.js'))
    .controller('DashboardCtrl', require('./dashboard.controller.js'));
})();
