(function(){
  'use strict';

  module.exports = HostConfirmCtrl;

  HostConfirmCtrl.$inject = ['$state'];

  function HostConfirmCtrl($state) {

    var vm = this;

    vm.showDashboard = function() {
      $state.go('group_finder.dashboard');
    };
  }

})();
