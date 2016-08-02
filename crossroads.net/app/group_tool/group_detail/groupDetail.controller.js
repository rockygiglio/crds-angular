export default class GroupDetailController {
  /*@ngInject*/
  constructor(GroupService, $state) {
    this.groupService = GroupService;
    this.state = $state;
    this.ready = false;
    this.tabs = [];
  }

  $onInit() {
    this.groupService.getIsLeader(this.state.params.groupId).then((isLeader) => {
      this.tabs.push({ title: 'About', active: false, route: 'grouptool.detail.about' });
      this.tabs.push({ title: 'Participants', active: false, route: 'grouptool.detail.participants', class: 'hidden-xs' });

      if(isLeader){
        this.tabs.push({ title: 'Requests', active: false, route: 'grouptool.detail.requests', class: 'hidden-xs' });
      }

      let foundActive = false;
      let currentState = this.state.current.name;
      this.tabs.forEach(function (tab) {
        tab.active = currentState === tab.route;
        if (tab.active) {
          foundActive = true;
        }
      });
      if (!foundActive) {
        this.goToTab(this.tabs[0]);
      }
    }).finally(() => {
      this.ready = true;
    });
  }

  goToTab(tab) {
    tab.active = true;
    this.state.go(tab.route);
  }
}