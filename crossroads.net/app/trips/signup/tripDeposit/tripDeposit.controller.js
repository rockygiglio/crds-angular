/*ngInject*/
class TripDepositController {
  constructor($rootScope, $state,
              DonationService, GiveTransferService,
              GiveFlow, AUTH_EVENTS,
              TripDeposit, TripsSignupService, $stateParams, $window){
    this.viewReady = false;
    this.rootScope = $rootScope;
    this.state = $state;
    this.donationService = DonationService;
    this.dto = GiveTransferService;
    this.giveFlow = GiveFlow;
    this.AUTH_EVENTS = AUTH_EVENTS;
    this.tripDeposit = TripDeposit;
    this.signupService = TripsSignupService;
    this.stateParams = $stateParams;
    this.loadingDonor = false;
    this.window = $window;
    this.initialized = false;
  }

  $onDestroy() {
    this.window.onbeforeunload = null;
  }

  $onInit() {

    this.window.onbeforeunload = () => {
        return 'Data will be lost if you leave the page, are you sure?';
    };

    this.initialized = true;

    // make sure that an application exists and is valid
    if(!this.signupService.applicationValid) {
      this.state.go('tripsignup', { campaignId: this.stateParams.campaignId });
      return;
    }

    /*jshint unused:false */
    this.rootScope.$on('$stateChangeStart', (event, toState, toParams) => {

      if (toState && !/^tripdeposit.*/.test(toState.name)) {
        return;
      }
      this.dto.processing = true;
      if (!this.dto.initialized || toState.name === 'tripdeposit') {
         return;
      }

      if (!this.loadingDonor) {
        this.loadingDonor = true;
        var promise = this.donationService.transitionForLoggedInUserBasedOnExistingDonor(event, toState);
        promise.finally( () => {
          this.loadingDonor = false;
          this.viewReady = true;
        });
      }
    });

    /*jshint unused:false */
    this.rootScope.$on(this.AUTH_EVENTS.logoutSuccess, (event) =>  {
      this.dto.reset();
      this.state.go('home');
    });

    this.rootScope.$on('$stateChangeSuccess', (event, toState, toParams, fromState, fromParams) => {
      if (toState && !/^tripdeposit.*/.test(toState.name)) {
        return;
      }
      if (fromState.name === 'tripsignup.application.page' &&
          toState.name === 'tripdeposit' &&
          fromParams.stepId &&
          Number(fromParams.stepId) < 5
          ) {
        event.preventDefault(); 
        this.state.go('tripsignup', { campaignId: this.stateParams.campaignId });
        return;
      }
      if (fromState.name !== 'tripsignup.application.page' &&
          fromState.name !== 'tripdeposit.confirm' &&
            toState.name === 'tripdeposit' ) {
        this.state.go('tripsignup', { campaignId: this.stateParams.campaignId });
        return;
      }
      this.dto.processing = false;
      if ((!this.dto.initialized || toState.name === 'tripdeposit') &&
          toState.name !== this.giveFlow.thankYou) {
        event.preventDefault();
        this.initDefaultState();
        return;
      }
      if (toState.name === this.giveFlow.thankYou) {
        this.signupService.pageId = 'thanks';
        this.dto.initialized = false;
        this.window.onbeforeunload = null;
      } 
    });

    this.rootScope.$on('$stateChangeError', (event, toState, toParams) => {
      this.dto.processing = false;
    });

    this.initDefaultState();
  }

 

  initDefaultState() {

    let program = {
      ProgramId: this.signupService.programId,
      Name: this.signupService.programName
    };

    let campaign = {
      campaignId: this.signupService.campaign.id,
      campaignName: this.signupService.campaign.name,
      pledgeDonorId: this.signupService.donorId
    };

    this.tripDeposit.initDefaultState(program, campaign, this.signupService.depositAmount);
    this.state.go('tripdeposit.account',
                  {campaignId: this.stateParams.campaignId, contactId: this.stateParams.contactId});
  }

  remainingAmount() {
    return this.signupService.pledgeAmount - this.signupService.depositAmount;
  }

  saveApplication(shouldSubmitBank = '') {
    this.dto.processing = true;
    if (this.tripDeposit.applicationSaved) {
      this.saveDeposit(shouldSubmitBank);
    } else {
      var application = new this.signupService.TripApplication();
      application.contactId = this.signupService.person.contactId;
      application.pledgeCampaignId = this.signupService.campaign.id;
      application.pageTwo = this.signupService.page2;
      application.pageThree = this.signupService.page3;
      application.pageFour = this.signupService.page4;
      application.pageFive = this.signupService.page5;
      application.pageSix = this.signupService.page6;
      application.inviteGUID = this.stateParams.invite;
      application.depositInformation = this.signupService.depositInfo;
      application.depositInformation.donationAmount = this.signupService.depositAmount;
      application.depositInformation.donationDate = moment(new Date()).format('l');
      application.depositInformation.paymentMethod = this.getPaymentType();

      this.signupService.saveApplication((data) => {
        this.tripDeposit.applicationSaved = true;
        this.dto.campaign.pledgeDonorId = data.donorId;
        this.saveDeposit(shouldSubmitBank);
      }, () => {
        this.dto.processing = false;
        this.rootScope.$emit('notify', this.rootScope.MESSAGES.generalError);
      });
    }
  }

  saveDeposit(shouldSubmitBank) {
    /*jshint unused:false */
    if (shouldSubmitBank === 'submit')
    {
      this.donationService.submitBankInfo(this.tripForm);
    } else if (shouldSubmitBank === 'changed') {
      this.donationService.submitChangedBankInfo(this.tripForm);
    } else {
      this.donationService.confirmDonation(null, (confirmation) => {
        this.tripDeposit.applicationSaved = false;
      });
    }
  }

}


export default TripDepositController;
