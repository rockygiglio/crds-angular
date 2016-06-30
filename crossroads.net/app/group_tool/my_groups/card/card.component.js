import controller  from './card.controller';

CardComponent.$inject = [ ];

export default function CardComponent() {

  let cardComponent = {
    restrict: 'E',
    templateUrl: 'card/card.html',
    controller: controller,
    controllerAs: 'card',
    bindToController: true
  };

  return cardComponent;

}
