(function() {
  'use strict';

  var constants = require('crds-constants');

  require('./join.html');
  require('./join_questions.html');
  require('./join_review.html');
  require('./templates/upsell.html');
  require('./templates/results.html');

  angular.module(constants.MODULES.GROUP_FINDER)
    .config(require('./join.routes'))
    .controller('JoinCtrl', require('./join.controller'))
    .controller('JoinQuestionsCtrl', require('./join_questions.controller'))
    .controller('JoinReviewCtrl', require('./join_review.controller'))
    ;

})();
