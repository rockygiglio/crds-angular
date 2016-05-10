(function() {
  'use strict';

  module.exports = FormBuilder;

  FormBuilder.$inject = ['$controller'];

  function FormBuilder($controller) {
    return {
      restrict: 'E',
      scope: {
        form: '=?'
      },
      templateUrl: 'templates/formBuilder.html',
      controller: function($scope, $attrs, $transclude) {
        var controllerName = 'FormBuilderDefaultCtrl';
        // TODO: Need something in form builder to tell us the controller to use
        if ($scope.formBuilder.form[0].name === "EditableFormStep_cf42d") {
          controllerName = 'UndividedFacilitatorCtrl';
        }

        return $controller(controllerName, $scope, $attrs, $transclude);
      },

      controllerAs: 'formBuilder',
      bindToController: true
    };
  }

})();
