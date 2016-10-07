export default class ServeTeamMembersController {
  constructor() {
    console.debug('Construct ServeTeamMembersController');
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
}