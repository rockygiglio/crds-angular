(function() {
  'use strict';

  module.exports = GroupCardDirective;

  require('./group_card.html');

  GroupCardDirective.$inject = ['$log'];

  function GroupCardDirective($log) {
    return {
      restrict: 'AE',
      scope: {
        group: '=',
        host: '='
      },
      templateUrl: 'directives/group_card.html'
    };
  }
})();
