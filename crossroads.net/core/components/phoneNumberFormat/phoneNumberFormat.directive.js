(function() {
    "use strict";

    module.exports = phoneNumberFormat;

    phoneNumberFormat.$inject = [];
    
    function phoneNumberFormat() {
        return {
            restrict: 'A',
            require: "ngModel",
            link: function(scope, element, attrs, ngModelCtrl) {
                ngModelCtrl.$parsers.push(function(number) {
                    let transformedNumber = number;

                    if (number.match(/^\d{4}$/)) {
                        transformedNumber = number.slice(0, 3) + '-' + number.slice(3);
                    }

                    if (number.match(/^[\d]{3}-[\d]{4}$/)) {
                        transformedNumber = number.slice(0, 7) + '-' + number.slice(7);
                    }

                    if (number.length > 12) {
                        transformedNumber = number.slice(0, 12);
                    }

                    if (transformedNumber !== number) {
                        ngModelCtrl.$setViewValue(transformedNumber);
                        ngModelCtrl.$render();
                    }

                    return transformedNumber;
                });
            }
        }
    }
})();