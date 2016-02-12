(function(){
  'use strict';

  var MODULES = require('crds-constants').MODULES;

  require('./common/layout.html');
  require('./login/welcome.html');
  require('./host/host.html');
  require('./host/review.html');
  require('./summary/summary.html');

  angular.module(MODULES.GROUP_FINDER, [MODULES.CORE, MODULES.COMMON])
    .config(require('./group_finder.routes'))
    .constant('SERIES',             require('./group_finder.constants'))
    .directive('questions',         require('./directives/questions/questions.directive'))
    .directive('question',          require('./directives/question/question.directive'))
    .directive('groupcard',         require('./directives/group_card.directive'))
    .filter('humanize',             require('./filters/humanize.filter.js'))
    .controller('LoginCtrl',        require('./login/login.controller'))
    .controller('HostCtrl',         require('./host/host.controller'))
    .controller('HostReviewCtrl',   require('./host/host_review.controller'))
    .controller('SummaryCtrl',      require('./summary/summary.controller'))
    ;

  require('./services');
  require('./dashboard');
  require('./join');

})();
