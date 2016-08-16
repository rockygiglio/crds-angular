import controller from './confirmRequest.controller';

ConfirmRequestComponent.$inject = [ ];

export default function ConfirmRequestComponent() {

  let confirmRequestComponent = {
    bindings: {
      group: '<',
      modalInstance: '<'
    },
    restrict: 'E',
    templateUrl: 'confirm_request/confirmRequest.html',
    controller: controller,
    controllerAs: 'confirmRequest',
    bindToController: true
  };

  return confirmRequestComponent;
}
