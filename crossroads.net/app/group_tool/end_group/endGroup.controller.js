
export default class EndGroupController {
  /*@ngInject*/
  constructor(GroupService, $state, $log, $cookies) {
    this.groupService = GroupService;
    this.state = $state;
    this.log = $log;
    this.cookies = $cookies;
    this.ready = false;
    this.error = false;
    this.isLeader = false;
    this.groupId = null;
  }

  $onInit() {
    this.groupId = this.state.params.groupId || this.data.groupId;

    // TODO map object posted from create into data object, then call this.setGroupImageUrl()
    this.ready = true;
  }

  cancel() {
    this.state.go('grouptool.detail.about', { groupId: this.groupId });
  }

}