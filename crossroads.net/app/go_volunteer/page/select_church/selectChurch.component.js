(function() {
  'use strict';

  module.exports = selectChurch;

  selectChurch.$inject = [];

  function selectChurch() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: selectChurchController,
      controllerAs: 'selectChurch',
      templateUrl: 'select_church/selectChurch.template.html'
    };

    function selectChurchController() {
      var vm = this;
    }
  }

})();
