
export default class GroupDetailAboutController {
  /*@ngInject*/
  constructor(GroupService, ImageService, $state, $log) {
    this.groupService = GroupService;
    this.imageService = ImageService;
    this.state = $state;
    this.log = $log;

    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
    this.groupId = this.state.params.groupId;
    this.ready = false;
    this.error = false;
  }

  $onInit() {
    if (this.groupId != null) {
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
    //else - use data object from binding
    //may need to do some mapping into object
    //this.data = stuff from binding
  }
}