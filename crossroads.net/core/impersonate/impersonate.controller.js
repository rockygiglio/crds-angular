
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

    this.process = () => {
      this.processing = true;
      $http({
        method: 'GET',
        url: `${__API_ENDPOINT__}api/user?username=${this.username}`
      }).success((response) => {
        this.processing = false;
        this.error = false;
        console.log(response);
        this.setImpersonateDetails(true, response.user, response.contact, response.user.UserRecordId);
      }).error(() => {
        this.processing = false;
        this.error = true;
        this.setImpersonateDetails(false);
      });
    };

    this.setImpersonateDetails = (active, user, contact, id) => {
      if (active !== true) {
        active = false;
      }
      $rootScope.impersonation.active = active;
      $rootScope.impersonation.user = user;
      $rootScope.impersonation.contact = contact;
      $http.defaults.headers.common.ImpersonateUser = id;
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
