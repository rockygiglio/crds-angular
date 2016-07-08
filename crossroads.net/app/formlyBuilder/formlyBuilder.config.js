(function() {
  'use strict';
  module.exports = formlyBuilderConfig;

  formlyBuilderConfig.$inject = ['formlyConfigProvider'];

  function formlyBuilderConfig(formlyConfigProvider) {
    formlyConfigProvider.setWrapper({
        name: 'createGroup',
        templateUrl: 'wrappers/createGroupWrapper.html'
    });
  };
})();
