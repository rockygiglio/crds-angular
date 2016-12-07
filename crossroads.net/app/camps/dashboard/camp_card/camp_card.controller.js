/**
 * CampCardController:
 * Passed in via the component directive:
 *    attendee
 *    startDate
 *    endDate
 *    paymentRemaining
 *    primary contact
 */
class CampCardController {
  constructor($state, CampsService) {
    this.state = $state;
    this.campsService = CampsService;
  }

  updateMedical() {
    this.campsService.initializeCamperData();
    this.state.go('campsignup.application', { page: 'medical-info', contactId: this.camperId, campId: this.campId, update: true });
  }

  formatDate() {
    const startDateMoment = moment(this.startDate);
    const endDateMoment = moment(this.endDate);
    const monthDayStart = startDateMoment.format('MMMM Do');
    const monthDayEnd = endDateMoment.format('MMMM Do');
    const year = startDateMoment.format('YYYY');
    return `${monthDayStart} - ${monthDayEnd}, ${year}`;
  }
}

export default CampCardController;
