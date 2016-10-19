import ServeTeamLeaderController from './serveTeamLeader.controller';

export default function serveTeamLeaderComponent() {
  return {
    bindings: {
      team: '<'
    },
    templateUrl: 'serve_team_leader/serveTeamLeader.html',
    controller: ServeTeamLeaderController,
    controllerAs: 'serveTeamLeader'
  }
}