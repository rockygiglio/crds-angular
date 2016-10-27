import CONSTANTS from 'crds-constants';

export default class ServeTeamContainerController {
  /* @ngInject*/
  constructor(ServeTeamService, $q, ServeOpportunities) {
    // this.team; from binding
    this.isLeader = false;
    this.serveTeamService = ServeTeamService;
    this.ready = false;
    this.qApi = $q;
    this.serveOpportunities = ServeOpportunities;
  }

  $onInit() {
    const promises = [this.serveTeamService.getIsLeader(this.team.groupId), this.serveTeamService.getTeamRsvps(this.team)];
    this.qApi.all(promises).then((results) => {
      this.isLeader = results[0].isLeader;
      this.team = results[1];
      this.ready = true;
    });
  }

  memberClick(member) {

    this.onMemberClick({ $member: member });
  }

  memberRemove(member) {
    if (member.Opportunity_ID === 0) {
      // do something else
    } else {
      const rsvp = {};
      rsvp.contactId = member.contactId;
      rsvp.opportunityId = 0;
      rsvp.opportunityIds = _.pluck(this.team.serveOpportunities, 'Opportunity_ID');
      rsvp.eventTypeId = this.team.eventTypeId;
      rsvp.endDate = moment(this.oppServeDate, 'MM/DD/YYYY').format('X');
      rsvp.startDate = moment(this.oppServeDate, 'MM/DD/YYYY').format('X');
      rsvp.signUp = false;
      rsvp.alternateWeeks = false;
      _.pull(rsvp.opportunityIds, 0);

      this.serveOpportunities.SaveRsvp.save(rsvp).$promise.then((eventData) => {
        this.updateTeam(member, 0);
      });
    }
  }

  updateTeam(person, opp) {
    debugger;
    let signedUp = (opp === 0) ? CONSTANTS.SERVING_RESPONSES.NOT_AVAILABLE : CONSTANTS.SERVING_RESPONSES.AVAILABLE;

    _.forEach(this.team.serveOpportunities, (opportunity) => {
      _.remove(opportunity.rsvpMembers, (member) => {
        return member.participantId === person.participantId;
      });
    });

    let signedUpOpp = _.find(this.team.serveOpportunities, { Opportunity_ID: opp });
    signedUpOpp.rsvpMembers.push(
      {
        nickName: person.nickName,
        lastName: person.lastName,
        participantId: person.participantId,
        responseResultId: signedUp,
        opportunityId: signedUpOpp.opportunityId
      }
    );
  }
}
