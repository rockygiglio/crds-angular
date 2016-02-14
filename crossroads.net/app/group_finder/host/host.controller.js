(function () {
  'use strict';

  module.exports = HostCtrl;

  HostCtrl.$inject = ['$scope', '$log', '$state', 'AuthService', 'QuestionDefinitions', 'SERIES'];

  function HostCtrl ($scope, $log, $state, AuthService, QuestionDefinitions, SERIES) {

    if (AuthService.isAuthenticated() === false) {
      $log.debug('not logged in');
      $state.go(SERIES.permalink + '.welcome');
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
        $state.go(SERIES.permalink + '.host');
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
      $state.go(SERIES.permalink + '.host.questions');
    };

    vm.joinQuestions = function() {
      $state.go(SERIES.permalink + '.join', { step: 1 });
    };

  }
})();
