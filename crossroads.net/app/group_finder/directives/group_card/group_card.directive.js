(function() {
  'use strict';

  module.exports = GroupCardDirective;

  require('./group_card.html');
  require('./group_join.html');
  require('./join_modal.html');

  GroupCardDirective.$inject = ['$log'];

  function GroupCardDirective($log) {
    return {
      restrict: 'AE',
      scope: {
        group: '=',
        host: '=',
        template: '@'
      },
      controller: require('./group_card.controller'),
      // templateUrl: 'group_card/group_card.html'
      templateUrl: function (el, attrs) {
        var template = 'group_card/group_card.html';
        if (attrs.template === 'join') {
          template = 'group_card/group_join.html';
        }

        return template;
      }
    };
  }
})();
