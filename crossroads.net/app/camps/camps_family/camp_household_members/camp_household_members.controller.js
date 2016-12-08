export default class CampHouseholdMembersController {
  /* @ngInject */
  constructor(CampsService, $state) {
    this.campsService = CampsService;
    this.state = $state;
  }

  signUp(member) {
    // Since we might be selected a new camper, ensure that the CampService does not have cached data
    // from the prior camper
    this.campsService.initializeCamperData();
    this.state.go('campsignup.application', { page: 'camper-info', contactId: member.contactId });
  }

  divClass(member) {
    if (!member.isSignedUp && member.isEligible) {
      return 'col-sm-9 col-md-10';
    }
    return '';
  }

  pClass(member) {
    if (!member.isSignedUp) {
      return 'flush-bottom';
    }

    return '';
  }
}
