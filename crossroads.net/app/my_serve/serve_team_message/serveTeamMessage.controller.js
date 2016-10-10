export default class ServeTeamMessageController {
  /* @ngInject */
  constructor() {
    console.debug('Serve Team Message controller');
    this.processing = false;

    // TODO !!! REPLACE MOCK DATA
    this.teams = [
      {
        name: 'FI Oakley Coffee Team',
        count: 43
      },
      {
        name: 'FI Florence Info Center Team',
        count: 37
      }
    ];
  }

  submit() {
    this.processing = true;
  }
}
