export default class ServeTeamMessageController {
  /* @ngInject */
  constructor(ServeTeamService) {
    console.debug('Serve Team Message controller');
    this.serveTeamService = ServeTeamService;
    this.processing = false;
    this.selection = null;
    this.individuals = [];
  }

  $onInit(){
    this.teams = this.serveTeamService.getTeamDetailsByLeader();
    this.teamPeople = this.serveTeamService.getAllTeamMembersByLeader();
  }

  loadIndividuals($query) {
    return this.teamPeople;
  }

  submit() {
    this.processing = true;
  }
}
