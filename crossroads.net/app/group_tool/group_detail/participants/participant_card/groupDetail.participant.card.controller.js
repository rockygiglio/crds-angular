export default class GroupDetailParticipantCardController {
  /*@ngInject*/
  constructor(ImageService) {
    this.imageService = ImageService;
  }

  $onInit() {
    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
  }

  invokeDeleteAction(participant) {
    this.deleteAction({participant: participant});
  }

  invokeRoleAction(participant) {
    console.debug("Invoke Role Action");
    this.roleAction({participant: participant});
  }
}