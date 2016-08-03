
export default class GroupCardController {
  /*@ngInject*/
  constructor($state) { 
    this.state = $state;
  }

  goToInvite() {
    this.state.go('grouptool.detail.requests', {groupId: this.group.groupId});
  }
}
