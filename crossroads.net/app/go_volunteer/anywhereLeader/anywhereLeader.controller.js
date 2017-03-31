export default class AnywhereLeaderController {
  /* @ngInject */
  constructor($rootScope, $state, $log, $cookies, GoVolunteerService, GoVolunteerDataService, FileSaver) {
    this.rootScope = $rootScope;
    this.state = $state;
    this.log = $log;
    this.cookies = $cookies;
    this.viewReady = false;
    this.project = GoVolunteerService.project;
    this.participants = GoVolunteerService.dashboard || [];
    this.goVolunteerDataService = GoVolunteerDataService;
    this.fileSaver = FileSaver;
    this.unauthorized = false;
  }

  $onInit() {
    const userId = this.cookies.get('userId');
    if (userId !== this.project.contactId.toString()) {
      this.unauthorized = true;
    }
    this.viewReady = true;
  }

  showDashboard() {
    return (this.viewReady && !this.unauthorized);
  }

  totalParticipants() {
    return this.participants
            .map(participant => participant.adults + participant.children)
            .reduce((acc, val) => acc + val, 1); // start at 1 to account for leader
  }

  getExport() {
    return this.goVolunteerDataService.getDashboardExport(this.state.toParams.projectId).then((result) => {
      this.fileSaver.saveAs(result.response.blob, result.response.filename);
    }).catch(() => {
      this.log.error;
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.gpexport_generation_error);
    });
  }
}
