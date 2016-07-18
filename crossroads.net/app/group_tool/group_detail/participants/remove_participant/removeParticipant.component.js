import controller from './removeParticipant.controller';

RemoveParticipantComponent.$inject = [];

export default function RemoveParticipantComponent() {

  let removeParticipantComponent = {
    bindings: {
      participant: '<',
      cancelAction: '&',
      submitAction: '&',
      processing: '<'
    },
    restrict: 'E',
    templateUrl: 'remove_participant/removeParticipant.html',
    controller: controller,
    controllerAs: 'removeParticipant',
    bindToController: true
  };

  return removeParticipantComponent;

}