(function(){
  'use strict';

  var MODULES = require('crds-constants').MODULES;

  require('./common/layout.html');
  require('./common/header.html');
  require('./summary/summary.html');

  angular.module(MODULES.GROUP_FINDER, [MODULES.CORE, MODULES.COMMON])
    .config(require('./group_finder.routes'))
    .constant('SERIES',             require('./group_finder.constants').SERIES)
    .constant('GROUP_TYPES',        require('./group_finder.constants').GROUP_TYPES)
    .directive('questions',         require('./directives/questions/questions.directive'))
    .directive('question',          require('./directives/question/question.directive'))
    .directive('groupcard',         require('./directives/group_card/group_card.directive'))
    .directive('membercard',        require('./directives/member_card/member_card.directive'))
    .filter('humanize',             require('./filters/humanize.filter.js'))
    .controller('GroupFinderCtrl',  require('./group_finder.controller'))
    .controller('SummaryCtrl',      require('./summary/summary.controller'))
    ;

  require('./services');
  require('./dashboard');
  require('./host');
  require('./join');

})();
