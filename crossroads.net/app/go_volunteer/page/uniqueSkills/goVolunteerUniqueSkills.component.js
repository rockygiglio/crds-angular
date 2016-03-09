(function() {
  'use strict';

  module.exports = GoVolunteerUniqueSkills;

  GoVolunteerUniqueSkills.$inject = [];

  function GoVolunteerUniqueSkills() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerUniqueSkillsController,
      controllerAs: 'goUniqueSkills',
      templateUrl: 'uniqueSkills/goVolunteerUniqueSkills.template.html'
    };

    function GoVolunteerUniqueSkillsController() {
      var vm = this;

    }
  }

})();
