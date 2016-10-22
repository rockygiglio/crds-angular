import CONSTANTS from 'crds-constants';

export default class ServeTeamLeaderController {
    /*@ngInject*/
    constructor(ServeTeamService, ServeOpportunities, $rootScope, growl) {
        console.debug('Construct ServeTeamLeaderController');
        //this.opportunities; from component binding
        //this.oppServeDate;
        //this.oppServeTime;
        this.selectedOpp = undefined;
        this.serveTeamService = ServeTeamService;
        this.serveOpportunities = ServeOpportunities;
        this.rootScope = $rootScope;
        this.growl = growl;
        this.model = {};
        this.formErrors = { from: false }
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
        //var validForm = isFormValid();

        //if (!validForm.valid) {
        //  $rootScope.$emit('notify', $rootScope.MESSAGES.generalError);
        //  return false;
        //}
        this.processing = true;
        let signUp = (this.model.selectedOpp !== 0) ? true : false;
        _.forEach(this.individuals, (person) => {
            var rsvp = {};
            rsvp.contactId = person.contactId;
            rsvp.opportunityId = this.model.selectedOpp;
            rsvp.opportunityIds = (signUp) ? [this.model.selectedOpp] : _.pluck(this.team.serveOpportunities, 'Opportunity_ID');
            rsvp.eventTypeId = this.team.eventTypeId;
            rsvp.endDate = moment(this.model.toDt, 'MM/DD/YYYY').format('X');
            rsvp.startDate = moment(this.model.fromDt, 'MM/DD/YYYY').format('X');
            rsvp.signUp = signUp;
            rsvp.alternateWeeks = (this.model.selectedFrequency.value === 2);
            this.serveOpportunities.SaveRsvp.save(rsvp);
            this.updateTeam(person, this.model.selectedOpp);
        });

        if (signUp)
            this.rootScope.$emit('notify', this.rootScope.MESSAGES.SU2S_Saving_Message);
        else {
            var saveMessage = `You have indicated that the participants are not available for ${this.team.name} on ${this.oppServeDate}`;
            this.growl.success(saveMessage);
        }


        this.processing = false;
    }

    updateTeam(person, opp) {
        let signedUp = (opp === 0) ? 2 : 1;

        _.forEach(this.team.serveOpportunities, (opportunity) => {
            _.remove(opportunity.rsvpMembers, (member) => {
                return member.Participant_ID == person.participantId;
            });
        });

        let signedUpOpp = _.find(this.team.serveOpportunities, { Opportunity_ID: opp });
        signedUpOpp.rsvpMembers.push(
            {
                NickName: person.nickName,
                Last_Name: person.lastName,
                Participant_ID: person.participantId,
                Response_Result_ID: signedUp,
                Opportunity_ID: signedUpOpp.Opportunity_ID
            }
        );
    }

    leaderCancel() {
        this.cancel();
        this.model.fromDt = null;
        this.model.toDt = null;
        this.model.selectedOpp = null;
    }

    loadTeamMembersSearch($query) {
        console.debug('Query team members');
        return _.filter(this.teamMembers, (person) => {
            return person.displayName.toLowerCase()
                .includes($query.toLowerCase())
        })
    }
}