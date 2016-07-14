
export default class GroupCardController {
  /*@ngInject*/
  constructor($state) { 
    this.state = $state;
  }
  
  gotoGroupDetail(group) {
    this.state.go('grouptool.detail', { 'groupId': group.id });
  }
}
