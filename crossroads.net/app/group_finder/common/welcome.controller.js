(function () {
  'use strict';

  module.exports = WelcomeCtrl;

  WelcomeCtrl.$scope = ['AuthService', '$state'];

  function WelcomeCtrl(AuthService, $state) {
    var vm = this;
    if (AuthService.isAuthenticated()) {
      $state.go('group_finder');
    }
  }
})();