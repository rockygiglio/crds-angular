export default class GroupDetailController {
  /*@ngInject*/
  constructor($state) {
    this.state = $state;
    this.tabs = [
      { title: 'About', active: false, route: 'grouptool.detail.about' },
      { title: 'Participants', active: false, route: 'grouptool.detail.participants', class: 'hidden-xs' },
      { title: 'Requests', active: false, route: 'grouptool.detail.requests', class: 'hidden-xs' }
    ];
  }

  $onInit() {
    var foundActive = false;
    var currentState = this.state.current.name;
    this.tabs.forEach(function (tab) {
      tab.active = currentState === tab.route;
      if (tab.active) {
        foundActive = true;
      }
    });
    if (!foundActive) {
      this.goToTab(this.tabs[0]);
    }
  }

  goToTab(tab) {
    tab.active = true;
    this.state.go(tab.route);
  }
}