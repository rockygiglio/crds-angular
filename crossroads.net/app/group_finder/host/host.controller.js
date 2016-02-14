(function () {
  'use strict';

  module.exports = HostCtrl;

  HostCtrl.$inject = ['$scope', '$log', '$state', 'AuthService', 'QuestionDefinitions'];

  function HostCtrl ($scope, $log, $state, AuthService, QuestionDefinitions) {

    if (AuthService.isAuthenticated() === false) {
      $log.debug('not logged in');
      $state.go('group_finder.welcome');
    }

    var vm = this;

    $scope.currentStep = 1;
    vm.totalSlides = 5;
    vm.currentSlide = 1;
    vm.nextButton = 'Next';

    vm.nextSlide = function () {
      if (vm.currentSlide < vm.totalSlides) {
        vm.currentSlide++;
        if (vm.onLastSlide()) {
            vm.nextButton = 'Choose a Role';
        }
      } else if (vm.onLastSlide()) {
        $state.go('group_finder.host');
      }
    };

    vm.previousSlide = function () {
      if (vm.currentSlide > 1) {
        vm.currentSlide--;
      }
    };

    vm.showSlide = function (index) {
      return index === vm.currentSlide;
    };

    vm.onLastSlide = function () {
      return vm.currentSlide === vm.totalSlides;
    };

    vm.hostQuestions = function() {
      $state.go('group_finder.host.questions');
    };

    vm.joinQuestions = function() {
      $state.go('group_finder.join', { step: 1 });
    };

  }
})();
