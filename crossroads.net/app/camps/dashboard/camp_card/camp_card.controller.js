/**
 * CampCardController:
 * Passed in via the component directive:
 *    attendee
 *    startDate
 *    endDate
 *    paymentRemaining
 *    primary contact
 *    camperId
 *    campId
 *    campPrimaryContact
 */
class CampCardController {
  constructor($state, CampsService) {
    this.state = $state;
    this.campsService = CampsService;
    this.isResolving = true;
    this.amountDue = null;
  }

  $onInit() {
    this.campsService.getCampProductInfo(this.campId, this.camperId).then((res) => {
      this.amountDue = res.camperInvoice.paymentLeft;
      this.isPaidInFull = (this.amountDue <= 0);
    }).catch(() => {
      this.isPaidInFull = true;
    }).finally(() => {
      console.log(this.amountDue);
      this.isResolving = false;
    });
  }

  updateMedical() {
    this.campsService.initializeCamperData();
    this.state.go('campsignup.application', { page: 'medical-info', contactId: this.camperId, campId: this.campId, update: true });
  }

  makePayment() {
    this.campsService.initializeCamperData();
    this.state.go('campsignup.application', { page: 'camps-payment', contactId: this.camperId, campId: this.campId, update: true, redirectTo: 'mycamps' });
  }

  formatDate() {
    const startDateMoment = moment(this.startDate);
    const endDateMoment = moment(this.endDate);
    const monthDayStart = startDateMoment.format('MMMM Do');
    const monthDayEnd = endDateMoment.format('MMMM Do');
    const year = startDateMoment.format('YYYY');
    return `${monthDayStart} - ${monthDayEnd}, ${year}`;
  }

  formatAmountDue() {
    if (!this.amountDue) {
      return 'Error Retrieving Product Info';
    } else {
      return `$${this.amountDue}`;
    }
  }
}

export default CampCardController;
