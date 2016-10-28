import forEach from 'lodash/collection/forEach';

export default class CampWaiversController {
  /* @ngInject */
  constructor($stateParams, CampsService) {
    this.$stateParams = $stateParams;
    this.campsService = CampsService;

    this.signature = null;
    this.camper = {
      firstName: 'John',
      lastName: 'Doe'
    };
  }

  $onInit() {
    this.waivers = this.campsService.waivers;
  }

  getFullName() {
    return `${this.camper.firstName} ${this.camper.lastName}`;
  }

  submitWaivers() {
    const approved = this.signature === 'guardian' || this.signature === 'self';
    const params = [];

    if (this.waivers.length > 0) {
      forEach(this.waivers, (waiver) => {
        params.push({
          waiverId: waiver.waiverId,
          approved
        });
      });

      this.campsService.submitWaivers(this.$stateParams.campId, this.$stateParams.contactId, params);
    }
  }
}
