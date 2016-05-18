(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.FORM_BUILDER;

  angular.module(MODULE, ['crossroads.core', 'crossroads.common'])
    .config(require('./formBuilder.routes'))
    .directive('formBuilder', require('./formBuilder.directive'))
    .directive('formField', require('./formField.directive'))
    .controller('FormBuilderDefaultCtrl', require('./formBuilderDefault.controller'))
    .controller('UndividedFacilitatorCtrl', require('./undividedFacilitator.controller'))
    ;

  //Require Templates
  require('./templates/formBuilder.html');
  require('./templates/defaultField.html');
  require('./templates/editableBooleanField.html');
  require('./templates/editableCheckbox.html');
  require('./templates/editableCheckboxGroupField.html');
  require('./templates/editableNumericField.html');
  require('./templates/editableRadioField.html');
  require('./templates/editableTextField.html');
  require('./templates/editableProfileNameTextField.html');
})();
