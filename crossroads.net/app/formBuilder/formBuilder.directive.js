(function() {
  'use strict';

  module.exports = FormBuilder;

  FormBuilder.$inject = [];

  function FormBuilder() {
    return {
      restrict: 'E',
      scope: {
        form: '=?'
      },
      templateUrl: 'templates/formBuilder.html',
      controller: FormBuilderController,
      controllerAs: 'formBuilder',
      bindToController: true
    };

    function FormBuilderController() {
      var vm = this;
    }
  }

})();
