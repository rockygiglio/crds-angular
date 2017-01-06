
/* eslint-disable no-param-reassign */
(() => {
  function ImpersonateController(
    $rootScope,
    $scope,
    $log,
    AuthService,
    $state,
    Session,
    $http
    ) {
    this.username = undefined;
    this.user = undefined;
    this.processing = false;
    this.error = false;

    this.startImpersonating = () => {
      this.processing = true;
      $http({
        method: 'GET',
        url: `${__API_ENDPOINT__}api/user?username=${this.username}`
      }).success((response) => {
        this.processing = false;
        this.error = false;
        this.storeCurrentUser();
        this.storeImpersonateDetails(true, response);
        this.setCurrentUser(response);
      }).error(() => {
        this.processing = false;
        this.error = true;
        this.storeImpersonateDetails(false);
      });
    };

    this.stopImpersonating = () => {
      this.username = undefined;
      this.storeImpersonateDetails(false);
      this.setCurrentUser($rootScope.impersonation.loggedIn);
    };

    this.storeCurrentUser = () => {
      $rootScope.impersonation.loggedIn = {
        userid: $rootScope.userid,
        username: $rootScope.username,
        email: $rootScope.email,
        phone: $rootScope.phone,
        roles: $rootScope.roles
      };
    };

    this.setCurrentUser = (user) => {
      $rootScope.userid = user.userid;
      $rootScope.username = user.username;
      $rootScope.email = user.email;
      $rootScope.phone = user.phone;
      $rootScope.roles = user.roles;
    };

    this.storeImpersonateDetails = (active, loginReturn) => {
      if (active !== true) {
        active = false;
      }
      $rootScope.impersonation.active = active;
      $rootScope.impersonation.impersonated = loginReturn;
      $http.defaults.headers.common.ImpersonateUserId = this.username;
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
    '$http'
  ];
})();
