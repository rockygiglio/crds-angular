
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
    this.currentView = 'List';
    this.processing = false;
  }

  $onInit() {
    this.participantService.get().then((myParticipant) => {
      this.myParticipantId = myParticipant.ParticipantId;
      this.loadGroupParticipants();
    });
  }

  loadGroupParticipants() {
    this.groupService.getGroupParticipants(this.groupId).then((data) => {
      this.data = data.sort((a, b) => {
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

  setView(newView) {
    this.currentView = newView;
  }

  beginDeleteParticipant(participant) {
    this.deleteParticipant = participant;
    this.deleteParticipant.deleteMessage = '';
    this.setView('Delete');
  }

  cancelDeleteParticipant(participant) {
    participant.deleteMessage = undefined;
    this.deleteParticipant = undefined;
    this.setView('Edit');
  }

  removeParticipant(participant) {
    this.log.info(`Deleting participant: ${JSON.stringify(participant)}`);
    this.processing = true;
    this.groupService.removeGroupParticipant(this.groupId, participant).then(() => {
      _.remove(this.data, function(p) {
          return p.groupParticipantId === participant.groupParticipantId;
      });
      this.rootScope.$emit('notify', this.rootScope.MESSAGES.groupToolRemoveParticipantSuccess);
      this.setView('List');  
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