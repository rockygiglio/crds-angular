(function() {
  module.exports = function LookupService($resource, Session) {
    return $resource(__GATEWAY_CLIENT_ENDPOINT__ + 'api/lookup/');
  };
})();
