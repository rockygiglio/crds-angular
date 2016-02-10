(function() {
  'use strict';

  module.exports = GroupInfoDirective;

  require('./group_info.html');

  GroupInfoDirective.$inject = ['$log'];

  function GroupInfoDirective($log) {
    return {
      restrict: 'AE',
      scope: {
        group: '=',
        host: '='
      },
      templateUrl: 'directives/group_info.html'
    };
  }
})();
