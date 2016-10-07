import signupIntroComponent from './signup_intro.component';
import constants from '../../../constants';
import html from './signup_intro.html';

export default angular.
  module(constants.MODULES.CAMPS)
    .component('signupIntro', signupIntroComponent)
  ;