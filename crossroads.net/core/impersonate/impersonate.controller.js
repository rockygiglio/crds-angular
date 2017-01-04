(function() {

  'use strict';
  module.exports = ImpersonateController;

  ImpersonateController.$inject = ['$rootScope', '$scope', '$log', 'AuthService', '$state', 'Session', '$resource'];

  function ImpersonateController($rootScope, $scope, $log, AuthService, $state, Session, $resource) {
    $log.debug('Inside ImpersonateController');

    //$scope.impersonateUsers = $resource(__API_ENDPOINT__ + 'api/user').query();

    $resource(__API_ENDPOINT__ + 'api/user').query(function(data) {
      $scope.impersonateUsers = data;
    })

    $scope.changeUser = function(newUser) {
        $scope.impersonateUser = newUser;
    }

  }
})();
