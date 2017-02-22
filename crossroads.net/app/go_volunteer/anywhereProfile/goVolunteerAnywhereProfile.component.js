import controller from './goVolunteerAnywhereProfile.controller';
import './goVolunteerAnywhereProfile.html';

export default function goVolunteerAnywhereProfileComponent() {
  let groupSearchComponent = {
    templateUrl: 'anywhereProfile/goVolunteerAnywhereProfile.html',
    controller,
    controllerAs: 'profile',
    bindToController: true
  };

  return groupSearchComponent;
}
