//import signupIntroComponent from './summercamp/signup_intro/signup_intro.component';
import CampRoutes from './camps.routes';
import constants from '../constants';
import CampComponent from './camp.component';
import CamperInfoComponent from './camper_info/camper_info.component';

var campsModule = angular.module(constants.MODULES.CAMPS, ['crossroads.core', 'crossroads.common'])
  .config(CampRoutes)
  .component('crossroadsCamp', CampComponent)
  .component('camperInfo', CamperInfoComponent)
  ;

export default campsModule;
