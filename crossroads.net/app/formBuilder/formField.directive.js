(function() {
  'use strict';

  module.exports = FormField;

  FormField.$inject = ['$templateRequest', '$compile'];

  function FormField($templateRequest, $compile) {
    return {
      restrict: 'E',
      scope: {
        field: '=?'
      },
      link: function(scope, element) {
        //var templateUrl = getTemplateUrl(newValue);
        $templateRequest('templates/textField.html').then(function(html) {
          var template = angular.element(html);
          element.append(template);
          $compile(template)(scope);
        });

      },
/*
      templateUrl: function(elem, attrs) {
        console.log(attrs);
        switch (attrs.className) {
          case 'EditableTextField':
            return 'templates/textField.html';
          default:
            return 'templates/formField.html';
        }
      },
*/
      controller: FormFieldController,
      controllerAs: 'formField',
      bindToController: true
    };

    function FormFieldController() {
      var vm = this;

    }

    function getTemplateUrl(className) {
      switch (className) {
        case 'EditableTextField':
          return 'templates/textField.html';
        default:
          return 'templates/formField.html';
      }
    }
  }

})();
