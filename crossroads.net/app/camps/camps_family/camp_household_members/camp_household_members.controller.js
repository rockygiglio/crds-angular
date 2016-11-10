export default class CampHouseholdMembersController {
  /* @ngInject */
  constructor(CampsService) {
    this.campsService = CampsService;
  }

  // eslint-disable-next-line class-methods-use-this
  isSignedUp(member) {
    return member.signedUpDate !== null;
  }

  divClass(member) {
    if (!this.isSignedUp(member) && member.isEligible) {
      return 'col-sm-9 col-md-10';
    }
    return '';
  }

  pClass(member) {
    if (!this.isSignedUp(member)) {
      return 'flush-bottom';
    }

    return '';
  }
}
