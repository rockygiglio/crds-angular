(function() {
  'use strict';

  module.exports = UserService;

  UserService.$inject = ['$http'];

  function UserService($http) {
    // TODO Update to use $resource
    return $http.get('/app/group_finder/data/user.group.json')
      .then(function(res){
        return res.data;
    });
  }
})();
