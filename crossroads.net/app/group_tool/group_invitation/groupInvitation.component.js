
import controller from './groupInvitation.controller';

GroupInvitationComponent.$inject = [];

export default function GroupInvitationComponent() {

  let groupDetailComponent = {
    restrict: 'E',
    templateUrl: 'group_invitation/groupInvitation.html',
    controller: controller,
    controllerAs: 'groupInvitation',
    bindToController: true
  };

  return GroupInvitationComponent;

}