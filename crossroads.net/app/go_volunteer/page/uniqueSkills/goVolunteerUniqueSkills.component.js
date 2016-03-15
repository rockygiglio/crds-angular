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

      vm.list = [
        { title: "Jack of all trades but not a professional" },
        { title: "I can design and paint a mural" },
        { title: "I like to entertain kids" },
        { title: "I'm a professional carpenter" },
        { title: "I'm a professional carpet installer" },
        { title: "I'm a professional contractor" },
        { title: "I'm a professional electrician" },
        { title: "I'm a professional landscaper" },
        { title: "I'm a professional painter" },
        { title: "I'm a professional plumber" }
      ];

    }
  }

})();
