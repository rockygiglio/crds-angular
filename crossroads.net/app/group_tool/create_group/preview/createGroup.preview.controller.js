
export default class CreateGroupPreviewController {
  /*@ngInject*/
  constructor(GroupService, CreateGroupService, ImageService, $state, $log) {
    this.groupService = GroupService;
    this.createGroupService = CreateGroupService;
    this.imageService = ImageService;
    this.state = $state;
    this.log = $log;

    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
    this.ready = false;
    this.error = false;
    //this.log.debug('groupService: ', this.groupService.createData.group);
  }

  $onInit() {
    this.groupData = this.createGroupService.mapSmallGroup();
  }

  editGroup() {
    // this.groupService.createData = this.model;
    this.state.go('grouptool.create');
  }

  submitGroup() {
    this.state.go('grouptool.mygroups');
  }

  //ui-sref='grouptool.mygroups'

}
