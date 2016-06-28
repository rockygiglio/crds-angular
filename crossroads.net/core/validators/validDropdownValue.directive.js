(function() {
  'use strict';

  module.exports = ValidDropdownValue;

  ValidDropdownValue.$inject = [];

  function ValidDropdownValue() {
    return {
      restrict: 'A',
      require: 'ngModel',
      scope: {
        validDropdownValue: '='
      },
      link: link
    };

    function link(scope, ele, attr, ctrl) {
      if (ctrl) {
        ctrl.$validators.required = function(val) {
          if (!val) {
            return false;
          }

          return _.includes(scope.validDropdownValue, val);
        };
      }
    }
  }

})();
