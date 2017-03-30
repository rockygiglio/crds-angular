export default class AnywhereLeaderController {
  /* @ngInject */
  constructor($state, $log, GoVolunteerService, GoVolunteerDataService, FileSaver) {
    this.state = $state;
    this.log = $log;
    this.viewReady = false;
    this.project = GoVolunteerService.project;
    this.participants = GoVolunteerService.dashboard || [];
    this.goVolunteerDataService = GoVolunteerDataService;
    this.fileSaver = FileSaver;
  }

  $onInit() {
    this.viewReady = true;
  }

  totalParticipants() {
    return this.participants
            .map(participant => participant.adults + participant.children)
            .reduce((acc, val) => acc + val, 1); // start at 1 to account for leader
  }

  getExport() {
    return this.goVolunteerDataService.getDashboardExport(this.state.toParams.projectId).then((result) => {
      this.fileSaver.saveAs(result.response.blob, result.response.filename);
    }).catch(this.log.error);
  }
}
