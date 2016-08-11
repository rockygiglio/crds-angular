import controller from './groupDetail.about.controller';

GroupDetailAboutComponent.$inject = [ ];

export default function GroupDetailAboutComponent() {

  let groupDetailAboutComponent = {
    bindings: {
      data: '<',
      edit: '<',
      forInvitation: '<',
      forSearch: '<'
    },
    restrict: 'E',
    templateUrl: 'about/groupDetail.about.html',
    controller: controller,
    controllerAs: 'groupDetailAbout',
    bindToController: true
  };

  return groupDetailAboutComponent;

}