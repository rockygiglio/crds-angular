import CONSTANTS from 'crds-constants';

export default class ServeTeamMembersController {
  /*@ngInject*/
  constructor() {
    console.debug('Construct ServeTeamMembersController');
    //this.opportunities; from component binding
    this.rsvpNoMembers = [];
    this.rsvpYesLeaders = [];
    this.allMembers = [];
    this.selectedRole = undefined;
    this.ready = false;
  }

  $onInit()
  {
    debugger;
    this.loadTeamMembers();
  }

  loadTeamMembers() {
      this.opportunities = this.splitMembers(this.opportunities);
      this.allMembers = [];

      _.forEach(this.opportunities, (opportunity) => {
        this.addTeam((opportunity.Opportunity_Title + " " + opportunity.roleTitle), opportunity.rsvpMembers);
      });

      this.addTeam('Not Available', _.uniq(this.rsvpNoMembers, 'Participant_ID'));
  }

  splitMembers(opportunities) {
    _.forEach(opportunities, (opportunity) => {
      let partitionedArray = _.partition(opportunity.rsvpMembers, (member) => {return member.Response_Result_ID === CONSTANTS.SERVING_RESPONSES.NOT_AVAILABLE});
      this.rsvpNoMembers = this.rsvpNoMembers.concat(partitionedArray[0]);
      opportunity.rsvpMembers = partitionedArray[1];
    })
    return opportunities;
  }

  addTeam(teamName, members)
  {
    let team = {
      name: teamName,
      members: null
    };
    team.members = (members !== null && members.length > 0) ? members : undefined;
    this.allMembers.push(team);
  }

  memberClick(member) {
    console.debug('member click', member);
    this.onMemberClick({ $member: member });
  }

  memberRemove(member) {
    console.debug('member remove', member);
    this.onMemberRemove({ $member: member });
  }
}