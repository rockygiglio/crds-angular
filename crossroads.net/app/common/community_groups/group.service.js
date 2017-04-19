(function() {

  module.exports = GroupService;

  GroupService.$inject = ['$resource', '$log'];

  function GroupService($resource, $log) {
    return {
      Participant: $resource(__GATEWAY_CLIENT_ENDPOINT__ +  'api/group/:groupId/participants', {groupId: '@groupId'},
        {save: {method:'POST', isArray:true}}),
      Type: $resource(__GATEWAY_CLIENT_ENDPOINT__ +  'api/group/groupType/:groupTypeId', {groupTypeId: '@groupTypeId'}),
      Search: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/group/groupType/:groupTypeId/search', {groupTypeId: '@groupTypeId'},
        {save: {method:'POST', isArray:true}}),
      Detail: $resource(__GATEWAY_CLIENT_ENDPOINT__ +  'api/group/:groupId', {groupId: '@groupId'}),
      Events: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/group/:groupId/events'),
      Participants: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/group/:groupId/event/:eventId'),
      EmailInvite: $resource(__GATEWAY_CLIENT_ENDPOINT__ + '/api/journey/emailinvite')
    };
  }

})();
