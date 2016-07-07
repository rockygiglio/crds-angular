export default class GroupDetailParticipantCardController {
  /*@ngInject*/
  constructor(Group, ImageService, $state) {
    this.groupService = Group;
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