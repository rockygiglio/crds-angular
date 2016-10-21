import CONSTANTS from 'crds-constants';

export default class ServeTeamContainerController {
  /*@ngInject*/
  constructor(ServeTeamService, $q) {
    console.debug('Construct ServeTeamContainerController');
    //this.servingOpportunities = {};
    this.isLeader = false;
    this.serveTeamService = ServeTeamService;
    this.ready = false;
    this.qApi = $q;
  }

  $onInit()
  {
    this.qApi.all([this.serveTeamService.getIsLeader(this.team.groupId), this.serveTeamService.getTeamRsvps(this.team)]).then((results) => {
      this.isLeader = results[0].isLeader;
      this.team = results[1];
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