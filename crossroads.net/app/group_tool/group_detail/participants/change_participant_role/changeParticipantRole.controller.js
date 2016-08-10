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

  cancel() {

  }
}