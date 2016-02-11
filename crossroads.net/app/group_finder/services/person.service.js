(function() {
  'use strict';

  module.exports = PersonService;

  PersonService.$inject = ['Profile', '$cookies'];

  function PersonService(Profile, $cookies) {
    var cid = $cookies.get('userId');
    if (cid) {
      return Profile.Person.get({contactId: cid}).$promise;
    }
  }
})();
