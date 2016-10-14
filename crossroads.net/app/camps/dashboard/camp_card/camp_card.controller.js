/**
 * CampCardController:
 * Passed in via the component directive:
 *    attendee
 *    startDate
 *    endDate
 *    paymentRemaining
 */
class CampCardController {
  constructor() {}

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
