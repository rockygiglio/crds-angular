
/* eslint-disable no-param-reassign */
(() => {
  function ImpersonateController(
    $rootScope,
    $http,
    $cookies,
    Impersonate
    ) {
    this.username = undefined;
    this.user = undefined;
    this.processing = false;
    this.error = false;

    this.start = () => {
      this.processing = true;
      Impersonate.start(this.username)
      .success((response) => {
        this.processing = false;
        this.error = false;
        Impersonate.storeCurrentUser();
        Impersonate.storeDetails(true, response, this.username);
        Impersonate.setCurrentUser(response);
      })
      .error(() => {
        this.processing = false;
        this.error = true;
        Impersonate.stop();
      });
    };

    this.stop = () => {
      this.username = undefined;
      Impersonate.stop();
    };
  }

  module.exports = ImpersonateController;

  ImpersonateController.$inject = [
    '$rootScope',
    '$http',
    '$cookies',
    'Impersonate'
  ];
})();
