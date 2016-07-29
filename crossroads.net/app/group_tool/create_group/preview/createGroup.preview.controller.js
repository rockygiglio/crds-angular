
export default class CreateGroupPreviewController {
  /*@ngInject*/
  constructor(GroupService, CreateGroupService, ImageService, $state, $log, $rootScope) {
    this.groupService = GroupService;
    this.createGroupService = CreateGroupService;
    this.imageService = ImageService;
    this.state = $state;
    this.log = $log;
    this.rootScope = $rootScope;

    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
    this.ready = false;
    this.error = false;
  }

  $onInit() {
    this.groupData = this.createGroupService.mapToSmallGroup();
    this.groupId = '';
  }

  save() {
    this.saving = true;
    this.successfulSave = false;
    try {
      var promise = this.groupService.saveCreateGroupForm(this.groupData)
        .then( (data) => {
          this.state.go('grouptool.mygroups')
          CreateGroupService.resolved = false;
        })
    }
    catch (error) {
      this.saving = false;
      this.successfulSave = false;
      throw (error);
    }

  }

  saveEdits() {
    this.saving = true;
    this.successfulSave = false;
    try {
      var promise = this.groupService.saveEditGroupForm(this.groupData)
        .then( (data) => {
          this.state.go('grouptool.mygroups')
          CreateGroupService.resolved = false;
        })
    }
    catch (error) {
      this.saving = false;
      this.successfulSave = false;
      throw (error);
    }

  }

}