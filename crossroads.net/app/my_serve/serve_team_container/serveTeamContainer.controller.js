export default class ServeTeamContainerController {
  /*@ngInject*/
  constructor(ServeTeamService, $q) {
    //this.team; from binding
    this.isLeader = false;
    this.serveTeamService = ServeTeamService;
    this.ready = false;
    this.qApi = $q;
  }

  $onInit() {
    let promises = [this.serveTeamService.getIsLeader(this.team.groupId), this.serveTeamService.getTeamRsvps(this.team)];
    this.qApi.all(promises).then((results) => {
      this.isLeader = results[0].isLeader;
      this.team = results[1];
      this.ready = true;
    });
  }

  memberClick(member) {
    this.onMemberClick({ $member: member });
  }

  memberRemove(member) {
    this.onMemberRemove({ $member: member });
  }
}