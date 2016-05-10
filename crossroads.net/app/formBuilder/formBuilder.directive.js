(function() {
  'use strict';

  module.exports = FormBuilder;

  FormBuilder.$inject = ['$controller'];

  function FormBuilder($controller) {
    return {
      restrict: 'E',
      scope: {
        form: '=?',
        cmsPage: '=?'
      },
      templateUrl: 'templates/formBuilder.html',
      controller: getController,
      controllerAs: 'formBuilder',
      bindToController: true
    };

    function getController($scope, $attrs, $transclude) {
      var controllerName = getControllerName($scope);
      return $controller(controllerName, $scope, $attrs, $transclude);
    }

    function getControllerName($scope) {
      var controllerName = 'FormBuilderDefaultCtrl';
      if ($scope.formBuilder && $scope.formBuilder.cmsPage && $scope.formBuilder.cmsPage.controllerName) {
        controllerName = $scope.formBuilder.cmsPage.controllerName;
      }

      return controllerName;
    }
  }

})();
