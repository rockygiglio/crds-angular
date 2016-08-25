(function() {
  'use strict';

  module.exports = TripsSignupService;

  TripsSignupService.$inject = ['$resource', '$location', '$log', '$stateParams'];

  function TripsSignupService($resource, $location, $log, $stateParams) {
    var signupService = {
      activate: activate,
      pages: [],
      reset: reset,
      TripApplication: $resource(__API_ENDPOINT__ + 'api/trip-application'),
      CreateTripParticipant: $resource(__API_ENDPOINT__+ 'api/trip-participant'),
      CampaignInfo: $resource(__API_ENDPOINT__ + 'api/trip-application/:contactId/:campaignId'),
      donorId: null,
      programId: null,
      programName: null,
      pledgeAmount: null,
      depositAmount: null,
      progressLabel: null,
      applicationValid: false,
      isScholarshipped: false,
      saveApplication: saveApplication
    };

    function activate() {
      $log.debug('signup service activate');

      if (signupService.page2 === undefined) {
        signupService.page2 = page2();
      }

      if (signupService.page3 === undefined) {
        signupService.page3 = page3();
      }

      if (signupService.page4 === undefined) {
        signupService.page4 = page4();
      }

      if (signupService.page5 === undefined) {
        signupService.page5 = page5();
      }

      if (signupService.page6 === undefined) {
        signupService.page6 = page6();
      }

      setupProps();
    }

    function setupProps() {
      //relying on Pledge Campaign Nickname field feels very fragile, is there another way?
      signupService.friendlyPageTitle = signupService.campaign.nickname;
      switch (signupService.campaign.nickname) {
        case 'NOLA':
          signupService.numberOfPages = 5;
          break;
        case 'South Africa':
          signupService.numberOfPages = 6;
          break;
        case 'India':
          signupService.numberOfPages = 6;
          signupService.whyPlaceholder = 'Please be specific. ' +
            'In instances where we have a limited number of spots, we strongly consider responses to this question.';
          break;
        case 'Nicaragua':
          signupService.numberOfPages = 6;
          break;
      }

    }

    function reset(campaign) {
      signupService.campaign = campaign;
      signupService.ageLimitReached = false;
      signupService.contactId = '';
      signupService.currentPage = 1;
      signupService.numberOfPages = 0;
      signupService.pageHasErrors = true;
      signupService.privateInvite = $location.search().invite;

      signupService.page2 = page2();
      signupService.page3 = page3();
      signupService.page4 = page4();
      signupService.page5 = page5();
      signupService.page6 = page6();
    }

    function page2() {
      return {
        guardianFirstName: null,
        guardianLastName: null,
        referral: null,
        conditions: null,
        why: null,
      };
    }

    function page3() {
      return {
        emergencyContactFirstName: null,
        emergencyContactLastName: null,
        emergencyContactEmail: null,
        emergencyContactPrimaryPhone: null,
        emergencyContactSecondaryPhone: null
      };
    }

    function page4() {
      return {
        groupCommonName: null,
        roommateFirstChoice: null,
        roommateSecondChoice: null,
        supportPersonEmail: null,
        interestedInGroupLeader: null,
        whyGroupLeader: null,
      };
    }

    function page5() {
      return {
        sponsorChildInNicaragua: null,
        sponsorChildFirstName: null,
        sponsorChildLastName: null,
        sponsorChildNumber: null,
        sponsorChildTown: null,
        nolaFirstChoiceWorkTeam: null,
        nolaFirstChoiceExperience: null,
        nolaSecondChoiceWorkTeam: null,
      };
    }

    function page6() {
      return {
        experienceAbroad: null,
        describeExperienceAbroad: null,
        pastAbuseHistory: null,
        validPassport: null
      };
    }

    function saveApplication(success, error) {
      var application = new signupService.TripApplication();
      application.contactId = signupService.person.contactId;
      application.pledgeCampaignId = signupService.campaign.id;
      application.pageTwo = signupService.page2;
      application.pageThree = signupService.page3;
      application.pageFour = signupService.page4;
      application.pageFive = signupService.page5;
      application.pageSix = signupService.page6;
      application.inviteGUID = $stateParams.invite;

      /*jshint unused:false */
      application.$save((data) => {
          _.each(signupService.familyMembers, (f) => {
            if (f.contactId === Number($stateParams.contactId)) {
              f.signedUp = true;
              f.signedUpDate = new Date();
            }
          });
          success();
        }, () => {
        error();
      });
    }

    return signupService;
  }
})();
