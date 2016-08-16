import CONSTANTS from '../../../../constants';

export default class ChangeParticipantRoleController {
  constructor(GroupService) {
    this.groupService = GroupService;
    let participant = this.participant;
    this.processing = false;
  }

  submit() {
    this.processing = true;
    if (this.isParticipant()){
      this.setParticipantRole();
    }
    else if (this.isLeader()){
      this.setLeaderRole();
    }
    else if (this.isApprentice()) {
      this.setApprenticeRole();
    }
    var promise = this.groupService.updateParticipant(this.participant)
      .then((data) => {
        this.processing = false;
        this.state.go('grouptool.mygroups');
      });
  }

  setParticipantRole() {
    this.participant.groupRoleId = CONSTANTS.GROUP.ROLES.MEMBER;
  }

  isParticipant() {
    return this.participant.groupRoleId === CONSTANTS.GROUP.ROLES.MEMBER;
  }

  setLeaderRole() {
    this.participant.groupRoleId = CONSTANTS.GROUP.ROLES.LEADER;
  }

  isLeader() {
    return (this.participant.groupRoleId === CONSTANTS.GROUP.ROLES.LEADER);
  }

  setApprenticeRole() {
    this.participant.groupRoleId = CONSTANTS.GROUP.ROLES.APPRENTICE;
  }
  isApprentice() {
    return (this.participant.groupRoleId === CONSTANTS.GROUP.ROLES.APPRENTICE);
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
