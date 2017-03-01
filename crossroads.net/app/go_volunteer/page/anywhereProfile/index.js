import CONSTANTS from 'crds-constants';

import goVolunteerAnywhereProfileComponent from './goVolunteerAnywhereProfile.component';
import GoVolunteerAnywhereProfileForm from './goVolunteerAnywhereProfileForm.service';

angular.module(CONSTANTS.MODULES.GO_VOLUNTEER)
  .component('goVolunteerAnywhereProfile', goVolunteerAnywhereProfileComponent())
  .service('GoVolunteerAnywhereProfileForm', GoVolunteerAnywhereProfileForm)
;
