(function() {
  'use strict';

  module.exports = GoVolunteerProjectPrefTwo;

  GoVolunteerProjectPrefTwo.$inject = ['$sce'];

  function GoVolunteerProjectPrefTwo() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerProjectPrefTwoController,
      controllerAs: 'goProjectPrefTwo',
      templateUrl: 'projectPrefTwo/goVolunteerProjectPrefTwo.template.html'
    };

    function GoVolunteerProjectPrefTwoController($sce) {
      var vm = this;

      vm.list = [
        { title: 'Artistic Painting', state: '', age: '13' },
        { title: 'Construction', state: '', age: '13' },
        { title: 'Gardening', state: '', age: '2' },
        { title: 'Landscaping', state: 'disabled', age: '8' },
        { title: 'Organizing and Cleaning', state: '', age: '2' },
        { title: 'Painting', state: '', age: '13' },
        { title: 'Prayer', state: '', age: '2' },
        { title: 'Serving Meals or Throw A Party', state: '', age: '8' },
        { title: 'Working with Children', state: '', age: '2' },
        { title: 'Working with the Elderly', state: '', age: '2' }
      ];

    }
  }

})();
