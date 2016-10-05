var WOW = require('wow.js/dist/wow.min.js');

export default class LandingController {
  constructor() {
    new WOW({
      offset: 100,
      mobile: false
    }).init();
  }
}