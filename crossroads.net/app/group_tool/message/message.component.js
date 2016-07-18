
import controller  from './message.controller';

MessageComponent.$inject = [ ];

export default function MessageComponent() {

  let messageComponent = {
    bindings: {
      person: '<',
      normalLoadingText: '@',
      loadingLoadingText: '@',
      cancel: '&',
      submit: '&',
      header: '@',
      subHeaderText: '<',
      contactText: '<',
      emailTemplateText: '<',
    },
    templateUrl: 'message/message.html',
    controller: controller,
    controllerAs: 'message',
    bindToController: true
  };

  return messageComponent;

}