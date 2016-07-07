export default class GroupDetailAboutController {
  /*@ngInject*/
  constructor(Group, ImageService) {
    this.groupService = Group;
    this.imageService = ImageService;
    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
    this.ready = false;
    this.error = false;
  }

  $onInit() {
    this.groupService.getGroup(123).then((data) => {
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