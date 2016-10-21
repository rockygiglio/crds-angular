import CONSTANTS from 'crds-constants';

export default class ServeTeamLeaderController {
  /*@ngInject*/
  constructor(ServeTeamService) {
    console.debug('Construct ServeTeamLeaderController');
    //this.opportunities; from component binding
    //this.oppServeDate;
    //this.oppServeTime;
    this.selectedOpp = undefined;
    this.serveTeamService = ServeTeamService;
    this.model = {};
    this.formErrors = {from: false}
    this.datesDisabled = false;
    this.processing = false;
  }

  $onInit() {
    this.serveTeamService.getAllTeamMembersForLoggedInLeader(this.team.groupId).then((data) => {
      this.teamMembers = data;
    });
    _.each(this.team.serveOpportunities, (opp) => {
      opp.capacity = this.serveTeamService.getCapacity(opp, this.team.eventId);
    });
    debugger;
    this.frequencies = this.getFrequency();
    this.model.selectedFrequency = this.frequencies[0];
    this.populateDates();

  }

  populateDates() {
    switch (this.model.selectedFrequency.value) {
      case null:
        this.model.fromDt = null;
        this.model.toDt = null;
        this.datesDisabled = true;
        break;
      case 0:
        this.model.fromDt = this.oppServeDate;
        this.model.toDt = this.oppServeDate;
        this.datesDisabled = true;
        this.formErrors.from = false;
    }
  }

  getFrequency() {
    let dateTime = moment(this.oppServeDate + ' ' + this.oppServeTime);
    let weeklyLabel = moment(this.oppServeDate).format('dddd') + 's' + ' ' + dateTime.format('h:mma');

    let once = {
      value: 0,
      text: 'Once ' + dateTime.format('M/D/YYYY h:mma')
    };
    let everyWeek = {
      value: 1,
      text: 'Every Week ' + weeklyLabel
    };
    let everyOtherWeek = {
      value: 2,
      text: 'Every Other Week ' + weeklyLabel
    };

    return [once, everyWeek, everyOtherWeek];
  }

  saveRsvp() {
    var validForm = isFormValid();

    if (!validForm.valid) {
      $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      return false;
    }

    scope.processing = true;
    var rsvp = {};
    rsvp.contactId = scope.currentMember.contactId;
    rsvp.opportunityId = scope.currentMember.serveRsvp.roleId;
    rsvp.opportunityIds = _.map(scope.currentMember.roles, function (role) {
      return role.roleId;
    });

    rsvp.eventTypeId = scope.team.eventTypeId;
    rsvp.endDate = parseDate(scope.currentMember.currentOpportunity.toDt);
    rsvp.startDate = parseDate(scope.currentMember.currentOpportunity.fromDt);
    if (scope.currentMember.serveRsvp.roleId !== 0) {
      rsvp.signUp = true;
    } else {
      rsvp.signUp = false;
    }

    rsvp.alternateWeeks = (scope.currentMember.currentOpportunity.frequency.value === 2);
    ServeOpportunities.SaveRsvp.save(rsvp, function (updatedEvents) {
      if (rsvp.signUp) {
        $rootScope.$emit('notify', $rootScope.MESSAGES.SU2S_Saving_Message);
      } else {
        var saveMessage = 'You have indicated that [participant] is not available for [team] on [date]';
        saveMessage = saveMessage.replace('[participant]', scope.currentActiveTab);
        saveMessage = saveMessage.replace('[team]', scope.team.name);
        saveMessage = saveMessage.replace('[date]', scope.oppServeDate);
        growl.success(saveMessage);
      }

      scope.currentMember.serveRsvp.isSaved = true;
      scope.processing = false;
      updateCapacity();
      savePanel(scope.currentMember, true);
      $rootScope.$emit('updateAfterSave',
        { member: scope.currentMember, groupId: scope.team.groupId, eventIds: updatedEvents.EventIds });

      determineSaveButtonState();

      // should we reset the form to pristine
      if (!isFormDirty()) {
        scope.teamForm.$setPristine();
      }

      return true;
    }, function (err) {

      $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
      scope.processing = false;
      determineSaveButtonState();
      return false;
    });
  }

  leaderCancel(){
    this.model.fromDt = null;
    this.model.toDt = null;
    this.model.selectedOpp = null;
    this.cancel();
  }

  loadTeamMembersSearch($query) {
    console.debug('Query team members');
    return _.filter(this.teamMembers, (person) => {
     return person.displayName.toLowerCase()
       .includes($query.toLowerCase())
   })
  }
}