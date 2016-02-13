(function() {
  'use strict';

  var constants = require('crds-constants');

  angular.module(constants.MODULES.GROUP_FINDER)
    .factory('GroupInfo',           require('./group_info.service'))
    .factory('Profile',             require('./profile.service'))
    .factory('Person',              require('./person.service'))
    .factory('Email',               require('./email.service'))
    .service('Responses',           require('./response.service'))
    .service('QuestionService',     require('./questions.service'))
    .service('GroupQuestionService',     require('./group_questions.service'))
    .service('ParticipantQuestionService',     require('./participant_questions.service'));

})();
