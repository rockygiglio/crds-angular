(function(){
  'use strict';

  var MODULES = require('crds-constants').MODULES;

  require('./common/layout.html');
  require('./login/welcome.html');
  require('./host/host.html');
  require('./host/review.html');
  require('./dashboard/dashboard.html');
  require('./summary/summary.html');

  angular.module('crossroads.group_finder', [MODULES.CORE, MODULES.COMMON])
    .config(require('./group_finder.routes'))
    .constant('SERIES',             require('./group_finder.constants'))
    .directive('questions',         require('./questions/questions.directive'))
    .directive('question',          require('./question/question.directive'))
    .factory('Group',               require('./services/group_finder.service'))
    .factory('GroupMember',         require('./services/group_member.service'))
    .factory('Profile',             require('./services/profile.service'))
    .service('Responses',           require('./services/response.service'))
    .service('QuestionService',     require('./services/group_questions.service'))
    .controller('LoginCtrl',        require('./login/login.controller'))
    .controller('DashboardCtrl',    require('./dashboard/dashboard.controller'))
    .controller('HostCtrl',         require('./host/host.controller'))
    .controller('HostReviewCtrl',   require('./host/host_review.controller'))
    .controller('SummaryCtrl',      require('./summary/summary.controller'))
    ;

})();
