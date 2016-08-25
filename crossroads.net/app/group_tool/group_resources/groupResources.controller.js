
export default class GroupResourcesController {
  /*@ngInject*/
  constructor(GroupResourcesService, $rootScope) {
    this.groupResourcesService = GroupResourcesService;
    this.rootScope = $rootScope;
    this.ready = false;
  }

  $onInit() {
    this.groupResourcesService.getGroupResources().then((resources) => {
      this.groupcats = resources;
    }, (err) => {
      this.groupCats = [];
    }).finally(() => {
      this.ready = true;
    });
  }
}