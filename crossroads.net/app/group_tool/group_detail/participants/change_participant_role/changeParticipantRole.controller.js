export default class ChangeParticipantRoleController {
  constructor() {
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
    return true;
  }

  isLeader() {
    return false;
  }

  isApprentice() {
    return false;
  }

  leaderDisabled() {
    return true;
  }

  apprenticeDisabled() {
    return true;
  }

  warningLeaderMax() {
    return true;
  }

  warningLeaderApproval() {
    return false;
  }

  warningApprenticeMax() {
    return true;
  }

  cancel() {
    // Invoke the parent callback function
    this.cancelAction();
  }
}
