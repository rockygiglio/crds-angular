'use strict()';
(function(){
  angular.module('crossroads.mptools').factory('Su2sData', Su2sData);

  Su2sData.$inject = ['$resource'];

  function Su2sData($resource){
    return $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/opportunity/getGroupParticipantsForOpportunity/:oppId');
  }

})();
