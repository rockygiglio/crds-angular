import createGroupComponent from './createGroup.component';
import CONSTANTS from 'crds-constants';
import html from './createGroup.html';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL).
  component('createGroup', createGroupComponent())
  ;

  import createGroupPreview from './preview';
