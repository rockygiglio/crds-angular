
export default class GroupInvitationController {
  /*@ngInject*/
  constructor(GroupService, ParticipantService, $state, $rootScope, $log) {
    this.groupService = GroupService;
    this.participantService = ParticipantService;
    this.state = $state;
    this.log = $log;
    this.rootScope = $rootScope;

    this.ready = false;
    this.error = false;
    this.processing = false;
    this.group = null;
  }

  $onInit() {
    if (this.state.params.invitationGUID !== undefined && this.state.params.invitationGUID !== null) {
      this.invitationGUID = this.state.params.invitationGUID;

      this.groupService.getGroupByInvitationGUID(this.invitationGUID).then((data) => {
        this.group = data;
        this.ready = true;
      },
      (err) => {
        this.log.error(`Unable to get group invitation: ${err.status} - ${err.statusText}`);
        this.error = true;
        this.ready = true;
      });
    }
    else {
      this.ready = true;
    }
  }

  invitationExists() {
    return this.group !== null;
  }

  accept() {
    this.processing = true;

    this.participantService.acceptDenyInvitation(this.group.groupId, this.invitationGUID, true).then(() => {
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.groupToolAcceptInvitationSuccessGrowler);
    },
    (err) => {
      this.log.error(`Unable to accept group Invitation: ${err.status} - ${err.statusText}`);
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.groupToolAcceptInvitationFailureGrowler);
    }).finally(() => {
      this.processing = false;
    });
  }

  deny() {
    this.processing = true;

    this.participantService.acceptDenyInvitation(this.group.groupId, this.invitationGUID, false).then(() => {
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.groupToolDenyInvitationSuccessGrowler);
    },
    (err) => {
      this.log.error(`Unable to revoke group Invitation: ${err.status} - ${err.statusText}`);
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.groupToolDenyInvitationFailureGrowler);
    }).finally(() => {
      this.processing = false;
    });
  }

}