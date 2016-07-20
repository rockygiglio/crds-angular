
export default class CreateGroupPreviewController {
  /*@ngInject*/
  constructor(GroupService, CreateGroupService, Group, ImageService, $state, $log) {
    this.groupService = GroupService;
    this.group = Group;
    this.createGroupService = CreateGroupService;
    this.imageService = ImageService;
    this.state = $state;
    this.log = $log;

    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
    this.ready = false;
    this.error = false;
  }

  $onInit() {
    this.groupData = this.createGroupService.mapSmallGroup();
    this.groupId = '';
  }

  save() {
    this.saving = true;
    this.successfulSave = false;
    try {
// TODO validate form
      // var promise = savePersonal();
      // promise = promise.then(saveGroup);
      // promise = promise.then(saveParticipantLeader);

      this.state.go('grouptool.mygroups');
    }
    catch (error) {
      vm.saving = false;
      vm.successfulSave = false;
      throw (error);
    }

  }

  savePersonal() {
    // set oldName to existing email address to work around password change dialog issue
    // this.groupData.profile.person.oldEmail = this.groupData.profile.person.emailAddress;
    // return this.groupData.person.$save();
  }

  saveGroup() {
    this.groupId = '';
  }

  saveParticipantLeader() {
    return this.group.Participant.save({
        groupId: this.groupId,
        },
        this.participants).$promise;
  }

}
