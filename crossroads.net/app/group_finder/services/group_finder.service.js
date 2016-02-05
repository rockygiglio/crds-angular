(function(){
  'use strict';

  module.exports = GroupFinderService;

  GroupFinderService.$inject = [];

  function GroupFinderService() {

    function GroupFinder(data) {
      this.setData(data);
    }

    GroupFinder.prototype = {
      setData: function(data) {
        angular.extend(this, data);
      },
      clear: function() {
        console.log('clear');
      }
    };

    return GroupFinder;
  }

})();
