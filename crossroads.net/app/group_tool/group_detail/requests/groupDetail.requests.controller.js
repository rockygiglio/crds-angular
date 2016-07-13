import CONSTANTS from 'crds-constants';
import GroupInvitation from '../../model/groupInvitation';

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
    this.groupParticipantRoles = [
      { 'id': CONSTANTS.GROUP.ROLES.MEMBER, 'label': 'Participant' },
      { 'id': CONSTANTS.GROUP.ROLES.LEADER, 'label': 'Co-Leader' },
      { 'id': CONSTANTS.GROUP.ROLES.APPRENTICE, 'label': 'Apprentice' }
    ]
  }

  $onInit() {
    this.ready = false;
    this.error = false;

    this.groupService.getGroupRequests(this.groupId).then((data) => {
      this.data = data;
      this.data.requests.forEach(function(request) {
          request.imageUrl = `${this.imageService.ProfileImageBaseURL}${request.contactId}`;
      }, this);
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
    this.invite = new GroupInvitation();
    this.invite.sourceId = this.groupId;
    this.currentView = 'Invite';
  }
    
  sendInvitation(form, invitation) {
    if(!form.$valid) {
      return;
    }
    this.invite.requestDate = new Date();

    // TODO Call API to send invitation, etc
    this.invite = null;
    this.$onInit();
    this.currentView = 'List';
  }
    
  beginApproveRequest(request) {
    this.currentRequest = request;
    this.currentView = 'Approve';    
  }
    
  approveRequest(request) {
    // TODO Call API to approve request, send email, etc
    _.remove(this.data.requests, request);
    this.currentRequest = null;
    this.currentView = 'List';
  }

  beginDenyRequest(request) {
    this.currentRequest = request;
    this.currentView = 'Deny';    
  }
    
  denyRequest(request) {
    // TODO Call API to deny request, send email, etc
    _.remove(this.data.requests, request);
    this.currentRequest = null;
    this.currentView = 'List';
  }
}