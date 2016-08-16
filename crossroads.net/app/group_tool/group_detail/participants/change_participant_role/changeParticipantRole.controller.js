export default class ChangeParticipantRoleController {
  constructor() {
    let participant = this.participant;
    this.processing = false;
  }

  submit() {
    this.processing = true;

    // TODO: Mock-up only, remove for implementation
    window.setTimeout(() => {
      // Invoke the parent callback function
      this.submitAction();
    }, 2000);
  }

  isParticipant() {
    return ! (this.participant.isLeader() && this.participant.isApprentice());
  }

  isLeader() {
    return this.participant.isLeader();
  }

  isApprentice() {
    return this.participant.isApprentice();
  }

  leaderDisabled() {
    return false;
  }

  apprenticeDisabled() {
    return false;
  }

  warningLeaderMax() {
    return false;
  }

  warningLeaderApproval() {
    return false;
  }

  warningApprenticeMax() {
    return false;
  }

  cancel() {
    // Invoke the parent callback function
    this.cancelAction();
  }
}
