(function(){
  'use strict';

  module.exports = HostReviewCtrl;

  HostReviewCtrl.$inject = ['$window', '$scope', '$state', 'Responses'];

  function HostReviewCtrl($window, $scope, $state, Responses) {
    var vm = this;

    vm.initialize = function() {
      vm.responses = Responses.data;

      if(vm.isPrivate()) {
        return $state.go('group_finder.host.confirm');
      }

      vm.group = {
        groupTitle: $scope.person.firstName + ' ' + $scope.person.lastName[0] + '.',
        time: vm.getGroupTime(),
        distance: '0 miles from you',
        description: vm.responses.description,
        type: vm.responses.group_type,
        attributes: vm.getGroupAttributes(),
        host: {
          contactId: $scope.person.contactId
        }
      };
    };

    vm.startOver = function() {
      $scope.$parent.currentStep = 1;
      $state.go('group_finder.host.questions');
    };

    vm.getGroupAttributes = function() {
      var ret = [];
      if (vm.responses.kids === '1') { ret.push('kids welcome'); }
      if (vm.responses.pets) {
        var pet_selections = _.map(Object.keys(vm.responses.pets), function(el) {
          return parseInt(el);
        });
        if (pet_selections.indexOf(0) !== -1) { ret.push('has a cat'); }
        if (pet_selections.indexOf(1) !== -1) { ret.push('has a dog'); }
      }
      return ret;
    };

    vm.getGroupTime = function() {
      var dt = vm.responses.date_and_time;
      if (dt) {
        return dt['day'] + 's @ ' + dt['time'] + dt['ampm'];
      }
    };

    vm.goBack = function() {
      $window.history.back();
    };

    vm.isPrivate = function() {
      return vm.responses && vm.responses.open_spots <= 0;
    };

    // ------------------------------- //

    vm.initialize();

  }

})();
