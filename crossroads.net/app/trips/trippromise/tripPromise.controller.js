export default class TripPromiseController {
  /* @ngInject() */
  constructor() {
    this.processing = false;
    this.tripPromiseForm = {};

    this.trip = {
      eventParticipantId: 7875336,
      tripStartDate: 'May 20, 2018',
      tripEnd: 'May 26, 2018',
      tripName: '2018 May BAHAMAS Angie Trip',
      fundraisingDays: 312,
      fundraisingGoal: 1000,
      totalRaised: 1101,
      tripGifts: [{
        donationDistributionId: 51802762,
        donorId: 7722808,
        donorNickname: 'Transfer',
        donorLastName: 'Transfer',
        donorEmail: null,
        donationAmount: 1001,
        donationDate: '7/18/2017',
        registeredDonor: false,
        anonymous: false,
        messageSent: false,
        paymentTypeId: 13
      }, {
        donationDistributionId: 51801849,
        donorId: 7736825,
        donorNickname: 'Chris',
        donorLastName: 'Tallent',
        donorEmail: 'chris@provenedge.com',
        donationAmount: 100,
        donationDate: '7/11/2017',
        registeredDonor: true,
        anonymous: false,
        messageSent: false,
        paymentTypeId: 4
      }],
      eventParticipantLastName: 'Tallent',
      eventParticipantFirstName: 'Chris'
    };
  }

  submit() {
    this.processing = true;
  }

  cancel() {
  }
}
