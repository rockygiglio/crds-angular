export default class GroupDetailRequestsController {
  /*@ngInject*/
  constructor(GroupService, ImageService, $state) {
    this.groupService = GroupService;
    this.imageService = ImageService;
    this.state = $state;

    this.defaultProfileImageUrl = this.imageService.DefaultProfileImage;
    this.groupId = this.state.params.groupId;
    this.ready = false;
    this.error = false;
    this.currentView = 'List';
    this.currentRequest = null;
    this.invite = null;
  }

  $onInit() {
    var imageBaseUrl = this.imageService.ProfileImageBaseURL;
    this.groupService.getGroupRequests(this.groupId).then((data) => {
      this.data = data;
      _.forEach(this.data.requests, function(request) {
          request.imageUrl = `${imageBaseUrl}${request.contactId}`;
      });
      this.ready = true;
    },
    (err) => {
      this.log.error(`Unable to get group requests: ${err.status} - ${err.statusText}`);
      this.error = true;
      this.ready = true;
    });
  }
    
  setView(newView) {
    this.currentView = newView;
  }

  beginInvitation() {
    this.invite = null;
    this.currentView = 'Invite';
  }
    
  sendInvitation(invitation) {
    // TODO Call API to send invitation, etc
    this.invite = null;
    this.currentView = 'List';
  }
    
  beginApproveRequest(request) {
    this.currentRequest = request;
    this.currentView = 'Approve';    
  }
    
  approveRequest(request) {
    // TODO Call API to approve request, send email, etc
    this.currentRequest = null;
    this.currentView = 'List';
  }

  beginDenyRequest(request) {
    this.currentRequest = request;
    this.currentView = 'Deny';    
  }
    
  denyRequest(request) {
    // TODO Call API to deny request, send email, etc
    this.currentRequest = null;
    this.currentView = 'List';
  }
}