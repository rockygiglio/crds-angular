(function() {
  'use strict';

  module.exports = GoVolunteerProjectPrefThree;

  GoVolunteerProjectPrefThree.$inject = ['$sce'];

  function GoVolunteerProjectPrefThree() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerProjectPrefThreeController,
      controllerAs: 'goProjectPrefThree',
      templateUrl: 'projectPrefThree/goVolunteerProjectPrefThree.template.html'
    };

    function GoVolunteerProjectPrefThreeController($sce) {
      var vm = this;

      vm.list = [
        { title: 'Artistic Painting', state: '', age: '13' },
        { title: 'Construction', state: 'disabled', age: '13' },
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
