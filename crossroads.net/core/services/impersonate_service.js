
/* eslint-disable no-param-reassign */
(() => {
  const cookieNames = require('crds-constants').COOKIES;

  function ImpersonateService(
    $http,
    $cookies,
    $rootScope
  ) {
    this.start = (username) => {
      const thisUsername = encodeURI(username).replace(/\+/g, '%2B');
      return $http.get(`${__API_ENDPOINT__}api/user?username=${thisUsername}`);
    };

    this.stop = () => {
      this.storeDetails(false);
      this.setCurrentUser($rootScope.impersonation.loggedIn);
    };

    this.clear = () => {
      $rootScope.impersonation = {
        active: false,
        loggedIn: undefined,
        impersonated: undefined
      };
    };

    this.setCurrentUser = (user) => {
      $rootScope.userid = user.userId;
      $rootScope.username = user.username;
      $rootScope.email = user.userEmail;
      $rootScope.phone = user.userPhone;
      $rootScope.roles = user.roles;
      $cookies.put('userId', user.userId);
      $cookies.put('username', user.username);
      $rootScope.$emit('profilePhotoChanged', user.userId);
    };

    this.storeCurrentUser = () => {
      $rootScope.impersonation.loggedIn = {
        userId: $rootScope.userid,
        username: $rootScope.username,
        userEmail: $rootScope.email,
        userPhone: $rootScope.phone,
        roles: $rootScope.roles
      };
    };

    this.storeDetails = (active, loginReturn, username) => {
      if (active !== true) {
        active = false;
      }
      $rootScope.impersonation.active = active;
      $rootScope.impersonation.impersonated = loginReturn;
      this.setHeaders(username);
      if (active === false) {
        $cookies.remove(cookieNames.IMPERSONATION_ID);
      } else {
        $cookies.put(cookieNames.IMPERSONATION_ID, username);
      }
    };

    this.setHeaders = (username) => {
      $http.defaults.headers.common.ImpersonateUserId = username;
    };
  }

  ImpersonateService.$inject = [
    '$http',
    '$cookies',
    '$rootScope'
  ];

  angular.module('crossroads.core').service('Impersonate', ImpersonateService);
})();
