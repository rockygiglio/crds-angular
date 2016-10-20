import CONSTANTS from 'crds-constants';

export default class ServeTeamContainerController {
  /*@ngInject*/
  constructor(ServeTeamService) {
    console.debug('Construct ServeTeamContainerController');
    //this.servingOpportunities = {};
    this.serveTeamService = ServeTeamService;
    this.isLeader = true;
    this.ready = false;
  }

  $onInit()
  {
     this.serveTeamService.getTeamRsvps(this.team).then((team) =>{
       this.team = team;
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