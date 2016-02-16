(function() {
  'use strict';

  module.exports = GroupProfileDirective;

  require('./group_profile.html');

  GroupProfileDirective.$inject = ['$log'];

  function GroupProfileDirective($log) {
    return {
      restrict: 'AE',
      scope: {
        group: '=?'
      },
      controller: require('./group_profile.controller'),
      templateUrl: 'group_profile/group_profile.html'
    };
  }
})();
