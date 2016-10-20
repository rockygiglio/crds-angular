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

  // $onChanges(changesObj)
  // {
  //   if(changesObj.servingOpportunities) {
  //     this.servingOpportunities = changesObj.servingOpportunities;
  //     this.loadTeamMembers();
  //   }
  // }

  $onInit()
  {
    debugger;
    this.loadTeamMembers();
  }

  loadTeamMembersSearch() {
        console.debug('Query team members');
        // TODO UI!!! IMPLEMENT THIS
        return [
          {
            id: 1001,
            name: 'Genie Simmons',
            email: 'gsimmons@gmail.com',
            phone: '513-313-5984',
            role: 'Leader'
          },
          {
            id: 1002,
            name: 'Holly Gennaro',
            email: 'hgennaro@excite.com',
            phone: '513-857-9587',
            role: null
          },
        ]
      }

  loadTeamMembers() {
      debugger;
      this.opportunities = this.splitMembers(this.opportunities);
      this.allMembers = [];

      _.forEach(this.servingOpportunities, (opportunity) => {
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