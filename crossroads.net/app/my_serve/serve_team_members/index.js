import CONSTANTS from 'crds-constants';

import serveTeamMembersComponent from './serveTeamMembers.component';
import html from './serveTeamMembers.html';
import './serveTeamAutocomplete.html';

export default angular.
module(CONSTANTS.MODULES.MY_SERVE).
component('serveTeamMembers', serveTeamMembersComponent())
;
