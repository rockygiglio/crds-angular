import cardComponent from './card.component';
import CONSTANTS from 'crds-constants';
import html from './card.html';

export default angular.
  module(CONSTANTS.MODULES.GROUP_TOOL).
  directive('card', cardComponent);
