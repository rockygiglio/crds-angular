
export default class MyGroupsController {

  /*@ngInject*/
  constructor(GroupService) {
    this.groupService = GroupService;
    this.groups = [];
    this.ready = false;
    this.error = false;
    this.errorMsg = '';
  }

  $onInit() {
    this.groupService.getMyGroups().then((smGroups) => {
      this.groups = smGroups;
      this.ready = true;
    },
    (err) => {
      this.errorMsg = `Unable to get my groups: ${err.status} - ${err.statusText}`
      console.log(this.errorMsg);
      this.error = true;
      this.ready = true;
    });
  }

}
