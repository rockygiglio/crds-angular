import forEach from 'lodash/collection/forEach';

export default class CampWaiversController {
  /* @ngInject */
  constructor($stateParams, $rootScope, CampsService) {
    // Injectables
    this.$stateParams = $stateParams;
    this.rootScope = $rootScope;
    this.campsService = CampsService;

    // Constants
    this.GUARDIAN = 'guardian';
    this.APPROVE_LATER = 'approveLater';
    this.SELF = 'self';

    // Variables
    this.signature = null;
    this.camper = {
      firstName: 'John',
      lastName: 'Doe'
    };

    this.processing = false;
  }

  $onInit() {
    this.waivers = this.campsService.waivers;

    // Determine if waivers have been previously signed
    let signee = 0;
    let accepted = true;

    if (this.waivers.length > 0) {
      forEach(this.waivers, (waiver) => {
        signee = waiver.signee;
        accepted = accepted && waiver.accepted;
      });
    }

    if (signee > 0) {
      if (accepted) {
        if (this.$stateParams.contactId === signee) {
          this.signature = this.SELF;
        } else {
          this.signature = this.GUARDIAN;
        }
      } else {
        this.signature = this.APPROVE_LATER;
      }
    }
  }

  getFullName() {
    return `${this.camper.firstName} ${this.camper.lastName}`;
  }

  submitWaivers() {
    if (this.form.$invalid) {
      return;
    }

    const approved = this.signature === this.GUARDIAN || this.signature === this.SELF;
    const params = [];

    forEach(this.waivers, (waiver) => {
      params.push({
        waiverId: waiver.waiverId,
        approved
      });
    });

    this.processing = true;
    this.campsService.submitWaivers(this.$stateParams.campId, this.$stateParams.contactId, params)
      .then(() => {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.successfulSubmission);
      }, () => {
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      }).finally(() => {
        this.processing = false;
      });
  }
}
