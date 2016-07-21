
export default class CreateGroupPreviewController {
  /*@ngInject*/
  constructor(GroupService, CreateGroupService, Group, ImageService, $state, $log, $rootScope) {
    this.groupService = GroupService;
    this.group = Group;
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
    this.groupData = this.createGroupService.mapSmallGroup();
    this.groupId = '';
  }

  save() {
    this.saving = true;
    this.successfulSave = false;
    try {
      var promise = this.groupService.saveCreateGroupForm(this.groupData);

      // promise.then(function () {
      //   this.rootScope.$emit('notify', this.rootScope.MESSAGES.successfulSubmission);
      //   this.saving = false;
      //   this.successfulSave = true;
      //   //$anchorScroll();
      // },
      //   function (data) {
      //     if (data && data.contentBlockMessage) {
      //       this.rootScope.$emit('notify', data.contentBlockMessage);
      //     } else {
      //       this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      //     }
      //     this.saving = false;
      //     this.successfulSave = false;
      //   }
      // );

      this.state.go('grouptool.mygroups');
    }
    catch (error) {
      this.saving = false;
      this.successfulSave = false;
      throw (error);
    }

  }

  savePersonal() {
    // set oldName to existing email address to work around password change dialog issue
    // this.groupData.profile.person.oldEmail = this.groupData.profile.person.emailAddress;
    // return this.groupData.person.$save();
  }


}
