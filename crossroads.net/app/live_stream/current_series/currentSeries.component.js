import controller from './currentSeries.controller';

CountdownComponent.$inject = [];

export default function CountdownComponent() {

  let currentSeriesComponent = {
    restrict: 'E',
    templateUrl: 'current_series/currentSeries.html',
    controller: controller,
    controllerAs: 'currentSeries',
    bindToController: true
  }

  return currentSeriesComponent;
}