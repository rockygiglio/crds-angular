import editGroupComponent from './editGroup.component';
import createGroupPreviewComponent from './preview/createGroup.preview.component';
import CONSTANTS from 'crds-constants';
/* jshint unused: false */
import createGroupHtml from './createGroup.html';
import editGroupHtml from './editGroup.html';
import previewGroupHtml from './preview/createGroup.preview.html';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL)
  .component('editGroup', editGroupComponent())
  .component('editGroupPreview', editGroupPreviewComponent())
  ;

