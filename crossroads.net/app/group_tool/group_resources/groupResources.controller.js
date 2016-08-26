
export default class GroupResourcesController {
  /*@ngInject*/
  constructor(GroupResourcesService, $rootScope) {
    this.groupResourcesService = GroupResourcesService;
    this.rootScope = $rootScope;
    this.categories = [];
    this.ready = false;
  }

  $onInit() {
    this.groupResourcesService.getGroupResources().then((resources) => {
      // Filter off any categories without resources - in case anyone wants to
      // add categories before they have resources ready for the category.
      this.categories = resources.filter((cat) => {
        return cat.hasResources() === true;
      });
    }, (/*err*/) => {
      this.categories = [];
    }).finally(() => {
      this.categories = [];
      this.ready = true;
    });
  }

  hasData() {
    return this.categories.length > 0;
  }

  getCategories() {
    return this.categories;
  }
}