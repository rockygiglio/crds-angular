import ChildcareDashboard from './childcareDashboard.component';
import ChildcareRoutes from './childcareDashboard.routes';
import ChildcareDashboardService from './childcareDashboard.service';
import ChildcareIntro from './childcare_intro/childcareIntro.component';
import constants from '../constants';

angular.module(constants.MODULES.CHILDCARE_DASHBOARD, [])
  .config(ChildcareRoutes)
  .component('childcareDashboard', ChildcareDashboard)
  .component('childcareIntro', ChildcareIntro)
  .service('ChildcareDashboardService', ChildcareDashboardService)
  ;

require('./childcareDashboard.html');
require('./childcare_intro/childcareIntro.html');
