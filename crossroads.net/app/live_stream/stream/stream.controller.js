export default class StreamController {
  /*@ngInject*/
  constructor($state, $rootScope) {
    this.state = $state;
    this.rootScope = $rootScope;
  }
}