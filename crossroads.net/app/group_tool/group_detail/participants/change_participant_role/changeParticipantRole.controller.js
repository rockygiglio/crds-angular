export default class ChangeParticipantRoleController {
  constructor() {
    this.processing = false;
  }

  submit() {
    this.processing = true;

    // Invoke the parent callback function
    this.submitAction();
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
