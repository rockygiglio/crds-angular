import CONSTANTS from 'crds-constants';

import organizationsComponent from './organizations.component';

angular.module(CONSTANTS.MODULES.GO_VOLUNTEER)
  .component('goVolunteerOrganizations', organizationsComponent())
  ;
