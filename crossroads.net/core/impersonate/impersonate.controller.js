
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
        this.setImpersonateDetails(true, response.user, response.contact);
      }).error(() => {
        this.processing = false;
        this.error = true;
        this.setImpersonateDetails(false);
      });
    };

    this.stop = () => {
      this.setImpersonateDetails(false);
    };

    this.setImpersonateDetails = (active, user, contact) => {
      if (active !== true) {
        active = false;
      }
      $rootScope.impersonation.active = active;
      $rootScope.impersonation.user = user;
      $rootScope.impersonation.contact = contact;
      if (user !== undefined) {
        this.setImpersonateHeader(user.UserRecordId, user.UserId, user.Guid);
      } else {
        this.setImpersonateHeader();
      }
    };

    this.setImpersonateHeader = (id, username, guid) => {
      $http.defaults.headers.common.ImpersonateRecordId = id;
      $http.defaults.headers.common.ImpersonateUserId = username;
      $http.defaults.headers.common.ImpersonateGuid = guid;
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
