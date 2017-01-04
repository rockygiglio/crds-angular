
/* eslint-disable no-param-reassign */
(() => {
  function ImpersonateController($rootScope, $scope, $log, AuthService, $state, Session, $resource) {
    $log.debug('Inside ImpersonateController');

    // $scope.impersonateUsers = $resource(`${__API_ENDPOINT__}api/user`).query();

    $resource(`${__API_ENDPOINT__}api/user`).query((data) => {
      $scope.impersonateUsers = data;
    });

    $scope.changeUser = (newUser) => {
      $scope.impersonateUser = newUser;
    };
  }

  module.exports = ImpersonateController;

  ImpersonateController.$inject = [
    '$rootScope',
    '$scope',
    '$log',
    'AuthService',
    '$state',
    'Session',
    '$resource'
  ];
})();
