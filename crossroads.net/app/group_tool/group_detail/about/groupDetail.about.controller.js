
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
    this.showFooter = false;

    this.forInvitation = (this.forInvitation === undefined || this.forInvitation === null) ? false : this.forInvitation;
    this.forSearch = (this.forSearch === undefined || this.forSearch === null) ? false : this.forSearch;
  }

  $onInit() {
    this.groupId = this.state.params.groupId || this.data.groupId;

    this.showFooter = this.state.params.groupId !== undefined && this.state.params.groupId !== null;
    if (this.showFooter) {
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

  //this is not efficient, gets called every time the digest cycle runs
  getAddress() {
    if(!this.ready) {
      return undefined;
    }

    if(!this.data.hasAddress()) {
      return 'Online';
    }

    if (!this.userInGroup()) {
      return `${this.data.address.city}, ${this.data.address.state} ${this.data.address.zip}`;
    } else {
      return this.data.address.toString();
    }
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

  goToEdit() {
    this.state.go('grouptool.edit', {groupId: this.state.params.groupId});
  }

  goToEnd() {
    this.state.go('grouptool.end-group', {groupId: this.state.params.groupId});
  }
}