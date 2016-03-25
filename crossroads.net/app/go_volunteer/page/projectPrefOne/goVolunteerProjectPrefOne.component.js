(function() {
  'use strict';

  module.exports = GoVolunteerProjectPrefOne;

  GoVolunteerProjectPrefOne.$inject = ['GoVolunteerService'];

  function GoVolunteerProjectPrefOne(GoVolunteerService) {
    return {
      restrict: 'E',
      scope: {},
      bindToController: true,
      controller: GoVolunteerProjectPrefOneController,
      controllerAs: 'goProjectPrefOne',
      templateUrl: 'projectPrefOne/goVolunteerProjectPrefOne.template.html'
    };

    function GoVolunteerProjectPrefOneController() {
      var vm = this;
      vm.projectTypes = GoVolunteerService.projectTypes;
      vm.submit = submit;

      function submit(projectTypeId) {
        GoVolunteerService.projectPrefOne = projectTypeId;
        vm.onSubmit({nextState: 'project-preference-two'});
      }

      // vm.list = [
      //   { title: 'Artistic Painting', state: '', age: '13', img: 'art' },
      //   { title: 'Construction', state: '', age: '13', img: 'cons' },
      //   { title: 'Gardening', state: '', age: '2', img: 'gard' },
      //   { title: 'Landscaping', state: '', age: '8', img: 'gardening' },
      //   { title: 'Organizing and Cleaning', state: '', age: '2', img: 'org' },
      //   { title: 'Painting', state: '', age: '13', img: 'painting' },
      //   { title: 'Prayer', state: '', age: '2', img: 'prayer' },
      //   { title: 'Serving Meals or Throw A Party', state: '', age: '8', img: 'cooking' },
      //   { title: 'Working with Children', state: '', age: '2', img: 'kids' },
      //   { title: 'Working with the Elderly', state: '', age: '2', img: 'elder' }
      // ];

    }
  }

})();
