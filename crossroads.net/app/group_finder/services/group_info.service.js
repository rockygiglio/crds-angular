(function(){
  'use strict';

  module.exports = GroupInfoService;

  GroupInfoService.$inject = ['$resource', 'User'];

  function GroupInfoService($resource) {

    var url = '/app/group_finder/data/host.questions.json';
    return $resource(url, {}, { get: { method:'GET', cache: true }});
  }

})();
