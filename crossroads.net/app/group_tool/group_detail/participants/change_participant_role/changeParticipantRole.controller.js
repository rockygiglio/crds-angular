export default class ChangeParticipantRoleController {
  constructor() {
    this.processing = false;
  }

  submit() {
    this.processing = true;
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

  }
}