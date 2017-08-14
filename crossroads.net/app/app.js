// require('./group_finder');

require('./childcare');
require('./mp_tools');
require('./group_tool');
require('./live_stream');
require('ui-select/dist/select.css');
require('./invoices/invoices.module');

(function () {
  'use strict()';

  var constants = require('./constants');
  angular.module(constants.MODULES.CROSSROADS, [
    constants.MODULES.CHILDCARE_DASHBOARD,
    constants.MODULES.CORE,
    constants.MODULES.COMMON,
    constants.MODULES.CORKBOARD,
    constants.MODULES.FORM_BUILDER,
    constants.MODULES.GIVE,
    constants.MODULES.GO_VOLUNTEER,
    constants.MODULES.MEDIA,
    constants.MODULES.LIVE_STREAM,
    constants.MODULES.MPTOOLS,
    constants.MODULES.MY_SERVE,
    constants.MODULES.GROUP_TOOL,
    constants.MODULES.PROFILE,
    constants.MODULES.SEARCH,
    constants.MODULES.SIGNUP,
    constants.MODULES.TRIPS,
    constants.MODULES.CAMPS,
    constants.MODULES.INVOICES,
    constants.MODULES.WAIVERS
  ]);

  angular.module(constants.MODULES.CROSSROADS)
    .config(require('./routes'))
    .config(require('./routes.content'))
    .config(['$logProvider', function($logProvider) {
      // disable debug log in prod
      if (!__CRDS_ENV__) {
        $logProvider.debugEnabled(false);
      }
    }]);

  require('./corkboard');
  require('./events');
  require('./signup');
  require('./styleguide');
  require('./superbowl');
  require('./thedaily');
  require('./gotrips');
  require('./my_serve');
  require('./brave_at_home');
  require('./volunteer_signup');
  require('./volunteer_application');
  require('./giving_history');
  require('./leaveyourmark');
})();
