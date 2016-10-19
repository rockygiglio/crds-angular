import CONSTANTS from 'crds-constants';

export default class ServeTeamLeaderController {
  /*@ngInject*/
  constructor(ServeTeamService) {
    console.debug('Construct ServeTeamLeaderController');
    //this.opportunities; from component binding
    this.selectedRole = undefined;
    this.serveTeamService = ServeTeamService;
  }

  $onInit()
  {
      debugger;
      _.each(this.team.serveOpportunities, (opp) => {
          opp.capacity = this.serveTeamService.getCapacity(opp, this.team.eventId);
      });
      debugger;
  }

  loadTeamMembersSearch() {
        console.debug('Query team members');
        // TODO UI!!! IMPLEMENT THIS
        return [
          {
            id: 1001,
            name: 'Genie Simmons',
            email: 'gsimmons@gmail.com',
            phone: '513-313-5984',
            role: 'Leader'
          },
          {
            id: 1002,
            name: 'Holly Gennaro',
            email: 'hgennaro@excite.com',
            phone: '513-857-9587',
            role: null
          },
        ]
      }
}