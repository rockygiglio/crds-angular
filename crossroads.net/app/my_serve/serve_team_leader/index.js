import CONSTANTS from 'crds-constants';

import serveTeamLeaderComponent from './serveTeamLeader.component';
import html from './serveTeamLeader.html';
import './serveTeamAutocomplete.html';

export default angular.
module(CONSTANTS.MODULES.MY_SERVE).
component('serveTeamLeader', serveTeamLeaderComponent())
;
