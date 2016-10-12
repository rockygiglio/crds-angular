import CONSTANTS from 'crds-constants';

export default class ServeTeamMembersController {
  constructor() {
    console.debug('Construct ServeTeamMembersController');
    this.servingOpportunities = {};
    this.rsvpNoMembers = [];
    this.rsvpYesLeaders = [];
    this.allMembers = [];
  }

  $onInit()
  {
    debugger;
    this.servingOpportunities = this.team.serveOppertunities; // gets passed in from component attribute.

    this.servingOpportunities = this.addRsvpNoMembers(this.servingOpportunities);
    this.allMembers = [];

    this.addTeam('Leaders', this.rsvpYesLeaders);

    _.forEach(this.servingOpportunities, (opportunity) => {
      this.addTeam(opportunity.Opportunity_Title, opportunity.rsvpMembers);
    })

    this.addTeam('Not Available', this.rsvpNoMembers);

  }

  addRsvpNoMembers(opportunities) {
    let rsvpNoMembers = [];

    _.forEach(opportunities, (opportunity) => {
      let partitionedArray = _.partition(opportunity.rsvpMembers, (member) => {return member.Response_Result_ID === 2});
      this.rsvpNoMembers.concat(partitionedArray[0]);
      partitionedArray = _.partition(partitionedArray[1], (member) => {return member.Group_Role_ID === CONSTANTS.GROUP.ROLES.LEADER});
      this.rsvpYesLeaders.concat(partitionedArray[0])
      opportunity.rsvpMembers = partitionedArray[1];
    })
    return opportunities;
  }

  addTeam(teamName, members)
  {
    debugger;
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