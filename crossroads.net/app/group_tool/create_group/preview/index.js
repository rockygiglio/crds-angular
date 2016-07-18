import createGroupPreviewComponent from './createGroup.preview.component';
import CONSTANTS from 'crds-constants';
import html from './createGroup.preview.html';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL).
  component('createGroupPreview', createGroupPreviewComponent())
  ;