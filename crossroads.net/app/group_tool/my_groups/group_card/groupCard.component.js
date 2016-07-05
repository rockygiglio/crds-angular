import controller  from './groupCard.controller';

GroupCardComponent.$inject = [ ];

export default function GroupCardComponent() {

  let groupCardComponent = {
    restrict: 'E',
    scope: {
      detail: '='
    },
    templateUrl: 'group_card/groupCard.html',
    controller: controller,
    controllerAs: 'groupCard',
    bindToController: true
  };

  return groupCardComponent;

}
