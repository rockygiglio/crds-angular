export default class GroupToolCms {
  /*@ngInject*/
  constructor(Page) {
    this.page = Page;
    this.link = this.link || '/groups/leader/resources/';
    this.content = '';
  }

  $onInit() {
    let cms = this.page.get({ url: this.link });

    cms.$promise.then((data) => {
      if(data.pages.length > 0) {
        this.content = data.pages[0].content;
      }
    });
  }
}
