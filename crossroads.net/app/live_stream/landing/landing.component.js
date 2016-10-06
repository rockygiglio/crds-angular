import controller from './landing.controller';

LandingComponent.$inject = [];

export default function LandingComponent() {

  let landingComponent = {
    restrict: 'E',
    templateUrl: 'landing/landing.html',
    controller: controller,
    controllerAs: 'landing',
    bindToController: true
  }

  return landingComponent;
}