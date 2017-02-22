export default class OrganizationsController {
  /* @ngInject */
  constructor() {
    this.viewReady = false;
  }

  $onInit() {
    this.viewReady = true;
  }
}
