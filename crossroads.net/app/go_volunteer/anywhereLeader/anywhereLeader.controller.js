export default class AnywhereLeaderController {
  /* @ngInject */
  constructor(GoVolunteerService, $cookies) {
    this.viewReady = false;
    this.project = GoVolunteerService.project;
    this.participants = GoVolunteerService.dashboard || [];
    this.cookies = $cookies;
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
}
