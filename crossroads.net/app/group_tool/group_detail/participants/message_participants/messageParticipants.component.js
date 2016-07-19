import controller from './messageParticipants.controller';

MessageParticipantsComponent.$inject = [];

export default function MessageParticipantsComponent() {

  let messageParticipantsComponent = {
    bindings: {
      message: '<',
      cancelAction: '&',
      submitAction: '&',
      processing: '<'
    },
    restrict: 'E',
    templateUrl: 'message_participants/messageParticipants.html',
    controller: controller,
    controllerAs: 'messageParticipants',
    bindToController: true
  };

  return messageParticipantsComponent;

}