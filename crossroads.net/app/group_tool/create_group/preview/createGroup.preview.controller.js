
export default class CreateGroupPreviewController {
  /*@ngInject*/
  constructor(GroupService, ImageService, $state, $log) {
    this.groupService = GroupService;
    this.imageService = ImageService;
    this.state = $state;
    this.log = $log;

    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
    this.ready = false;
    this.error = false;
    this.log.debug('groupService: ', this.groupService.createData.group);
  }

  $onInit() {
    this.groupData = this.groupService.createData.group;
  }
}
