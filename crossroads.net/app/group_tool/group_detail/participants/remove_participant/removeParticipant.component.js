import controller from './removeParticipant.controller';

RemoveParticipantController.$inject = [];

export default function RemoveParticipantController() {

  let removeParticipantController = {
    bindings: {
      participant: '<',
      cancelAction: '&',
      submitAction: '&',
      processing: '<'
    },
    restrict: 'E',
    templateUrl: 'participant_card/removeParticipant.html',
    controller: controller,
    controllerAs: 'removeParticipant',
    bindToController: true
  };

  return removeParticipantController;

}