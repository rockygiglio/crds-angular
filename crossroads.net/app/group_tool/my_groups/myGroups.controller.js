
import SmallGroup from '../model/SmallGroup';

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
      data.forEach(function(group) {
        this.groups.push(new SmallGroup(group));
      }, this);

      this.ready = true;
    },
    (err) => {
      this.log.error(`Unable to get my groups: ${err.status} - ${err.statusText}`);
      this.error = true;
      this.ready = true;
    });
  }

}
