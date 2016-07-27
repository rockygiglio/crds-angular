(function() {
  'use strict';
  module.exports = formlyBuilderConfig;

  formlyBuilderConfig.$inject = ['formlyConfigProvider'];

  function formlyBuilderConfig(formlyConfigProvider) {
    // formlyConfigProvider.setWrapper([{
    //     template: [
    //       '<formly-transclude></formly-transclude>',
    //       '<div class="validation"',
    //       '  ng-if="showError"',
    //       '  ng-messages="fc.$error">',
    //       '  <div ng-message="{{::name}}" ng-repeat="(name, message) in ::options.validation.messages">',
    //       '    {{message(fc.$viewValue, fc.$modelValue, this)}}',
    //       '  </div>',
    //       '</div>'
    //     ].join(' ')}
    //   ]);
  };
})();
