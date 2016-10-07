import controller from './geolocation.controller';

GeolocationComponent.$inject = [];

export default function GeolocationComponent() {

  let geolocationComponent = {
    restrict: 'E',
    templateUrl: 'geolocation/geolocation.html',
    controller: controller,
    controllerAs: 'geolocation',
    bindToController: true
  }

  return geolocationComponent;
}