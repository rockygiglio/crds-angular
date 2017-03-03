import controller from './anywhereLeader.controller';
import './anywhereLeader.html';

export default function anywhereLeaderComponent() {
  const anywhereLeaderComponent = {
    templateUrl: 'anywhereLeader/anywhereLeader.html',
    controller
  };

  return anywhereLeaderComponent;
}
