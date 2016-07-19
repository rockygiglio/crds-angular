
export default class GroupDetailParticipantsController {
  /*@ngInject*/
  constructor(GroupService, ImageService, $state, $log, ParticipantService, $rootScope) {
    this.groupService = GroupService;
    this.imageService = ImageService;
    this.state = $state;
    this.log = $log;
    this.participantService = ParticipantService;
    this.rootScope = $rootScope;

    this.groupId = this.state.params.groupId;
    this.ready = false;
    this.error = false;
    this.processing = false;

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
  }

  loadGroupParticipants() {
    this.groupService.getGroupParticipants(this.groupId).then((data) => {
      this.data = data.slice().sort((a, b) => {
        return(a.compareTo(b));
      });
      this.data.forEach(function(participant) {
        participant.me = participant.participantId === this.myParticipantId;
        participant.imageUrl = `${this.imageService.ProfileImageBaseURL}${participant.contactId}`;
      }, this);

      this.ready = true;
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
    debugger;
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
}