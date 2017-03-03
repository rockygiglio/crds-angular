import controller from './goVolunteerProjectCard.controller';
import './goVolunteerProjectCard.html';

export default function goVolunteerProjectCardComponent() {
  const component = {
    templateUrl: 'projectCard/goVolunteerProjectCard.html',
    controller,
    controllerAs: 'card',
    bindToController: true,
    bindings: {
      project: '<'
    }
  };

  return component;
}
