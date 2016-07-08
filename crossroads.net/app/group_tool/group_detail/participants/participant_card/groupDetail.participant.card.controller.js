export default class GroupDetailParticipantCardController {
  /*@ngInject*/
  constructor(GroupService, ImageService, $state) {
    this.groupService = GroupService;
    this.imageService = ImageService;
    this.state = $state;
  }

  $onInit() {
    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
  }

  markParticipantDeleted(participant) {
    participant.deleted = true;
  }
}