(function(){
  'use strict';

  module.exports = GroupInvitationService;

  GroupInvitationService.$inject = ['$log', '$q', '$timeout'];

  function GroupInvitationService($log, $q, $timeout) {
    var service = {};
    service.acceptInvitation = acceptInvitation;

    function acceptInvitation(groupId, contactId) {
      $log.debug("Accepting invitation to", groupId);

      // TODO this is a temporary promise that would be replaced by the actual HTTP call
      var deferred = $q.defer();

      // TODO remove fake API all delay
      $timeout(function() {
        $log.debug("InvitationService fake API call completed");
        deferred.resolve(true);
      }, 2000);

      return deferred.promise;
    }

    return service;
  }

})();
