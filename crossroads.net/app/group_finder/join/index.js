(function() {
  'use strict';

  var MODULES = require('crds-constants');

  require('./join.html');
  require('./review.html');

  angular.module('crossroads.group_finder')
    .controller('JoinCtrl', require('./join.controller'))
    .controller('JoinReviewCtrl', require('./join_review.controller'));
})();
