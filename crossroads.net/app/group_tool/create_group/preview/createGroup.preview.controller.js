
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
    this.edit = false;
    this.saving = false;
  }

  $onInit() {
    this.groupData = this.createGroupService.mapToSmallGroup();

    this.edit = this.groupData.groupId == null || this.groupData.groupId == undefined ? false : true;
  }

  save() {
    this.saving = true;
    this.successfulSave = false;
    debugger;
    try {
      var promise = this.groupService.saveCreateGroupForm(this.groupData)
        .then( (data) => {
          this.saving = false;
          this.successfulSave = true;
          this.createGroupService.resolved = false;
          this.state.go('grouptool.mygroups')
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
          this.saving = false;
          this.successfulSave = true;
          this.createGroupService.resolved = false;
          this.state.go('grouptool.mygroups')
        })
    }
    catch (error) {
      this.saving = false;
      this.successfulSave = false;
      throw (error);
    }

  }

  submit() {
    this.edit ? this.saveEdits() : this.save();
  }

}