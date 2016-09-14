import controller from './groupSearchFilterRenderer.controller';

GroupSearchFilterRendererComponent.$inject = [];

export default function GroupSearchFilterRendererComponent() {

  let groupSearchFilterRendererComponent = {
    bindings: {
      value: '<',
    },
    restrict: 'E',
    templateUrl: 'filter_renderer/groupSearchFilterRenderer.html',
    controller: controller,
    controllerAs: 'groupSearchFilterRenderer',
    bindToController: true
  };

  return groupSearchFilterRendererComponent;

}
