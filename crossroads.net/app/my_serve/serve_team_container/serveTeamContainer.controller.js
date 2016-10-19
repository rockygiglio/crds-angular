import CONSTANTS from 'crds-constants';

export default class ServeTeamContainerController {
  /*@ngInject*/
  constructor(ServeTeamService) {
    console.debug('Construct ServeTeamContainerController');
    //this.servingOpportunities = {};
    this.serveTeamService = ServeTeamService;
    this.ready = false;
  }

  $onInit()
  {
     this.serveTeamService.getTeamRsvps(this.team).then((team) =>{
       debugger;
       this.servingOpportunities = team;
       this.ready = true;
    });
  }

  memberClick(member) {
    console.debug('member click', member);
    this.onMemberClick({ $member: member });
  }

  memberRemove(member) {
    console.debug('member remove', member);
    this.onMemberRemove({ $member: member });
  }
}