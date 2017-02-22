import controller from './goVolunteerAnywhereProfile.controller';
import './goVolunteerAnywhereProfile.html';

export default function goVolunteerAnywhereProfileComponent() {
  const anywhereComponent = {
    templateUrl: 'anywhereProfile/goVolunteerAnywhereProfile.html',
    controller,
    bindToController: true
  };

  return anywhereComponent;
}
