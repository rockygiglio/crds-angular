
import controller from './groupEmail.controller';

GroupEmailComponent.$inject = [];

export default function GroupEmailComponent() {

  let groupEmailComponent = {
    bindings: {
      message: '<',
      cancelAction: '&',
      submitAction: '&',
      header: '@',
      allowSubject: '<',
      processing: '<',
      process: '<'
    },
    restrict: 'E',
    templateUrl: 'group_email/groupEmail.html',
    controller: controller,
    controllerAs: 'groupEmail',
    bindToController: true
  };

  return groupEmailComponent;

}