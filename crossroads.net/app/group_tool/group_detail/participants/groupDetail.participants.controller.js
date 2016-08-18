import GroupMessage from '../../model/groupMessage';

export default class GroupDetailParticipantsController {
  /*@ngInject*/
  constructor(GroupService, ImageService, $state, $log, ParticipantService, $rootScope, MessageService) {
    this.groupService = GroupService;
    this.imageService = ImageService;
    this.state = $state;
    this.log = $log;
    this.participantService = ParticipantService;
    this.rootScope = $rootScope;
    this.messageService = MessageService;

    this.groupId = this.state.params.groupId;
    this.ready = false;
    this.error = false;
    this.processing = false;
    this.isLeader = false;
    this.data = [];
    this.countLeaders = 0;
    this.countApprentice = 0;

    this.setListView();
  }

  $onInit() {
    this.participantService.get().then((myParticipant) => {
      this.myParticipantId = myParticipant.ParticipantId;
      this.loadGroupParticipants();
    }, (err) => {
      this.log.error(`Unable to get my participant: ${err.status} - ${err.statusText}`);
      this.error = true;
      this.ready = true;
    });

    this.groupService.getIsLeader(this.groupId).then((isLeader) => {
      this.isLeader = isLeader;
    })
  }

 getApprenticeCount() {
    if (!this.data) {
      return 0;
    }
    return this.data.filter(function (val) {
      //TODO inject constants
      //return val === CONSTANTS.GROUP.ROLES.APPRENTICE;
      return val.groupRoleId === 66;
    }).length;
  }

 getLeaderCount() {
    if (!this.data) {
      return 0;
    }
    return this.data.filter(function (val) {
      //TODO inject constants
      //return val === CONSTANTS.GROUP.ROLES.LEADER;
      return val.groupRoleId === 22;
    }).length;
  }

  loadGroupParticipants() {
    //look for existing smallGroup object if already hit mygroups
    this.groupService.getGroupParticipants(this.groupId).then((data) => {
      this.data = data.slice().sort((a, b) => {
        return(a.compareTo(b));
      });
      this.data.forEach(function(participant) {
        participant.me = participant.participantId === this.myParticipantId;
        participant.imageUrl = `${this.imageService.ProfileImageBaseURL}${participant.contactId}`;
      }, this);
debugger;
      this.ready = true;
      //TODO move off of rootScope
      this.countLeaders = this.getLeaderCount();
      this.rootScope.countLeaders = this.countLeaders;
      this.countApprentice = this.getApprenticeCount();
      this.rootScope.countApprentice = this.countApprentice;
    },
    (err) => {
      this.log.error(`Unable to get group participants: ${err.status} - ${err.statusText}`);
      this.error = true;
      this.ready = true;
    });
  }

  setDeleteView() {
    this.currentView = 'Delete';
  }

  isDeleteView() {
    return this.currentView === 'Delete';
  }

  setEditView() {
    this.currentView = 'Edit';
  }

  isEditView() {
    return this.currentView === 'Edit';
  }

  setListView() {
    this.currentView = 'List';
  }

  isListView() {
    return this.currentView === 'List';
  }

  setEmailView() {
    this.currentView = 'Email';
  }

  isEmailView() {
    return this.currentView === 'Email';
  }

  setRoleView() {
    this.currentView = 'Role';
  }

  isRoleView() {
    return this.currentView === 'Role';
  }

  beginRemoveParticipant(participant) {
    this.deleteParticipant = participant;
    this.deleteParticipant.message = '';
    this.setDeleteView();
  }

  cancelRemoveParticipant(participant) {
    participant.message = undefined;
    this.deleteParticipant = undefined;
    this.setEditView();
  }

  removeParticipant(person) {
    this.log.info(`Deleting participant: ${JSON.stringify(person)}`);
    this.processing = true;

    this.groupService.removeGroupParticipant(this.groupId, person).then(() => {
      _.remove(this.data, function(p) {
          return p.groupParticipantId === person.groupParticipantId;
      });
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.groupToolRemoveParticipantSuccess);
      this.setListView();
      this.deleteParticipant = undefined;
      this.ready = true;
    },
    (err) => {
      this.log.error(`Unable to remove group participant: ${err.status} - ${err.statusText}`);
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.groupToolRemoveParticipantFailure);
      this.error = true;
      this.ready = true;
    }).finally(() => {
      this.processing = false;
    });
  }

  beginChangeParticipantRole(participant) {
    this.roleParticipant = participant;
    this.roleParticipant.message = '';
    this.setRoleView();
  }

  finishChangeParticipantRole() {
    this.roleParticipant = undefined;
    this.setEditView();
  }

  beginMessageParticipants() {
    this.groupMessage = new GroupMessage();
    this.groupMessage.groupId = '';
    this.groupMessage.subject = '';
    this.groupMessage.body = '';
    this.setEmailView();
  }

  cancelMessageParticipants(message) {
    this.groupMessage = undefined;
    this.setListView();
  }

  messageParticipants(message) {

    // TODO: Fill in implementation
    this.processing = true;

    this.messageService.sendGroupMessage(this.groupId, message).then(
        () => {
          this.groupMessage = undefined;
          this.$onInit();
          this.currentView = 'List';
          this.rootScope.$emit('notify', this.rootScope.MESSAGES.emailSent);
        },
        (error) => {
          this.rootScope.$emit('notify', this.rootScope.MESSAGES.emailSendingError);
        }
    ).finally(() => {
      this.processing = false;
    });
  }

  emailList() {
    let emailList = "";

    this.data.forEach(function(participant) {
      emailList = `${emailList}${participant.email},`;
    }, emailList);

    return emailList;
  }
}
