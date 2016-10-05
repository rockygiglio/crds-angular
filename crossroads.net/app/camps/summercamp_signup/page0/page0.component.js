import controller from './page0.controller';

EndGroupController.$inject = [ ];

export default function page0Controller() {

  let page0Controller = {
    bindings: {
      data: '<',
    },
    restrict: 'E',
    templateUrl: 'page0/page0.html',
    controller: controller,
    controllerAs: 'page0',
    bindToController: true
  };

  return page0Controller;

}