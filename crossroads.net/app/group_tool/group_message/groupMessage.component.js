
import controller  from './groupMessage.controller';

GroupMessageComponent.$inject = [ ];

export default function GroupMessageComponent() {

  let GroupMessageComponent = {
    restrict: 'E',
   /* bindings: {
      person: '<',
      normalLoadingText: '@',
      loadingLoadingText: '@',
      cancel: '&',
      submit: '&',
      header: '@',
      subHeaderText: '<',
      contactText: '<',
      emailTemplateText: '<',
    },*/
    templateUrl: 'group_message/groupMessage.html', 
    controller: controller,
    controllerAs: 'groupMessage',
    bindToController: true
  };

  return GroupMessageComponent;

}