
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
    this.username = '';
    this.user = undefined;
    this.processing = false;
    this.error = false;
    this.userId = undefined;

    this.process = () => {
      this.processing = true;
      $http({
        method: 'GET',
        url: `${__API_ENDPOINT__}api/user?username=${this.username}`
      }).success((user) => {
        this.processing = false;
        this.error = false;
        this.userId = user;
        this.configureImpersonateHeader();
      }).error(() => {
        this.processing = false;
        this.error = true;
      });
    };

    this.configureImpersonateHeader = () => {
      $http.defaults.headers.common.ImpersonateUser = this.userId;
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
    '$http'
  ];
})();
