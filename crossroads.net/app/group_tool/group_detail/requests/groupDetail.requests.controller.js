import CONSTANTS from 'crds-constants';
import GroupInvitation from '../../model/groupInvitation';

export default class GroupDetailRequestsController {
  /*@ngInject*/
  constructor(GroupService, ImageService, $state, $rootScope, $log) {
    this.groupService = GroupService;
    this.imageService = ImageService;
    this.state = $state;
    this.rootScope = $rootScope;
    this.log = $log;

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
    ];

    this.processing = false;
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
    this.processing = false;
    this.invite = new GroupInvitation();
    this.invite.sourceId = this.groupId;
    this.currentView = 'Invite';
  }
    
  sendInvitation(form, invitation) {
    this.processing = true;
    if(!form.$valid) {
      this.processing = false;
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      return;
    }
    invitation.requestDate = new Date();

    this.groupService.sendGroupInvitation(invitation).then(
      (/*data*/) => {
        this.invite = null;
        this.$onInit();
        this.currentView = 'List';
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.emailSent);
      },
      (/*err*/) => {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.emailSendingError);
      }
    ).finally(() => {
      this.processing = false;
    });
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