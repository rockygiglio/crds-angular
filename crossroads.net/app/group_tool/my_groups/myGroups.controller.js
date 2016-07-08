export default class MyGroupsController {

  /*@ngInject*/
  constructor(GroupService) {
    this.groupService = GroupService;
    this.groups = [];
    this.error = false;
    this.ready = false;
  }

  $onInit() {
    this.groupService.getMyGroups().then((data) => {
      this.groups = data;
      this.ready = true;
    },
    (err) => {
      this.log.error(`Unable to get my groups: ${err.status} - ${err.statusText}`);
      this.error = true;
      this.ready = true;
    });
  }


}
