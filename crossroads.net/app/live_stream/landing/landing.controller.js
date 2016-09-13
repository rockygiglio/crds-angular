var WOW = require('wow.js/dist/wow.min.js');

export default class LandingController {
  /*@ngInject*/
  constructor($state, $rootScope) {
    this.state = $state;
    this.rootScope = $rootScope;

    new WOW({
      offset: 100,
      mobile: false
    }).init();
  }
}