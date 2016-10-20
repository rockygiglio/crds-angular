import ServeTeamContainerController from './serveTeamContainer.controller';

export default function serveTeamContainerComponent() {
  return {
    bindings: {
      team: '<',
      oppServeDate: '<',
      oppServeTime: '<',
      cancel: '&'
    },
    templateUrl: 'serve_team_container/serveTeamContainer.html',
    controller: ServeTeamContainerController,
    controllerAs: 'serveTeamContainer'
  }
}