(function() {
  'use strict';

  module.exports = function() {

    return {
      restrict: 'A',
      require: 'ngModel',
      scope: true,
      link: function(scope, element, attrs, ngModel) {
        ngModel.$validators.endTime = function(value) {
          if (value === undefined || value === null || value === '') {
            return false;
          }

          var start = moment(scope.startTime);
          return start.isBefore(value);
        };
      }

    };
  };
})();
