import constants from '../constants';
import routes from './goVolunteer.routes';
import formly from './goVolunteer.formly';
import goVolunteerService from './goVolunteer.service';
import goVolunteerOrganizations from './organizations.service';
import goVolunteerDataService from './goVolunteerData.service';
import skillsService from './skills.service';
import groupConnectors from './groupConnectors.service';

export default angular.module(constants.MODULES.GO_VOLUNTEER, ['crossroads.core', 'crossroads.common'])
  .config(routes)
  .config(formly)
  .factory('GoVolunteerService', goVolunteerService)
  .factory('Organizations', goVolunteerOrganizations)
  .factory('GoVolunteerDataService', goVolunteerDataService)
  .factory('SkillsService', skillsService)
  .factory('GroupConnectors', groupConnectors)
  .name
  ;

require('./cms');
require('./city');
require('./organizations');
require('./page');

require('./anywhereProfile');
require('./projectCard');
