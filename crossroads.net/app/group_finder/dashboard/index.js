(function() {
  'use strict';

  var constants = require('crds-constants');

  require('./templates/groups.html');
  require('./templates/resources.html');
  require('./templates/group_contact_modal.html');
  require('./dashboard.html');
  require('./group_detail.html');

  angular.module(constants.MODULES.GROUP_FINDER)
    .controller('GroupContactCtrl', require('./group_contact.controller.js'))
    .controller('GroupDetailCtrl', require('./group_detail.controller.js'))
    .controller('DashboardCtrl', require('./dashboard.controller.js'));
})();
