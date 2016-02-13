(function(){
  'use strict';

  var MODULES = require('crds-constants').MODULES;

  require('./common/layout.html');
  require('./common/header.html');
  require('./login/welcome.html');
  require('./host/host.html');
  require('./host/review.html');
  require('./summary/summary.html');

  angular.module('crossroads.group_finder', [MODULES.CORE, MODULES.COMMON])
    .config(require('./group_finder.routes'))
    .constant('SERIES',                     require('./group_finder.constants').SERIES)
    .constant('GROUP_TYPES',                require('./group_finder.constants').GROUP_TYPES)
    .directive('questions',                 require('./directives/questions/questions.directive'))
    .directive('question',                  require('./directives/question/question.directive'))
    .directive('groupcard',                 require('./directives/group_card/group_card.directive'))
    .directive('membercard',                require('./directives/member_card/member_card.directive'))
    .filter('humanize',                     require('./filters/humanize.filter.js'))
    .factory('Group',                       require('./services/group_finder.service'))
    .factory('GroupInfo',                   require('./services/group_info.service'))
    .factory('GroupMember',                 require('./services/group_member.service'))
    .factory('Profile',                     require('./services/profile.service'))
    .factory('Person',                      require('./services/person.service'))
    .factory('User',                        require('./services/user.service'))
    .factory('Email',                       require('./services/email.service'))
    .service('Responses',                   require('./services/response.service'))
    .service('QuestionService',             require('./services/group_questions.service'))
    .service('GroupQuestionService',        require('./services/group_questions.service'))
    .service('ParticipantQuestionService',  require('./services/participant_questions.service'))
    .controller('GroupFinderCtrl',          require('./group_finder.controller'))
    .controller('LoginCtrl',                require('./login/login.controller'))
    .controller('HostCtrl',                 require('./host/host.controller'))
    .controller('HostReviewCtrl',           require('./host/host_review.controller'))
    .controller('SummaryCtrl',              require('./summary/summary.controller'))
    ;

  require('./dashboard');
  require('./join');

})();
