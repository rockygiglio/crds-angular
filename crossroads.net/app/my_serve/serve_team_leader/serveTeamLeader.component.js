import ServeTeamLeaderController from './serveTeamLeader.controller';

export default function serveTeamLeaderComponent() {
  return {
    bindings: {
      team: '<',
      oppServeDate: '<',
      oppServeTime: '<'
    },
    templateUrl: 'serve_team_leader/serveTeamLeader.html',
    controller: ServeTeamLeaderController,
    controllerAs: 'serveTeamLeader'
  }
}