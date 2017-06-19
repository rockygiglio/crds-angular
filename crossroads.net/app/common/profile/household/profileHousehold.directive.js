'use strict';
(function() {
  module.exports = ProfileHouseholdDirective;

  ProfileHouseholdDirective.$inject = ['$log'];

  function ProfileHouseholdDirective($log) {
    // don't log anything in prod environment
    if (!__CRDS_ENV__) {
      $log.debug('ProfileHouseholdDirective');
    }
    return {
      restrict: 'E',
      bindToController: true,
      scope: {
        householdId: '=?',
        householdInfo: '=',
        householdForm: '=',
        isCollapsed: '=?',
        modalInstance: '=?',
        locations: '='
      },
      templateUrl: 'household/profileHousehold.template.html',
      controller: 'ProfileHouseholdController as household',
      link: link
    };
  }

  function link(scope, el, attr, controller) {
    var throwAway = scope.householdId;
  }

})();
