(function() {
  'use strict';

  module.exports = PersonService;

  PersonService.$inject = ['Profile'];

  function PersonService(Profile) {
    var person = {};

    person.loadProfile = function(cid) {
      if (cid) {
        Profile.Person.get({contactId: cid}, function(data) {
          console.log('PersonService', data);
          person = data;
        });
      }
    };

    person.getProfile = function() {
      return person;
    };

    return person;
  }
})();
