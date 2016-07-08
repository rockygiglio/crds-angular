export default class GroupDetailRequestsController {
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
    var imageBaseUrl = this.imageService.ProfileImageBaseURL;
    this.groupService.getGroupRequests(this.groupId).then((data) => {
      this.data = data;
      _.forEach(this.data.requests, function(request) {
          request.imageUrl = `${imageBaseUrl}${request.contactId}`;
      });
      this.ready = true;
    },
    (err) => {
      this.log.error(`Unable to get group requests: ${err.status} - ${err.statusText}`);
      this.error = true;
      this.ready = true;
    });
  }
}