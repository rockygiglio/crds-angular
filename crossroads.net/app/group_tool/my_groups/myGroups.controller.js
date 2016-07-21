
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
      // this.groups = smGroups;
      // this.groups = smGroups.slice(0,1);

      let testId = 1;
      this.groups = smGroups;
      angular.forEach(this.groups, function(group) {
        let fake = angular.copy(group);
        fake.groupId = testId++;
        smGroups.push(fake);
      });

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
