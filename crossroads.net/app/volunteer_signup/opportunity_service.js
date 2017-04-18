module.exports = function($resource, Session) {
    return $resource(__GATEWAY_CLIENT_ENDPOINT__ + "api/opportunity/:opportunityId");
};
