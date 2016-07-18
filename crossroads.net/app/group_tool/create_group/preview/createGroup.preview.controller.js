
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
  }

  $onInit() {
    if (this.state.params.groupId !== undefined && this.state.params.groupId !== null) {
      this.groupId = this.state.params.groupId;
      this.groupService.getGroup(this.groupId).then((data) => {
        this.data = data;
        var primaryContactId = this.data.contactId;
        this.data.primaryContact = {
          imageUrl: `${this.imageService.ProfileImageBaseURL}${primaryContactId}`,
          contactId: primaryContactId
        };
        this.ready = true;
      },
      (err) => {
        this.log.error(`Unable to get group details: ${err.status} - ${err.statusText}`);
        this.error = true;
        this.ready = true;
      });
    }
    else {
      //TODO map object posted from create into data object
      this.ready = true;
    }

  }
}