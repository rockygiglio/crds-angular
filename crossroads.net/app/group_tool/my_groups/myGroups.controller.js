
export default class MyGroupsController {

  /*@ngInject*/
  constructor(GroupService) {
    this.groupService = GroupService;
    this.groups = [];
    this.ready = false;
    this.error = false;
    this.errorMsg = '';

    this.groupsEven = function() {
      return this.groups.length % 2 == 0;
    };

    this.groupsOdd = function() {
      return this.groups.length % 2 == 1;
    };
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
