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
        { title: 'Artistic Painting', state: '', age: '13', img: 'art' },
        { title: 'Construction', state: '', age: '13', img: 'cons' },
        { title: 'Gardening', state: '', age: '2', img: 'gard' },
        { title: 'Landscaping', state: '', age: '8', img: 'gardening' },
        { title: 'Organizing and Cleaning', state: '', age: '2', img: 'org' },
        { title: 'Painting', state: 'disabled', age: '13', img: 'painting' },
        { title: 'Prayer', state: '', age: '2', img: 'prayer' },
        { title: 'Serving Meals or Throw A Party', state: '', age: '8', img: 'cooking' },
        { title: 'Working with Children', state: '', age: '2', img: 'kids' },
        { title: 'Working with the Elderly', state: '', age: '2', img: 'elder' }
      ];

    }
  }

})();
