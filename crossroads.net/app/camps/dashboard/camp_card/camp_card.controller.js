/**
 * CampCardController:
 * Passed in via the component directive:
 *    attendee
 *    startDate
 *    endDate
 *    paymentRemaining
 */
class CampCardController {
  constructor($stateParams, $state) {
    this.stateParams = $stateParams;
    this.state = $state;
  }

  updateMedical() {
    // build the link and navigate
    // this.state.go('campsignup.medical', { eventId: this.stateParams.eventId, contactId: this.stateParams.contactId });

    let camperId = 1;
    this.state.go('campsignup.application', { page: 'product-summary', camperId });
  }

  formatDate() {
    let startDateMoment = moment(this.startDate);
    let endDateMoment = moment(this.endDate);
    let monthDayStart = startDateMoment.format('MMMM Do');
    let monthDayEnd = endDateMoment.format('MMMM Do');
    let year = startDateMoment.format('YYYY');
    return `${monthDayStart} - ${monthDayEnd}, ${year}`;
  }
}

export default CampCardController;
