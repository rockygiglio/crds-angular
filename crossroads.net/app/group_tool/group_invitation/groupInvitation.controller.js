
export default class GroupInvitationController {
  /*@ngInject*/
  constructor(GroupService, $state, $log) {
    this.groupService = GroupService;
    this.state = $state;
    this.log = $log;

    this.invitationGUID = null;
    this.ready = false;
    this.error = false;
  }

  $onInit() {
    if (this.state.params.invitationGUID !== undefined && this.state.params.invitationGUID !== null) {
      this.invitationGUID = this.state.params.invitationGUID;
      this.groupService.getGroup(this.groupId).then((data) => {
        this.data = data;
        this.ready = true;
      },
      (err) => {
        this.log.error(`Unable to get group invitation: ${err.status} - ${err.statusText}`);
        this.error = true;
        this.ready = true;
      });
    }
    else {
      //TODO map object posted from create into data object
      this.ready = true;
    }
  }
}