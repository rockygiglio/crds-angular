(function() {
  'use strict';

  module.exports = FormBuilderService;

  FormBuilderService.$inject = ['$resource'];

    function FormBuilderService($resource) {      
        return {
           PageView: $resource( __API_ENDPOINT__ +  'api/formbuilder/pages/:pageView'),
          
        }
    }

})()