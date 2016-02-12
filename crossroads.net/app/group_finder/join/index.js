(function() {
  'use strict';

  var MODULES = require('crds-constants');

  require('./join.html');
  require('./review.html');
  require('./templates/upsell.html');
  require('./templates/results.html');

  angular.module('crossroads.group_finder')
    .controller('JoinCtrl', require('./join.controller'))
    .controller('JoinReviewCtrl', require('./join_review.controller'));
})();
