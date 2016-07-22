(function () {
    'use strict';
    module.exports = formlyBuilderRun;

    require('./templates/datepicker.html');

    formlyBuilderRun.$inject = ['formlyConfig', 'formlyValidationMessages'];

    function formlyBuilderRun(formlyConfig, formlyValidationMessages) {
        var attributes = [
            'date-disabled',
            'custom-class',
            'show-weeks',
            'starting-day',
            'init-date',
            'min-mode',
            'max-mode',
            'format-day',
            'format-month',
            'format-year',
            'format-day-header',
            'format-day-title',
            'format-month-title',
            'year-range',
            'shortcut-propagation',
            'datepicker-popup',
            'show-button-bar',
            'current-text',
            'clear-text',
            'close-text',
            'close-on-date-selection',
            'datepicker-append-to-body'
        ];

        var bindings = [
            'datepicker-mode',
            'min-date',
            'max-date'
        ];

        var ngModelAttrs = {};

        angular.forEach(attributes, function (attr) {
            ngModelAttrs[camelize(attr)] = { attribute: attr };
        });

        angular.forEach(bindings, function (binding) {
            ngModelAttrs[camelize(binding)] = { bound: binding };
        });

        formlyConfig.setType({
            name: 'datepicker',
            templateUrl: 'templates/datepicker.html',
            wrapper: ['bootstrapLabel', 'bootstrapHasError'],
            defaultOptions: {
                ngModelAttrs: ngModelAttrs,
                templateOptions: {
                    datepickerOptions: {
                        format: 'MM/dd/yyyy',
                        initDate: new Date()
                    }
                }
            },
            controller: ['$scope', function ($scope) {
                $scope.datepicker = {};

                $scope.datepicker.opened = false;

                $scope.datepicker.open = function ($event) {
                    $scope.datepicker.opened = !$scope.datepicker.opened;
                };
            }]
        });

        ngModelAttrs = {};

        // attributes
        angular.forEach([
            'meridians',
            'readonly-input',
            'mousewheel',
            'arrowkeys'
        ], function (attr) {
            ngModelAttrs[camelize(attr)] = { attribute: attr };
        });

        // bindings
        angular.forEach([
            'hour-step',
            'minute-step',
            'show-meridian'
        ], function (binding) {
            ngModelAttrs[camelize(binding)] = { bound: binding };
        });

        formlyConfig.setType({
            name: 'timepicker',
            template: '<timepicker ng-model="model[options.key]"></timepicker>',
            wrapper: ['bootstrapLabel', 'bootstrapHasError'],
            defaultOptions: {
                ngModelAttrs: ngModelAttrs,
                templateOptions: {
                    datepickerOptions: {}
                }
            }
        });

        formlyConfig.setType({
            name: 'boldcheckbox',
            template: require('./templates/boldCheckbox.html'),
            wrapper: ['bootstrapHasError'],
            apiCheck: check => ({
                templateOptions: {
                    label: check.string
                }
            })
        });
        formlyConfig.setType({
            name: 'zipcode',
            defaultOptions: {
                validators: {
                    zipcode: {
                        expression: function (value) {
                            let regex = /^\d{5}$/;
                            return regex.test(value);
                        },
                        message: "'Zip code does not appear to be valid.'"
                    }
                }
            }
        });

        ngModelAttrs = {};

        // bindings
        angular.forEach([
            'contact-id',
            'wrapper-class',
            'image-class'
        ], function(binding) {
            ngModelAttrs[camelize(binding)] = { bound: binding };
        });

        formlyConfig.setType({
            name: 'profilePicture',
            template: require('./templates/profilePicture.html'),
            wrapper: ['bootstrapHasError'],
            defaultOptions: {
                ngModelAttrs: ngModelAttrs,
                templateOptions: {}
            }
        });
        

        function camelize(string) {
            string = string.replace(/[\-_\s]+(.)?/g, function (match, chr) {
                return chr ? chr.toUpperCase() : '';
            });
            // Ensure 1st char is always lowercase
            return string.replace(/^([A-Z])/, function (match, chr) {
                return chr ? chr.toLowerCase() : '';
            });
        }

        formlyValidationMessages.addStringMessage('required', 'This field is required');
        formlyValidationMessages.addTemplateOptionValueMessage('maxlength', 'maxlength', '', 'is the maximum length', 'Too long');
        formlyConfig.extras.errorExistsAndShouldBeVisibleExpression = 'fc.$touched || form.$submitted';


        formlyConfig.setType({
            name: 'multiCheckBoxCombo',
            template: require('./templates/multiCheckBoxCombo.html'),
            wrapper: ['bootstrapLabel', 'bootstrapHasError'],
            apiCheck: check => ({
                templateOptions: {
                    options: check.arrayOf(check.object),
                    labelProp: check.string.optional,
                    valueProp: check.string.optional,
                    placeholder: check.string.optional
                }
            }),
            defaultOptions: {
                noFormControl: false,
                ngModelAttrs: {
                    required: {
                        attribute: '',
                        bound: ''
                    }
                }
            },
            controller: /* @ngInject */ function ($scope) {
                const to = $scope.to;
                const opts = $scope.options;
                const ep = $scope.expressionProperties;
                $scope.multiCheckboxCombo = {
                    checked: [],
                    detail: [],
                    change: setModel
                };

                // initialize the checkboxes check property
                $scope.$watch('model', function modelWatcher(newModelValue) {
                    var modelValue, valueProp;
                    if (Object.keys(newModelValue).length) {
                        modelValue = newModelValue[opts.key];

                        $scope.$watch('to.options', function optionsWatcher(newOptionsValues) {
                            if (newOptionsValues && Array.isArray(newOptionsValues) && Array.isArray(modelValue)) {
                                valueProp = to.valueProp || 'value';
                                for (var index = 0; index < newOptionsValues.length; index++) {
                                    //$scope.multiCheckboxCombo.checked[index] = modelValue.indexOf(newOptionsValues[index][valueProp]) !== -1;
                                    $scope.multiCheckboxCombo.checked[index] = _.findIndex(modelValue, (item) => {return item.value == newOptionsValues[index][valueProp]} ) !== -1;// modelValue.indexOf(newOptionsValues[index][valueProp]) !== -1;
                                }
                            }
                        });
                    }
                }, true);

                function areRequiredDetailsFilledOut() {
                    var valid = true;
                    angular.forEach($scope.model[opts.key], (item, index) => {
                        if (item.detail == 'undefined' || item.detail == null || item.detail == '') {
                            valid = false;
                        }
                    });
                    return valid;
                }

                function checkValidity(expressionValue) {
                    var checkValid, detailValid;
                    if ($scope.to.required) {
                        checkValid = angular.isArray($scope.model[opts.key]) &&
                            $scope.model[opts.key].length > 0 &&
                            expressionValue;
                        
                        //if checkbox is checked, detail is required
                        detailValid = areRequiredDetailsFilledOut();

                        $scope.fc[0].$setValidity('required', checkValid && detailValid);
                    }
                }

                function setModel() {
                    $scope.model[opts.key] = [];
                    angular.forEach($scope.multiCheckboxCombo.checked, (checkbox, index) => {
                        if (checkbox) {
                            $scope.model[opts.key].push({ detail: $scope.multiCheckboxCombo.detail[index], value: to.options[index][to.valueProp || 'value']});
                        } else {
                            $scope.multiCheckboxCombo.detail[index] = '';
                        }
                    });
                    // Must make sure we mark as touched because only the last checkbox due to a bug in angular.
                    $scope.fc[0].$setTouched();
                    $scope.fc[1].$setTouched();
                    checkValidity(true);

                    if ($scope.to.onChange) {
                        $scope.to.onChange();
                    }
                }

                if (opts.expressionProperties && opts.expressionProperties['templateOptions.required']) {
                    $scope.$watch(function () {
                        return $scope.to.required;
                    }, function (newValue) {
                        checkValidity(newValue);
                    });
                }

                if ($scope.to.required) {
                    var unwatchFormControl = $scope.$watch('fc', function (newValue) {
                        if (!newValue) {
                            return;
                        }
                        checkValidity(true);
                        unwatchFormControl();
                    });
                }
            }
        });
    }
})();
