export default class GroupDetailParticipantsController {
  /*@ngInject*/
  constructor(GroupService, ImageService, $state) {
    this.groupService = GroupService;
    this.imageService = ImageService;
    this.state = $state;

    this.groupId = this.state.params.groupId;
    this.ready = false;
    this.error = false;
    this.currentView = 'List';
  }

  $onInit() {
    var imageBaseUrl = this.imageService.ProfileImageBaseURL;
    this.groupService.getGroupParticipants(this.groupId).then((data) => {
      this.data = data;
      _.forEach(this.data.participants, function(participant) {
          participant.imageUrl = `${imageBaseUrl}${participant.contactId}`;
      });
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

  deleteParticipants() {
    // TODO Implement backend call to delete (end-date) participant
    _.remove(this.data.participants, function(participant) {
        return participant.deleted === true;
    });
    this.currentView = 'List';  
  }

  undeleteParticipants() {
    this.data.participants.map(p => p.deleted = false);
    this.currentView = 'List';  
  }
}