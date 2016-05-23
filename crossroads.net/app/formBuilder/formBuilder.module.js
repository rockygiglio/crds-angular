(function() {
  'use strict';

  var MODULE = require('crds-constants').MODULES.FORM_BUILDER;

  angular.module(MODULE, ['crossroads.core', 'crossroads.common'])
    .config(require('./formBuilder.routes'))
    .factory('FormBuilderService', require('./formBuilder.service'))
    .directive('formBuilder', require('./formBuilder.directive'))
    .directive('formField', require('./formField.directive'))
    .controller('FormBuilderDefaultCtrl', require('./formBuilderDefault.controller'))
    .controller('UndividedFacilitatorCtrl', require('./undividedFacilitator.controller'))
    ;

  //Require Templates
  require('./templates/formBuilder.html');
  require('./templates/default/defaultField.html');
  require('./templates/default/editableBooleanField.html');
  require('./templates/default/editableCheckbox.html');
  require('./templates/default/editableCheckboxGroupField.html');
  require('./templates/default/editableNumericField.html');
  require('./templates/default/editableRadioField.html');
  require('./templates/default/editableTextField.html');
  require('./templates/groupParticipant/Childcare.html');
  require('./templates/groupParticipant/CoFacilitator.html');
  require('./templates/groupParticipant/FacilitatorTraining.html');  
  require('./templates/groupParticipant/KickOffEvent.html'); 
  require('./templates/groupParticipant/PreferredSession.html'); 
  require('./templates/profile/Email.html');
  require('./templates/profile/Ethnicity.html');  
  require('./templates/profile/Gender.html'); 
  require('./templates/profile/Name.html');
  
})();
