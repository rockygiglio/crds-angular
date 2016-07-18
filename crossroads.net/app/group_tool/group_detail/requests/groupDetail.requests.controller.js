
import CONSTANTS from 'crds-constants';
import GroupInvitation from '../../model/groupInvitation';

export default class GroupDetailRequestsController {
  /*@ngInject*/
  constructor(GroupService, $state, $rootScope, $log) {
    this.groupService = GroupService;
    this.state = $state;
    this.rootScope = $rootScope;
    this.log = $log;

    this.groupId = this.state.params.groupId;
    this.ready = false;
    this.error = false;
    this.currentView = 'List';
    this.invited = [];
    this.inquired = [];

    /*
    Possibly TODO on erasing these

    this.currentRequest = null;

    */
  }

  $onInit() {
    this.getRequests();
  }

  getRequests() {
    this.ready = false;
    this.error = false;

    this.groupService.getInquiries(this.groupId).then((inquiries) => {
      this.inquired = inquiries.filter(function (inquiry) { return inquiry.placed === null || inquiry.placed === undefined; });

      this.groupService.getInvities(this.groupId).then((invitations) => {
        this.invited = invitations;
        this.ready = true;
      },
      (err) => {
        this.rootScope.$emit('notify', `Unable to get group invitations: ${err.status} - ${err.statusText}`);
        this.log.error(`Unable to get group invitations: ${err.status} - ${err.statusText}`);
        this.error = true;
        this.ready = true;
      });
    },
    (err) => {
      this.rootScope.$emit('notify', `Unable to get group inquiries: ${err.status} - ${err.statusText}`);
      this.log.error(`Unable to get group inquiries: ${err.status} - ${err.statusText}`);
      this.error = true;
      this.ready = true;
    });
  }

  setView(newView, refresh) {
    this.currentView = newView;

    if(refresh) {
      this.getRequests();
    }
  }

  listView() {
    return this.currentView === 'List';
  }

  inviteView() {
    return this.currentView === 'Invite';
  }

  approveView() {
    return this.currentView === 'Approve';
  }

  denyView() {
    return this.currentView === 'Deny';
  }

  //////TODO////////////////////////////////////
  /*
  beginApproveRequest(request) {
    this.currentRequest = request;
    this.setView('Approve', false);    
  }
    
  approveRequest(request) {
    // TODO Call API to approve request, send email, etc
    _.remove(this.data.requests, request);
    this.currentRequest = null;
    this.setView('List', true);
  }

  beginDenyRequest(request) {
    this.currentRequest = request;
    this.setView('Deny', false);    
  }
    
  denyRequest(request) {
    // TODO Call API to deny request, send email, etc
    _.remove(this.data.requests, request);
    this.currentRequest = null;
    this.setView('List', true);
  }
  */
}