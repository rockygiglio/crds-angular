(function() {
  'use strict';

  module.exports = PersonService;

  PersonService.$inject = ['$rootScope', '$cookies', 'Profile', 'AUTH_EVENTS', '$log'];

  function PersonService($rootScope, $cookies, Profile, AUTH_EVENTS, $log) {
    var promise = null;

    //
    // Authenticated Person Info Service
    //

    var service = {};
    service.loadProfile = loadProfile;
    service.getProfile = getProfile;

    //
    // Listen for the logout event notification to clear the data
    //

    $rootScope.$on(AUTH_EVENTS.logoutSuccess, clearData);

    //
    // Service Implementation
    //

    function loadProfile() {
      if (!promise) {
        var cid = $cookies.get('userId');
        if (cid) {
          $log.debug('PersonService - load profile for', cid);
          promise = Profile.Person.get({contactId: cid}).$promise;

          promise.then(function(data) {
            $log.debug('PersonService - profile loaded', data);
            service.profile = data;
          });
        }
      }

      return promise;
    }

    function getProfile() {
      var loadPromise = loadProfile();
      return loadPromise.then(function() {
        $log.debug('PersonService.getProfile() return authenticated profile');
        return service.profile;
      });
    }

    function clearData() {
      $log.debug('Clear Group Profile data for logged in user');
      promise = null;
      delete service.profile;
    }

    // Return the service instance
    return service;
  }
})();
