export default class AnywhereLeaderController {
  /* @ngInject */
  constructor(GoVolunteerService) {
    this.viewReady = false;
    this.project = GoVolunteerService.project;
    this.participants = GoVolunteerService.dashboard || [];
  }

  $onInit() {
    this.viewReady = true;
  }

  totalParticipants() {
    return this.participants
            .map(participant => participant.adults + participant.children)
            .reduce((acc, val) => acc + val, 1); // start at 1 to account for leader
  }
}
