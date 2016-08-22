(function() {
  'use strict';
  module.exports = formlyBuilderConfig;

  formlyBuilderConfig.$inject = ['formlyConfigProvider'];

  function formlyBuilderConfig(formlyConfigProvider) {
    formlyConfigProvider.setWrapper([{
        template: [
          '<formly-transclude></formly-transclude>',
          '<div class="validation"',
          '  ng-if="showError"',
          '  ng-messages="fc.$error || fc[0].$error">',
          '  <div ng-message="{{::name}}" ng-repeat="(name, message) in ::options.validation.messages">',
          '    {{message(fc.$viewValue, fc.$modelValue, this)}}',
          '  </div>',
          '</div>'
        ].join(' ')}
      ]);

    formlyConfigProvider.setWrapper([
      {
        name: 'formlyBuilderLabel',
        template: require('./templates/formlyBuilder-label.html'),
        apiCheck: check => ({
          templateOptions: {
            label: check.string.optional,
            required: check.bool.optional,
            labelSrOnly: check.bool.optional,
          }
        })
      },
      {name: 'formlyBuilderHasError', template: require('./templates/formlyBuilder-has-error.html')}
    ]);

    formlyConfigProvider.setType({
      name: 'formlyBuilderCheckbox',
      template: require('./templates/formlyBuilder-checkbox.html'),
      wrapper: ['formlyBuilderHasError'],
      apiCheck: check => ({
        templateOptions: {
        	label: check.string
        }
      })
    });

    formlyConfigProvider.setType({
      name: 'formlyBuilderInput',
      template: '<input class="form-control" ng-model="model[options.key]">',
      wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel']
    });

    formlyConfigProvider.setType({
      name: 'formlyBuilderMultiCheckbox',
      template: require('./templates/formlyBuilder-multiCheckbox.html'),
      wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel'],
      apiCheck: check => ({
        templateOptions: {
          options: check.arrayOf(check.object),
          labelProp: check.string.optional,
          valueProp: check.string.optional
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
      controller: /* @ngInject */ function($scope) {
        const to = $scope.to;
        const opts = $scope.options;
        $scope.multiCheckbox = {
          checked: [],
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
                  $scope.multiCheckbox.checked[index] = modelValue.indexOf(newOptionsValues[index][valueProp]) !== -1;
                }
              }
            });
          }
        }, true);

        function checkValidity(expressionValue) {
          var valid;
          if ($scope.to.required) {
            valid = angular.isArray($scope.model[opts.key]) &&
              $scope.model[opts.key].length > 0 &&
              expressionValue;

            $scope.fc.$setValidity('required', valid);
          }
        }

        function setModel() {
          $scope.model[opts.key] = [];
          angular.forEach($scope.multiCheckbox.checked, (checkbox, index) => {
            if (checkbox) {
              $scope.model[opts.key].push(to.options[index][to.valueProp || 'value']);
            }
          });

          // Must make sure we mark as touched because only the last checkbox due to a bug in angular.
          $scope.fc.$setTouched();
          checkValidity(true);
          
          if ($scope.to.onChange) {
            $scope.to.onChange();
          }
        }

        if (opts.expressionProperties && opts.expressionProperties['templateOptions.required']) {
          $scope.$watch(function() {
            return $scope.to.required;
          }, function(newValue) {
            checkValidity(newValue);
          });
        }

        if ($scope.to.required) {
          var unwatchFormControl = $scope.$watch('fc', function(newValue) {
            if (!newValue) {
              return;
            }
            checkValidity(true);
            unwatchFormControl();
          });
        }
      }
    });

    formlyConfigProvider.setType({
      name: 'formlyBuilderRadio',
      template: require('./templates/formlyBuilder-radio.html'),
      wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel'],
      defaultOptions: {
        noFormControl: false
      },
      apiCheck: check => ({
        templateOptions: {
          options: check.arrayOf(check.object),
          labelProp: check.string.optional,
          valueProp: check.string.optional,
          inline: check.bool.optional,
        }
      })
    });

    formlyConfigProvider.setType({
      name: 'formlyBuilderRadioDesc',
      template: require('./templates/formlyBuilder-radioDescription.html'),
      wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel'],
      defaultOptions: {
        noFormControl: false
      },
      apiCheck: check => ({
        templateOptions: {
          options: check.arrayOf(check.object),
          labelProp: check.string.optional,
          valueProp: check.string.optional,
          descProp: check.string.optional,
          inline: check.bool.optional,
        }
      })
    });


    formlyConfigProvider.setType({
      name: 'formlyBuilderSelect',
      template: '<select class="form-control" ng-model="model[options.key]"></select>',
      wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel'],
      defaultOptions(options) {
        let ngOptions = options.templateOptions.ngOptions || `option[to.valueProp || 'value'] as option[to.labelProp || 'name'] group by option[to.groupProp || 'group'] for option in to.options`;
        return {
          ngModelAttrs: {
            [ngOptions]: {
              value: options.templateOptions.optionsAttr || 'ng-options'
            }
          }
        };
      },
      apiCheck: check => ({
        templateOptions: {
          options: check.arrayOf(check.object),
          optionsAttr: check.string.optional,
          labelProp: check.string.optional,
          valueProp: check.string.optional,
          groupProp: check.string.optional
        }
      })
    });

    formlyConfigProvider.setType({
      name: 'formlyBuilderTextarea',
      template: '<textarea class="form-control" ng-model="model[options.key]"></textarea>',
      wrapper: ['formlyBuilderHasError', 'formlyBuilderLabel'],
      defaultOptions: {
        ngModelAttrs: {
          rows: {attribute: 'rows'},
          cols: {attribute: 'cols'}
        }
      },
      apiCheck: check => ({
        templateOptions: {
          rows: check.number.optional,
          cols: check.number.optional
        }
      })
    });
  };
})();
