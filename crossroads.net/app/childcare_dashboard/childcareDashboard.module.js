import ChildcareDashboard from './childcareDashboard.component';
import ChildcareRoutes from './childcareDashboard.routes';
import ChildcareDashboardService from './childcareDashboard.service';
import ChildcareIntro from './childcare_intro/childcareIntro.component';
import ChildcareGroup from './childcare_group/childcareGroup.component';
import constants from '../constants';

var childcareModule = angular.module(constants.MODULES.CHILDCARE_DASHBOARD, ['crossroads.core', 'crossroads.common'])
  .config(ChildcareRoutes)
  .component('childcareDashboard', ChildcareDashboard)
  .component('childcareIntro', ChildcareIntro)
  .component('childcareGroup', ChildcareGroup)
  .service('ChildcareDashboardService', ChildcareDashboardService)
  ;

require('./childcareDashboard.html');
require('./childcare_intro/childcareIntro.html');
require('./childcare_group/childcareGroup.html');

export default childcareModule;
