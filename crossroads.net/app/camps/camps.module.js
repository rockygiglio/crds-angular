import CampRoutes from './camps.routes';
import constants from '../constants';
import CampComponent from './camp.component';
import CamperInfoComponent from './camper_info/camper_info.component';
import CampsDashboardComponent from './dashboard/camps_dashboard.component';
import CampCardComponent from './dashboard/camp_card/camp_card.component';

import CampsService from './camps.service';
import CamperInfoForm from './camper_info/camper_info_form.service';

export default angular.module(constants.MODULES.CAMPS, ['crossroads.core', 'crossroads.common'])
  .config(CampRoutes)
  .component('crossroadsCamp', CampComponent)
  .component('camperInfo', CamperInfoComponent)
  .component('campsDashboard', CampsDashboardComponent)
  .component('campCard', CampCardComponent)

  .service('CampsService', CampsService)
  .service('CamperInfoForm', CamperInfoForm);