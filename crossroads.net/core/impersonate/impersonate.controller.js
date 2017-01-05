
/* eslint-disable no-param-reassign */
(() => {
  function ImpersonateController($rootScope, $scope, $log, AuthService, $state, Session, $resource) {

    console.log($rootScope);

    $resource(`${__API_ENDPOINT__}api/user`).query((data) => {
      $scope.impersonateUsers = data;
    });
    
    $scope.impersonate = () =>{
      console.log('Impersonating ' + $scope.impersonateUser.DisplayName + '(' + $scope.impersonateUser.UserEmail + ')');
    }

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
