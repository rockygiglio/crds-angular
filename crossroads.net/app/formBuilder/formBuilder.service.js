(function() {
  'use strict';

  module.exports = FormBuilderService;
  FormBuilderService.$inject = ['$resource'];

  function FormBuilderService($resource) {
    return {
      Groups: $resource(__GATEWAY_CLIENT_ENDPOINT__ +  'api/formbuilder/groups/:templateType'),
      Attribute: $resource(__GATEWAY_CLIENT_ENDPOINT__ +  'api/attributetype/:attributeTypeId'),
    };
  }
})();
