export default class ServeTeamMembersController {
  constructor() {
    console.debug('Construct ServeTeamMembersController');
    this.team = {};
    //
    // !!!!
    // TODO REMOVE TEMP TEAM DATA
    //
    this.allMembers = [
      {
        name: 'Leaders',
        members: [ { name: 'Don Whitman' }]
      },
      {
        name: 'Service Setup',
        members: [ { name: 'Dave Groben' }, { name: 'Denny Smyth' }, { name: 'Andy Jones (SV)' } ]
      },
      {
        name: 'Service Member',
        members: [ ]
      },
      {
        name: 'Not Available',
        members: [ { name: 'Jimmy McCarthy' }]
      },
    ];
  }

  $onInit()
  {
    debugger;
    this.team;
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