(function() {
  'use strict';
  module.exports = groupToolFormlyBuilderConfig;

  groupToolFormlyBuilderConfig.$inject = ['groupToolFormlyBuilderConfig'];

  function groupToolFormlyBuilderConfig(formlyConfigProvider) {
    formlyConfigProvider.setWrapper({
        name: 'createGroup',
        templateUrl: 'formlyWrappers/createGroupWrapper.html'
    });
  };
})();
