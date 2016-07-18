
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

    editGroup() {
   // this.groupService.createData = this.model;
    this.state.go('grouptool.create');
  }

    submitGroup() {
       this.state.go('grouptool.mygroups');
 }

  savePersonal() {
      // set oldName to existing email address to work around password change dialog issue
      this.data.profileData.person.oldEmail = this.data.profileData.person.emailAddress;
      return this.data.profileData.person.$save();
  }

  createGroup() {

  }

  addParticipant() {
    var participants = [this.data.groupParticipant];

    return group.Participant.save({
        groupId: this.data.group.groupId,
      },
      participants).$promise;
  }
}
