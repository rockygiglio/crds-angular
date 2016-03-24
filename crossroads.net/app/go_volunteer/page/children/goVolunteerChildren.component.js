(function() {
  'use strict';

  module.exports = GoVolunteerChildren;

  GoVolunteerChildren.$inject = [];

  function GoVolunteerChildren() {
    return {
      restrict: 'E',
      scope: {
        onSubmit: '&' 
      },
      bindToController: true,
      controller: GoVolunteerChildrenController,
      controllerAs: 'goChildren',
      templateUrl: 'children/goVolunteerChildren.template.html'
    };

    function GoVolunteerChildrenController() {
      var vm = this;
      vm.submit = submit;

      function submit(childrenServing) {
        if (childrenServing) {
          vm.onSubmit({nextState: 'children-count'});
        } else {
          vm.onSubmit({nextState: 'group-connector'});
        }
      }

    }
  }

})();
