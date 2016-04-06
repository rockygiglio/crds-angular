require('./group_finder');

require('./childcare');
require('./mp_tools');
require('../lib/select.css');

(function() {
  'use strict()';

  var MODULE = 'crossroads';
  var constants = require('./constants');

  angular.module(constants.MODULES.CROSSROADS, [
      constants.MODULES.CHILDCARE,
      constants.MODULES.CORE,
      constants.MODULES.COMMON,
      constants.MODULES.GIVE,
      constants.MODULES.GO_VOLUNTEER,
      constants.MODULES.GROUP_FINDER,
      constants.MODULES.MEDIA,
      constants.MODULES.MPTOOLS,
      constants.MODULES.PROFILE,
      constants.MODULES.SEARCH,
      constants.MODULES.SIGNUP,
      constants.MODULES.TRIPS,
   ]);

  angular.module(constants.MODULES.CROSSROADS).config(require('./routes'));
    require('./events');
  require('./signup');
  require('./styleguide');
  require('./superbowl');
  require('./thedaily');
  require('./explore');
  require('./gotrips');
  require('./my_serve');
  require('./brave_at_home');
  require('./volunteer_signup');
  require('./volunteer_application');
  require('./giving_history');

})();
