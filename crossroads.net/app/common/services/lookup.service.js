(function() {
    'use strict';

    module.exports = LookupService;

    LookupService.$inject = ['$resource'];

    function LookupService($resource) {
        return {
            Congregations: $resource(__API_ENDPOINT__ + 'api/lookup/:crossroadslocations')
        };
    }

})();
