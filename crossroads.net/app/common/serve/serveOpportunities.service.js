(function(){
  module.exports = ServeOpportunities;

  ServeOpportunities.$inject = ['$resource'];

  function ServeOpportunities($resource){
    return {
    	AllOpportunityDates: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/opportunity/getAllOpportunityDates/:id'),
    	LastOpportunityDate: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/opportunity/getLastOpportunityDate/:id'),
    	ServeDays: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/serve/family-serve-days/:id'),
      SaveRsvp: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/serve/save-rsvp'),
      Family: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/serve/family/:id'),
      QualifiedServers: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/serve/qualifiedservers/:groupId/:opportunityId'),
      SaveQualifiedServers: $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/opportunity/save-qualified-server')
    }
  }
})();
