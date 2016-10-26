export default class CampWaiversController {
  /* @ngInject */
  constructor(CampsService) {
    this.campsService = CampsService;
    this.signature = null;
    this.camper = {
      firstName: 'John',
      lastName: 'Doe'
    }
  }

  $onInit() {
    this.waivers = this.campsService.waivers;
  }

  getFullName() {
    return this.camper.firstName + ' ' + this.camper.lastName;
  }
}
