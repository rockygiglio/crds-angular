import signupIntroComponent from './signup_intro.component';
import CONSTANTS from 'crds-constants';
import html from './signup_intro.html';

export default angular.
  module(CONSTANTS.MODULES.CAMPS)
    .component('signupIntroComponent', signupIntroComponent())
  ;