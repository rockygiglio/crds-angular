(function() {
  'use strict';

  var constants = require('crds-constants');

  require('./join.html');
  require('./review.html');
  require('./templates/upsell.html');
  require('./templates/results.html');
  require('./templates/contact.html');

  angular.module(constants.MODULES.GROUP_FINDER)
    .controller('JoinCtrl', require('./join.controller'))
    .controller('JoinReviewCtrl', require('./join_review.controller'));
})();
