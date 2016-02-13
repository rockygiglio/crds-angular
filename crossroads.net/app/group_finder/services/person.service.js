(function() {
  'use strict';

  module.exports = PersonService;

  PersonService.$inject = ['Profile', '$cookies'];

  function PersonService(Profile, $cookies) {
    var resource = Profile.Person;
    var cid = $cookies.get('userId');
    if (cid) {
      resource = resource.get({contactId: cid}).$promise;
    }

    return resource;
  }
})();
