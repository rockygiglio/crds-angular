export default class GroupDetailAboutController {
  /*@ngInject*/
  constructor(GroupService, ImageService, $state) {
    this.groupService = GroupService;
    this.imageService = ImageService;
    this.state = $state;

    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
    this.groupId = this.state.params.groupId;
    this.ready = false;
    this.error = false;
  }

  $onInit() {
    this.groupService.getGroup(this.groupId).then((data) => {
      this.data = data;
      var primaryContactId = _.get(this.data, 'primaryContact.contactId');
      this.data.primaryContact.imageUrl = `${this.imageService.ProfileImageBaseURL}${primaryContactId}`;
      this.ready = true;
    },
    (err) => {
      this.log.error(`Unable to get group details: ${err.status} - ${err.statusText}`);
      this.error = true;
      this.ready = true;
    });
  }
}