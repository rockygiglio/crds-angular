(function() {
  'use strict';

  module.exports = SiteFilter;

  SiteFilter.$inject = ['$log', 'LookupService'];

  function SiteFilter($log, LookupService) {
    return {
      restrict: 'EA',
      replace: true,
      scope: {
        siteId: '=',
        onChange: '&'
      },
      templateUrl: 'templates/siteFilter.html',
      link: link
    };

    function link(scope, element, attrs) {
      scope.sites = [];

      activate();

      /////////////////////////////////
      ////// IMPLMENTATION DETAILS ////
      /////////////////////////////////

      function activate() {
        LookupService.Congregations.query({},
          function(data) {
            scope.sites = data;
        });
      }

    }
  }
})();
