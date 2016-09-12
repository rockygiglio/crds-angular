export default class GroupToolCms {
  /*@ngInject*/
  constructor(Page, ParticipantService, $state) {
    this.page = Page;
    this.participantService = ParticipantService;
    this.state = $state;
    this.link = this.link || '/groups/leader/resources/';
    this.content = '';
  }

  $onInit() {
    this.participantService.get().then((data) => {
      if (_.get(data, 'ApprovedSmallGroupLeader', false)) {
        this.page.get({ url: this.link }).$promise.then((data) => {
          if(data.pages.length > 0) {
            this.content = data.pages[0].content;
          }
        });
      } else {
        this.state.go("content", { "link": "/groups/leader" });
      }
    });
  }
}
