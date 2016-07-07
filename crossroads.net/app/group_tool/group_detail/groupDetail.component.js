import controller from './groupDetail.controller';

GroupDetailComponent.$inject = [];

export default function GroupDetailComponent() {

  let groupDetailComponent = {
    restrict: 'E',
    templateUrl: 'group_detail/groupDetail.html',
    controller: controller,
    controllerAs: 'groupDetail',
    bindToController: true
  };

  return groupDetailComponent;

}