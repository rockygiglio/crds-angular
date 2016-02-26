(function(){
  'use strict';

  module.exports = HostQuestionsCtrl;

  HostQuestionsCtrl.$inject = ['$scope', 'Responses', 'QuestionDefinitions', 'GROUP_TYPES', 'AuthenticatedPerson'];

  function HostQuestionsCtrl($scope, Responses, QuestionDefinitions, GROUP_TYPES, AuthenticatedPerson) {

    if (_.has(Responses.data, 'location') === false) {
      Responses.data.location = {
        city:   AuthenticatedPerson.city,
        state:  AuthenticatedPerson.state,
        zip:    AuthenticatedPerson.postalCode,
        street: AuthenticatedPerson.addressLine1
      };
    }
    if (_.has(Responses.data, 'date_and_time') === false) {
      Responses.data.date_and_time = {};
      Responses.data.date_and_time.time = moment().hours(moment().hour()).minute(0)._d;
    }

    var vm = this;
        vm.questions = QuestionDefinitions;
        vm.currentStep = $scope.$parent.currentStep;
        vm.responses = $scope.responses = Responses.data;

    $scope.details = {};

    $scope.$watch('responses', function(responses) {
      vm.updateDetails();
    }, true);

    vm.getGroupType = function() {
      if(vm.responses && _.contains(Object.keys(vm.responses),'group_type')) {
        return GROUP_TYPES[vm.responses.group_type];
      }
    };

    vm.getGroupTime = function() {
      if(vm.responses && _.contains(Object.keys(vm.responses),'date_and_time')) {
        return vm.responses.date_and_time['day'] + 's, ' +
               vm.responses.date_and_time['time'] + vm.responses.date_and_time['ampm'];
      }
    };

    // TODO Populate with actual affinities based on user responses.
    vm.getGroupAffinities = function() {
      return [
        'Kids welcome',
        'Has a cat',
        'Has a dog'
      ];
    };

    // TODO Implement distance.
    vm.getGroupDistance = function() {
      return '0 miles from you';
    };

    // TODO This should be a factory
    vm.updateDetails = function() {
      $scope.details = {
        affinities: vm.getGroupAffinities(),
        distance: vm.getGroupDistance(),
        type: vm.getGroupType(),
        time: vm.getGroupTime()
      };
    };

  }

})();
