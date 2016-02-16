(function () {
  'use strict';

  module.exports = SummaryCtrl;

  SummaryCtrl.$inject = ['$scope', '$log', '$state'];

  function SummaryCtrl ($scope, $log, $state) {

    var vm = this;

    vm.totalSlides = 5;
    vm.currentSlide = 1;
    vm.nextButton = 'Next';

    vm.nextSlide = function() {
      if (vm.currentSlide < vm.totalSlides) {
        vm.currentSlide++;
      }
    };

    vm.previousSlide = function() {
      if (vm.currentSlide > 1) {
        vm.currentSlide--;
      }
    };

    vm.showSlide = function(index) {
      return index === vm.currentSlide;
    };

    vm.onLastSlide = function() {
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
