export default class ServeTeamMessageController {
  /* @ngInject */
  constructor() {
    console.debug('Serve Team Message controller');
    this.processing = false;
    this.selection = -1;
    this.individuals = [];

    // TODO !!! REPLACE MOCK DATA
    this.teams = [
      {
        name: 'FI Oakley Coffee Team',
        count: 43
      },
      {
        name: 'FI Florence Info Center Team',
        count: 37
      },
      {
        name: 'Wes Donaldson',
        isLeader: true
      }
    ];
  }

  loadIndividuals($query) {
    return [
      {
        id: 1001,
        name: 'Genie Simmons',
        email: 'gsimmons@gmail.com',
        role: 'Leader'
      },
      {
        id: 1002,
        name: 'Holly Gennaro',
        email: 'hgennaro@excite.com',
        role: null
      },
    ]
  }

  submit() {
    this.processing = true;
  }
}
