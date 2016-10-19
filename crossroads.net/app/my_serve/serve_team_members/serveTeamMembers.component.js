import ServeTeamMembersController from './serveTeamMembers.controller';

export default function serveTeamMembersComponent() {
  return {
    bindings: {
      opportunities: '<',
      onMemberClick: '&',
      onMemberRemove: '&'
    },
    templateUrl: 'serve_team_members/serveTeamMembers.html',
    controller: ServeTeamMembersController,
    controllerAs: 'serveTeamMembers'
  }
}