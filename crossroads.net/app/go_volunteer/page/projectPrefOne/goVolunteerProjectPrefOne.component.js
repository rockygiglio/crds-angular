(function() {
  'use strict';

  module.exports = GoVolunteerProjectPrefOne;

  GoVolunteerProjectPrefOne.$inject = ['$sce'];

  function GoVolunteerProjectPrefOne() {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerProjectPrefOneController,
      controllerAs: 'goProjectPrefOne',
      templateUrl: 'projectPrefOne/goVolunteerProjectPrefOne.template.html'
    };

    function GoVolunteerProjectPrefOneController($sce) {
      var vm = this;

      vm.list = [
        { title: 'Artistic Painting', state: '', age: '13', img: 'art' },
        { title: 'Construction', state: '', age: '13', img: 'cons' },
        { title: 'Gardening', state: '', age: '2', img: 'gard' },
        { title: 'Landscaping', state: '', age: '8', img: 'gardening' },
        { title: 'Organizing and Cleaning', state: '', age: '2', img: 'org' },
        { title: 'Painting', state: '', age: '13', img: 'painting' },
        { title: 'Prayer', state: '', age: '2', img: 'prayer' },
        { title: 'Serving Meals or Throw A Party', state: '', age: '8', img: 'cooking' },
        { title: 'Working with Children', state: '', age: '2', img: 'kids' },
        { title: 'Working with the Elderly', state: '', age: '2', img: 'elder' }
      ];

    }
  }

})();
