
export default class GroupDetailAboutController {
  /*@ngInject*/
  constructor(GroupService, ImageService, $state, $log, $cookies) {
    this.groupService = GroupService;
    this.imageService = ImageService;
    this.state = $state;
    this.log = $log;
    this.cookies = $cookies;

    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
    this.ready = false;
    this.error = false;
    this.isLeader = false;

    this.forInvitation = (this.forInvitation === undefined || this.forInvitation === null) ? false : this.forInvitation;
  }

  $onInit() {
    this.groupId = this.state.params.groupId || this.data.groupId;

    if (this.state.params.groupId !== undefined && this.state.params.groupId !== null) {
      this.groupService.getGroup(this.groupId).then((data) => {
        this.data = data;
        this.setGroupImageUrl();
        this.groupService.getIsLeader(this.groupId).then((isLeader) => {
          this.isLeader = isLeader;
        });        
      },
      (err) => {
        this.log.error(`Unable to get group details: ${err.status} - ${err.statusText}`);
        this.error = true;
      }).finally(() => {
        this.ready = true;
      });
    } else if(this.data != null) {
      this.setGroupImageUrl();
      this.ready = true;
    } else {
      // TODO map object posted from create into data object, then call this.setGroupImageUrl()
      //this.setGroupImageUrl();
      this.ready = true;
    }
  }

  setGroupImageUrl() {
    var primaryContactId = this.data.contactId;
    this.data.primaryContact = {
      imageUrl: `${this.imageService.ProfileImageBaseURL}${primaryContactId}`,
      contactId: primaryContactId
    };
  }

  groupExists() {
    if (this.groupId !== undefined && this.groupId !== null) {
      return true;
    }
    else {
      return false;
    }
  }

  userInGroup() {
    if (this.data){
      return this.data.participantInGroup(this.cookies.get("userId"));
    }
    return false;
  }
}