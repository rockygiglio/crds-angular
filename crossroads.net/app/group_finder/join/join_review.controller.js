(function(){
  'use strict';

  module.exports = JoinReviewCtrl;

  JoinReviewCtrl.$inject = [
    '$scope',
    '$state',
    'Responses',
    'ZipcodeService',
    'GroupInvitationService',
    'LookupDefinitions',
    'ANYWHERE_GROUP_ID',
    'GROUP_ROLE_ID_PARTICIPANT',
    '$window'
  ];

  function JoinReviewCtrl(
    $scope,
    $state,
    Responses,
    ZipcodeService,
    GroupInvitationService,
    LookupDefinitions,
    ANYWHERE_GROUP_ID,
    GROUP_ROLE_ID_PARTICIPANT,
    $window
  ) {
    var vm = this;
    vm.initialize = initialize;
    vm.goToHost = goToHost;
    vm.goToResults = goToResults;
    vm.lookup = LookupDefinitions;
    vm.goBack = goBack;
    vm.lookupContains = lookupContains;

    function initialize() {

      vm.responses = getResponses();
      vm.showUpsell = parseInt(vm.responses.prior_participation) > 2;
      vm.showResults = vm.showUpsell === false;
      vm.contactCrds = false;
      var meetTime = {
        week: true,
        weekend: true
      };

      // check of invalid date_times
      _.each(vm.responses.date_time_week, function(value, id) {
        if (value && vm.lookupContains(id, 'can\'t meet')) {
          meetTime.week = false;
        }
      });

      // check of invalid date_times
      _.each(vm.responses.date_time_weekend, function(value, id) {
        if (value && vm.lookupContains(id, 'can\'t meet')) {
          meetTime.weekend = false;
        }
      });

      vm.invalidTime = (!meetTime.week && !meetTime.weekend);

      if (vm.responses.location && vm.responses.location.zip) {
        vm.zipcode = parseInt(vm.responses.location.zip);
        if (ZipcodeService.isLocalZipcode(vm.zipcode) === false) {
          vm.showUpsell = false;
          vm.showResults = false;
          vm.contactCrds = true;

          var promise = GroupInvitationService.acceptInvitation(ANYWHERE_GROUP_ID,
            {capacity: 1, groupRoleId: GROUP_ROLE_ID_PARTICIPANT});
          promise.then(function() {
            // Invitation acceptance was successful
            vm.accepted = true;
          }, function(error) {
            // An error happened accepting the invitation
            vm.rejected = true;
          });
        }
      }

      if (vm.showResults === true && vm.contactCrds === false) {
        $state.go('group_finder.join.results');
      }

      if (parseInt(vm.responses.relationship_status) === 2) {
        $scope.showInvite = true;
      }
    }
    
    function getResponses() {
      var responses = Responses.data;
      if (responses.completed_flow === true) {
        sessionStorage.setItem('participant', angular.toJson(responses));
      } else {
        responses = angular.fromJson(sessionStorage.getItem('participant'));
      }

      return responses;
    }

    function goToHost() {
      $state.go('group_finder.host');
    }

    function goToResults() {
      $state.go('group_finder.join.results');
    }

    function lookupContains(id, keyword) {
      return vm.lookup[id].name.toLowerCase().indexOf(keyword) > -1;
    }

    function goBack() {
      $window.history.back();
    }

    vm.initialize();

  }

})();
