//import signupIntroComponent from './summercamp/signup_intro/signup_intro.component';
import SummercampComponent from'./summercamp/summercamp.component'
import CampRoutes from './camps.routes';
import constants from '../constants';

var campsModule = angular.module(constants.MODULES.CAMPS, ['crossroads.core', 'crossroads.common'])
  .config(CampRoutes)
  //.component('SignupIntroComponent', signupIntroComponent)
  ;

require('./summercamp');

export default campsModule;
