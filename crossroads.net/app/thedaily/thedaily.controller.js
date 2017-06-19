(function() {
    'use strict';

    module.exports = TheDailyController;

    TheDailyController.$inject = [
        '$rootScope',
        '$log',
        '$state'
    ];

    function TheDailyController(
        $rootScope,
        $log,
        $state
    ) {

        if (!__CRDS_ENV__) {
            $log.debug('Inside The Daily Controller');
        }

        var vm = this;
    }
})();
